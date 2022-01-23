using System;
using System.IO;
using System.Collections.Generic;
using EasySave.Model;
using System.Text.Json;
using System.Linq;
using System.Diagnostics;
using System.Windows;
using System.Security.Cryptography;
using EasySave.ViewModel.Settings;
using System.Threading;

namespace EasySave.Command
{
    static class Commands
    {
        
        /// <summary>
        /// Modify the location of the InfoFile
        /// </summary>
        /// <param name="path">New location of the InfoFile</param>
        public static void ModifySaveInfoPath(string path)
        {
            var MyIni = new IniFile();
            if (File.Exists(Path.Combine(path, "EasySaveBackup.json")))
            {
                //error if path is a file that already exists
                string error = "SaveFileAlreadyExists";
                return;
            }
            else
            {
                File.Create(Path.Combine(path, "EasySaveBackup.json")).Close();
                MyIni.Write("SavePath", Path.Combine(path, "EasySaveBackup.json"));
            }
        }

        /// <summary>
        ///  Modify the location of the LogFile
        /// </summary>
        /// <param name="path">New location of the LogFile</param>
        public static void ModifyLogsPath(string path)
        {
            var MyIni = new IniFile();
            if (Directory.Exists(Path.Combine(path, "EasySaveLogs")))
            {
                //error if path is a file that already exists
                string error = "LogDirectoryAlreadyExists";
                return;
            }
            else
            {
                Directory.CreateDirectory(Path.Combine(path, "EasySaveLogs"));
                MyIni.Write("LogsPath", Path.Combine(path, "EasySaveLogs"));
            }
        }

        /// <summary>
        ///  Modify the location of the StateFile
        /// </summary>
        /// <param name="path">New location of the StateFile</param>
        public static void ModifySaveStatePath(string path)
        {
            var MyIni = new IniFile();
            if (File.Exists(Path.Combine(path, "EasySaveSaveState.json")))
            {
                //error if path is a file that already exists
                string error = "SaveStateFileAlreadyExists";
                return;
            }
            else
            {
                File.Create(Path.Combine(path, "EasySaveSaveState.json")).Close();
                MyIni.Write("SaveState", Path.Combine(path, "EasySaveSaveState.json"));
            }
        }

        /// <summary>
        /// Get the size of the folder to save
        /// </summary>
        /// <param name="path">The folder to save</param>
        /// <returns>The size of the folder to save in bits</returns>
        public static long GetDirectoryTotalSize(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            long totalSize = info.EnumerateFiles("", SearchOption.AllDirectories).Sum(file => file.Length);
            return totalSize;
        }

        /// <summary>
        /// Get the number of files in the folder to save
        /// </summary>
        /// <param name="path">The folder to save</param>
        /// <returns>The nomber of files in the folder to save</returns>
        public static long GetDirectoryTotalNbFile(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            long nbfile = info.EnumerateFiles("", SearchOption.AllDirectories).Sum(file => 1);
            return nbfile;
        }

        /// <summary>
        /// Change the format of the log file 
        /// </summary>
        /// <param name="input">Format chosen</param>
        public static void ChangeLogFormat(int input)
        {
            var MyIni = new IniFile();
            if (input == 1)
            {
                MyIni.Write("LogFormat", ".json");
            }
            else if (input == 2)
            {
                MyIni.Write("LogFormat", ".xml");
            }
        }

        /// <summary>
        /// Shutdown application
        /// </summary>
        public static void ShutDown()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Add extension of files to crypt
        /// </summary>
        /// <param name="ext">The extension chosen</param>
        public static void AddExtensionToCrypt(string ext)
        {
            List<string> extensionList = new List<string>();

            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileExtensionToCrypt.json"));
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            extensionList = GetAllExtensionToCrypt();
            extensionList.Add(ext);
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(extensionList, options);
            File.WriteAllText(path, jsonString);
        }

        /// <summary>
        /// Get all extensions to crypt
        /// </summary>
        /// <returns>The list of extensions</returns>
        public static List<string> GetAllExtensionToCrypt()
        {
            List<string> extensionList = new List<string>();

            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileExtensionToCrypt.json"));
            if (File.Exists(path))
            {
                try
                {
                    string jsonString = File.ReadAllText(path);
                    extensionList = JsonSerializer.Deserialize<List<string>>(jsonString);
                    return extensionList;
                }
                catch (Exception e)
                {
                    string error = e.ToString();
                    return extensionList;
                }
            }
            else
            {
                string error = "FileExtensionToCryptNotExisting";
                return extensionList;
            }
        }

        /// <summary>
        /// Delete extension(s) of files to crypt
        /// </summary>
        /// <param name="ext">Extension(s) chosen</param>
        public static void DeleteExtensionToCrypt(string ext)
        {
            List<string> extensionList = new List<string>();

            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileExtensionToCrypt.json"));
            extensionList = GetAllExtensionToCrypt();
            extensionList.RemoveAll(e => e == ext);
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(extensionList, options);
            File.WriteAllText(path, jsonString);
        }

