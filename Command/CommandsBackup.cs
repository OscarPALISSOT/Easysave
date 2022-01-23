using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using EasySave.Model;
using System.IO;
using System.Threading;
using System.Text.Json;

namespace EasySave.Command
{
    static class CommandsBackup
    {

        private static object LOCK = new object();

        /// <summary>
        /// Execute the BackupWork chosen
        /// </summary>
        /// <param name="saveWork">The BackupWork chosen</param>
        private static void ExecuteBackup(SaveWork saveWork)
        {
            saveWork.State.Progression = 0;
            saveWork.State.NbFilesLeftToDo = saveWork.State.TotalFileToCopy;
            saveWork.State.TotalRemainingSize = saveWork.State.TotalDirectorySize;

            List<string> priorityFiles = new List<string>();
            List<string> filesList = new List<string>();

            IsTherePriority(saveWork, priorityFiles, filesList);

            void SaveFile(string path)
            {
                FileInfo fi = new FileInfo(path);
                saveWork.State.FileSize = fi.Length;
                bool process = Commands.IsProcessRunning(Commands.GetAllBusinessSoftware());

                while (process)
                {
                    process = Commands.IsProcessRunning(Commands.GetAllBusinessSoftware());
                }

                bool priorityFilesSaved = IsAllPriorityFilesSaved();
                while (!priorityFilesSaved)
                {
                    priorityFilesSaved = IsAllPriorityFilesSaved();
                }

                Stopwatch transferTime = new Stopwatch();

                transferTime.Start();
                List<string> extensionList = new List<string>();
                extensionList = Commands.GetAllExtensionToCrypt();
                long fileEncryptionTime = 0;
                bool crypt = false;
                foreach (var extension in extensionList)
                {
                    if (extension == Path.GetExtension(path))
                    {
                        crypt = true;
                    }
                }
                if (crypt)
                {
                    Stopwatch encryptionTime = new Stopwatch();
                    encryptionTime.Start();
                    long exitCrypt = EncryptDecrypt(path, path.Replace(saveWork.Info.FileSource, saveWork.Info.FileTarget));
                    encryptionTime.Stop();
                    if (exitCrypt == -1)
                    {
                        fileEncryptionTime = -1;
                    }
                    else
                    {
                        fileEncryptionTime = encryptionTime.ElapsedMilliseconds;
                    }
                }
                else
                {
                    File.Copy(path, path.Replace(saveWork.Info.FileSource, saveWork.Info.FileTarget), true);
                }
                long length = new FileInfo(Path.Combine(saveWork.Info.FileSource, path)).Length;
                LogsCommands logCom = new LogsCommands();
                DateTime time = DateTime.Now;
                saveWork.State.NbFilesLeftToDo--;
                saveWork.State.TotalRemainingSize -= saveWork.State.FileSize;
                saveWork.State.Progression = 100-((double)saveWork.State.TotalRemainingSize * 100 / saveWork.State.TotalDirectorySize);
                transferTime.Stop();
                long fileTransferTime = transferTime.ElapsedMilliseconds;
                logCom.AddLogs(saveWork.Info.Name, path, path.Replace(saveWork.Info.FileSource, saveWork.Info.FileTarget), saveWork.State.State, length, fileTransferTime, saveWork.State.NbFilesLeftToDo, fileEncryptionTime, saveWork.State.Progression);
                UpdateSate(saveWork);
            }

            void SaveListFile(SaveWork saveWork, List<string> files)
            {
                if (File.Exists(saveWork.Info.FileSource))
                {
                    //error if path is a file and not a directory
                    string error = "file";
                    return;
                }
                else if (saveWork.Info.Full == true)
                {

                    saveWork.State.State = 2;
                    //full backup
                    //Create all of the directories
                    foreach (string dirPath in Directory.GetDirectories(saveWork.Info.FileSource, "*", SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(dirPath.Replace(saveWork.Info.FileSource, saveWork.Info.FileTarget));
                    }

                    //Copy all the files & Replaces any files with the same name
                    foreach (string path in files)
                    {
                        SaveFile(path);
                    }
                }
                else
                {
                    saveWork.State.State = 2;
                    //differencial backup
                    //Create all of the directories
                    foreach (string dirPath in Directory.GetDirectories(saveWork.Info.FileSource, "*", SearchOption.AllDirectories))
                    {
                        if (!Directory.Exists(dirPath.Replace(saveWork.Info.FileSource, saveWork.Info.FileTarget)))
                        {
                            Directory.CreateDirectory(dirPath.Replace(saveWork.Info.FileSource, saveWork.Info.FileTarget));
                        }

                    }
                    //Copy all the files & Replaces any files with the same name
                    foreach (string path in files)
                    {
                        if (!File.Exists(path.Replace(saveWork.Info.FileSource, saveWork.Info.FileTarget)))
                        {
                            SaveFile(path);
                        }
                    }
                }
            }

            if (saveWork.Priority)
            {
                SaveListFile(saveWork, priorityFiles);
                saveWork.Priority = false;
            }

            SaveListFile(saveWork, filesList);
        }

        private static void UpdateSate(SaveWork saveWork)
        {
            lock (LOCK)
            {
                var MyIni = new IniFile();
                List<SaveWork> BackupsUpdated = GetAllBackups();
                SaveWork BackupToDel = GetBackup(saveWork.Name);
                foreach (var backup in BackupsUpdated)
                {
                    if (backup.Name == saveWork.Name)
                    {
                        BackupToDel = backup;
                    }
                }
                BackupsUpdated.Remove(BackupToDel);
                BackupsUpdated.Add(saveWork);
                List<SaveState> BackupsState = new List<SaveState>();
                foreach (var backup in BackupsUpdated)
                {
                    BackupsState.Add(backup.State);
                }
                var options = new JsonSerializerOptions { WriteIndented = true };
                var jsonStringState = JsonSerializer.Serialize(BackupsState, options);
                File.WriteAllText(MyIni.Read("SaveState"), jsonStringState);
            }
        }

        /// <summary>
        /// Tell if there is a priority file in files to save
        /// </summary>
        /// <param name="saveWork"></param>
        /// <param name="priorityFiles"></param>
        /// <param name="filesList"></param>
        private static void IsTherePriority(SaveWork saveWork, List<string> priorityFiles, List<string> filesList)
        {
            List<string> priorityExtensions = Commands.GetAllPriorityFile();

            List<string> filesToRemove= new List<string>();

            string[] files = Directory.GetFiles(saveWork.Info.FileSource, "*.*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                filesList.Add(file);
            }


            foreach (var file in filesList)
            {
                foreach (var priorityExtension in priorityExtensions)
                {
                    if (Path.GetExtension(Path.Combine(saveWork.Info.FileSource, file)) == priorityExtension)
                    {
                        priorityFiles.Add(file);
                        filesToRemove.Add(file);
                    }
                }
            }

            foreach (var file in filesToRemove)
            {
                filesList.Remove(file);
            }

            if (priorityFiles.Count > 0)
            {
                saveWork.Priority = true;
            }
        }

        /// <summary>
        /// Tell if all priority files are saved
        /// </summary>
        /// <returns>true if finiched or false if not </returns>
        public static bool IsAllPriorityFilesSaved()
        {
            List<SaveWork> Backups = GetAllBackups();
            foreach (var backup in Backups)
            {
                if (backup.Priority)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Overload of the ExecuteBackup function, execute one or several backups chosen
        /// </summary>
        /// <param name="Backups">The list of SaveWork(s) chosen to execute</param>
        public static void ExecuteBackup(List<SaveWork> Backups)
        {
            foreach (var backup in Backups)
            {
                Random rnd = new Random();
                Thread.Sleep(rnd.Next(5, 15));
                ExecuteThread(backup);
            }
        }

        /// <summary>
        /// Start a thread which execute a savework
        /// </summary>
        /// <param name="saveWork">The savework to do by the thread</param>
        public static void ExecuteThread(SaveWork saveWork)
        {
            Thread Tsave = new Thread(new ThreadStart(
                () =>
                {
                    ExecuteBackup(saveWork);
                }
                )
            );
            Tsave.Start();
        }

        /// <summary>
        /// Create a BackupWork
        /// </summary>
        /// <param name="name">Name of the BackupWork</param>
        /// <param name="fileSource">Source file to copy</param>
        /// <param name="fileTarget">Destination file to paste the source file data</param>
        /// <param name="type">Type of backup (Full=True Differential=False)</param>
        public static void CreateBackup(string name, string fileSource, string fileTarget, bool type)
        {
            var MyIni = new IniFile();
            List<SaveInfo> InfoList = GetBackUpsInfo();
            List<SaveState> StateList = GetBackUpsState();
            foreach (var nameTest in InfoList)
            {
                if (nameTest.Name == name)
                {
                    string error = "SaveWorkAlreadyExists";
                    return;
                }
            }
            SaveWork saveWork = new SaveWork(name, fileSource, fileTarget, type);

            InfoList.Add(saveWork.Info);
            StateList.Add(saveWork.State);
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonStringInfo = JsonSerializer.Serialize(InfoList, options);
            File.WriteAllText(MyIni.Read("SavePath"), jsonStringInfo);
            var jsonStringState = JsonSerializer.Serialize(StateList, options);
            File.WriteAllText(MyIni.Read("SaveState"), jsonStringState);

        }

        /// <summary>
        /// Delete the BackupWork chosen
        /// </summary>
        /// <param name="name">Name of the BackupWork to delete</param>
        public static void DeleteBackup(string name)
        {
            var MyIni = new IniFile();
            List<SaveWork> backups = GetAllBackups();
            List<SaveInfo> BackupsInfo = new List<SaveInfo>();
            List<SaveState> BackupsState = new List<SaveState>();
            SaveWork backupToDelete = GetBackup(name);
            foreach (var backup in backups)
            {
                if (backup.Name == name)
                {
                    backupToDelete = backup;
                }
            }
            backups.Remove(backupToDelete);
            foreach (var backup in backups)
            {
                BackupsInfo.Add(backup.Info);
                BackupsState.Add(backup.State);
            }
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonStringInfo = JsonSerializer.Serialize(BackupsInfo, options);
            File.WriteAllText(MyIni.Read("SavePath"), jsonStringInfo);
            var jsonStringState = JsonSerializer.Serialize(BackupsState, options);
            File.WriteAllText(MyIni.Read("SaveState"), jsonStringState);
        }

        /// <summary>
        /// Overload of the DeleteBackup function, delete one or several backups chosen
        /// </summary>
        /// <param name="Backups">The list of SaveWork(s) chosen to delete</param>
        public static void DeleteBackup(List<SaveWork> Backups)
        {
            foreach (var backup in Backups)
            {
                DeleteBackup(backup.Name);
            }
        }

        /// <summary>
        /// Get all infos of all BackupWorks (Name,Source,Destination,Type)
        /// </summary>
        /// <returns>Infos of Backups</returns>
        public static List<SaveInfo> GetBackUpsInfo()
        {
            var MyIni = new IniFile();
            List<SaveInfo> BackupsInfo = new List<SaveInfo>();
            string path = MyIni.Read("SavePath");
            if (File.Exists(MyIni.Read("SavePath")))
            {
                try
                {
                    string jsonString = File.ReadAllText(path);
                    BackupsInfo = JsonSerializer.Deserialize<List<SaveInfo>>(jsonString);
                    return BackupsInfo;
                }
                catch (Exception)
                {
                    return BackupsInfo;
                }
            }
            else
            {
                string error = "SavePathNotExisting";
                return BackupsInfo;
            }
        }

        /// <summary>
        /// Get all State of all BackupWorks (Name,State,File,Filesize,FileTransferTime,TotalFileToCopy,TotalFileSize,NbFilesLeftToDo,Progression)
        /// </summary>
        /// <returns>States of Backups</returns>
        public static List<SaveState> GetBackUpsState()
        {
            var MyIni = new IniFile();
            List<SaveState> BackupsState = new List<SaveState>();

            string path = MyIni.Read("SaveState");
            if (File.Exists(MyIni.Read("SaveState")))
            {
                try
                {
                    string jsonString = File.ReadAllText(path);
                    BackupsState = JsonSerializer.Deserialize<List<SaveState>>(jsonString);
                    return BackupsState;
                }
                catch (Exception)
                {
                    return BackupsState;
                }
            }
            else
            {
                string error = "SaveStatePathNotExisting";
                return BackupsState;
            }

        }

        /// <summary>
        /// Get the backup chosen
        /// </summary>
        /// <param name="name">The backup chosen</param>
        /// <returns>The backup chosen</returns>
        public static SaveWork GetBackup(string name)
        {
            List<SaveWork> SaveList = GetAllBackups();
            foreach (var saveWork in SaveList)
            {
                if (saveWork.Name == name)
                {
                    return saveWork;
                }
            }
            return new SaveWork(name, "", "", true);

        }

        /// <summary>
        /// Get all backups
        /// </summary>
        /// <returns>Backups</returns>
        public static List<SaveWork> GetAllBackups()
        {
            List<SaveInfo> infos = GetBackUpsInfo();
            List<SaveState> states = GetBackUpsState();
            List<SaveWork> Backups = new List<SaveWork>();
            foreach (var info in infos)
            {
                foreach (var state in states)
                {
                    if (info.Name == state.Name)
                    {
                        SaveWork saveWork = new SaveWork(info.Name, info.FileSource, info.FileTarget, info.Full);
                        saveWork.Info = info;
                        saveWork.State = state;
                        Backups.Add(saveWork);
                    }
                }
            }
            return Backups;
        }

        /// <summary>
        /// Encrypt or decrypt a file
        /// </summary>
        /// <param name="sourceFile">The file to encrypt or decrypt</param>
        /// <param name="outputFile">The file encrypted or decrypted </param>
        private static long EncryptDecrypt(string sourceFile, string outputFile)
        {
            var MyIni = new IniFile();
            string key = MyIni.Read("EncryptionKey");
            var process = new ProcessStartInfo
            {
                FileName = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../netcoreapp3.1/CryptoSoft/netcoreapp3.1/CryptoSoft.exe")),
                Arguments = sourceFile + " " + outputFile + " " + key
            };

            using var proc = Process.Start(process);
            proc.WaitForExit();
            return proc.ExitCode;
        }

        /// <summary>
        /// Decrypt a Savework
        /// </summary>
        /// <param name="saveWork">The savework to decrypt</param>
        private static void DecryptBackup(SaveWork saveWork)
        {
            List<string> extensionCrypt = Commands.GetAllExtensionToCrypt();
            string[] files = Directory.GetFiles(saveWork.Info.FileTarget, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                foreach (var extension in extensionCrypt)
                {
                    if (extension == Path.GetExtension(Path.Combine(saveWork.Info.FileTarget, file)))
                    {
                        EncryptDecrypt(Path.Combine(saveWork.Info.FileTarget, file), Path.Combine(saveWork.Info.FileTarget, file));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Overload of DecryptBackup function, decrypt several saveworks
        /// </summary>
        /// <param name="Backups">Saveworks to decrypt</param>
        public static void DecryptBackup(List<SaveWork> Backups)
        {
            foreach (var backup in Backups)
            {
                DecryptThread(backup);
            }
        }

        /// <summary>
        /// Start a thread which decrypt a savework 
        /// </summary>
        /// <param name="saveWork">The savework to decrypt</param>
        private static void DecryptThread(SaveWork saveWork)
        {
            Thread tdecrypt = new Thread(new ThreadStart(
                () =>
                {
                    DecryptBackup(saveWork);
                }
                )
            );
            tdecrypt.Start();
        }
    }
}