        /// <summary>
        /// Overload of DeleteExtensionToCrypt function, delete several extensions of files to crypt
        /// </summary>
        /// <param name="extentions">List of extensions to crypt to delete</param>
        public static void DeleteExtensionToCrypt(List<Extention> extentions)
        {
            foreach (var ext in extentions)
            {
                DeleteExtensionToCrypt(ext.Ext);
            }
            
        }

        /// <summary>
        /// Tell if a process is running or not
        /// </summary>
        /// <param name="processes">List of process</param>
        /// <returns></returns>
        public static bool IsProcessRunning(List<string> processes)
        {
            Process[] getProcess = Process.GetProcesses();
            foreach (var process in processes)
            {
                foreach (Process p in getProcess)
                {
                    if (process == p.ProcessName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Get the encryption key 
        /// </summary>
        /// <returns>The encryption key</returns>
        public static string GetEncryptionKey()
        {
            var MyIni = new IniFile();
            string key = MyIni.Read("EncryptionKey");
            return key;
        }

        /// <summary>
        /// Modify the encryption key
        /// </summary>
        /// <param name="key">The encryption key</param>
        public static void ModifyEncryptionKey(string key)
        {
            var MyIni = new IniFile();
            MyIni.Write("EncryptionKey", key);
        }

        /// <summary>
        /// Generate a 64bits encryption key
        /// </summary>
        public static void Generate64BitKey()
        {
            RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            string key = Convert.ToBase64String(randomBytes);
            ModifyEncryptionKey(key);
        }

        /// <summary>
        /// Add a business software which put process in pause
        /// </summary>
        /// <param name="soft">The business software</param>
        public static void AddBusinessSoftware(string soft)
        {
            List<string> softwareList = new List<string>();

            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BusinessSoftware.json"));
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            softwareList = GetAllBusinessSoftware();
            softwareList.Add(soft);
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(softwareList, options);
            File.WriteAllText(path, jsonString);
        }

        /// <summary>
        /// Get all business softwares
        /// </summary>
        /// <returns>The list of business softwares</returns>
        public static List<string> GetAllBusinessSoftware()
        {
            List<string> softwareList = new List<string>();

            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BusinessSoftware.json"));
            if (File.Exists(path))
            {
                try
                {
                    string jsonString = File.ReadAllText(path);
                    softwareList = JsonSerializer.Deserialize<List<string>>(jsonString);
                    return softwareList;
                }
                catch (Exception e)
                {
                    string error = e.ToString();
                    return softwareList;
                }
            }
            else
            {
                string error = "BusinessSoftwareFileNotExisting";
                return softwareList;
            }
        }

        /// <summary>
        /// Delete business software(s)
        /// </summary>
        /// <param name="soft">Business software to delete</param>
        public static void DeleteBusinessSoftware(string soft)
        {
            List<string> softwareList = new List<string>();

            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BusinessSoftware.json"));
            softwareList = GetAllBusinessSoftware();
            softwareList.RemoveAll(e => e == soft);
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(softwareList, options);
            File.WriteAllText(path, jsonString);
        }

        /// <summary>
        /// Overload of DeleteBusinessSoftware function, delete several Business softwares
        /// </summary>
        /// <param name="softwares">List of business softwares to delete</param>
        public static void DeleteBusinessSoftware(List<BusinessSoftware> softwares)
        {
            foreach (var soft in softwares)
            {
                DeleteBusinessSoftware(soft.Soft);
            }

        }

        /// <summary>
        /// Add priority file
        /// </summary>
        /// <param name="file">The priority file</param>
        public static void AddPriorityFile(string file)
        {
            List<string> fileList = new List<string>();

            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PriorityFile.json"));
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            fileList = GetAllPriorityFile();
            fileList.Add(file);
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(fileList, options);
            File.WriteAllText(path, jsonString);
        }

        /// <summary>
        /// Get all priority files
        /// </summary>
        /// <returns>Priority files list</returns>
        public static List<string> GetAllPriorityFile()
        {
            List<string> fileList = new List<string>();

            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PriorityFile.json"));
            if (File.Exists(path))
            {
                try
                {
                    string jsonString = File.ReadAllText(path);
                    fileList = JsonSerializer.Deserialize<List<string>>(jsonString);
                    return fileList;
                }
                catch (Exception e)
                {
                    string error = e.ToString();
                    return fileList;
                }
            }
            else
            {
                string error = "PriorityFileFileNotExisting";
                return fileList;
            }
        }

        /// <summary>
        /// Delete a priority file
        /// </summary>
        /// <param name="file">The priority file to delete</param>
        public static void DeletePriorityFile(string file)
        {
            List<string> fileList = new List<string>();

            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PriorityFile.json"));
            fileList = GetAllPriorityFile();
            fileList.RemoveAll(e => e == file);
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(fileList, options);
            File.WriteAllText(path, jsonString);
        }

        /// <summary>
        /// Overload of the DeletePriorityFile function, delete several priority files 
        /// </summary>
        /// <param name="files">The list of priority files to delete</param>
        public static void DeletePriorityFile(List<PriorityFile> files)
        {
            foreach (var file in files)
            {
                DeletePriorityFile(file.File);
            }

        }
    }
}
