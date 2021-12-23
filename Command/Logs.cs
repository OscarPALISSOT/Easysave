using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using EasySave.Model;

namespace EasySave.Command
{
    public class Logs
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public int State { get; set; }
        public long FileSize { get; set; }
        public long FileTransferTime { get; set; }
        public long FileEncryptionTime { get; set; }
        public string Time { get; set; }
        public long NbFilesLeftToDo { get; set; }
        public double Progression { get; set; }

    }

    static class LogsCommands
    {
        
        private static object LockLog = new object();
        private static List<Logs> LogsList;

        /// <summary>
        /// Create the path of the LogFile
        /// </summary>
        /// <returns>Logfile's path</returns>
        private static string path()
        {
            var MyIni = new IniFile();
            DateTime localDate = DateTime.Today;
            string date = localDate.ToString("_dd_MM_yyyy");
            string name = string.Concat("EasySaveLogs", date, MyIni.Read("LogFormat"));
            string path = Path.Combine(MyIni.Read("LogsPath"), name);
            return path;
        }

        /// <summary>
        /// Create a LogFile if does not exist
        /// </summary>
        private static void LogFileExist()
        {
            if (!File.Exists(path()))
            {
                File.Create(path()).Close();
            }
        }
        /// <summary>
        /// Add log informations in the log file
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileSource"></param>
        /// <param name="fileTarget"></param>
        /// <param name="state"></param>
        /// <param name="fileSize"></param>
        /// <param name="fileTransferTime"></param>
        /// <param name="nbFilesLeftToDo"></param>
        public static void AddLogs(string name, string fileSource, string fileTarget, int state, long fileSize, long fileTransferTime, long nbFilesLeftToDo, long fileEncryptionTime, double progression)
        {
            lock (LockLog)
            {
                Logs log = new Logs();
                LogsList = GetLogs();
                log.Name = name;
                log.FileSource = fileSource;
                log.FileTarget = fileTarget;
                log.State = state;
                log.FileSize = fileSize;
                log.FileTransferTime = fileTransferTime;
                log.FileEncryptionTime = fileEncryptionTime;
                log.NbFilesLeftToDo = nbFilesLeftToDo;
                log.Progression = progression;
                DateTime time = DateTime.Now;
                log.Time = time.ToString("dd/MM/yyyy HH:mm:ss ");
                LogsList.Add(log);
                WriteLog(LogsList);
            }
            
        }

        /// <summary>
        /// Get all logs in the object list
        /// </summary>
        /// <returns>The list of logs</returns>
        private static List<Logs> GetLogs()
        {

            var MyIni = new IniFile();
            if (File.Exists(path()))
            {
                try
                {
                    if (MyIni.Read("LogFormat") == ".json")
                    {
                        string jsonString = File.ReadAllText(path());
                        LogsList = JsonSerializer.Deserialize<List<Logs>>(jsonString);
                        return LogsList;
                    }
                    else
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Logs>));

                        using (FileStream stream = File.OpenRead(path()))
                        {
                            LogsList = (List<Logs>)serializer.Deserialize(stream);
                        }
                        return LogsList;
                    }
                }
                catch (Exception e)
                {
                    String error = Convert.ToString(e);
                    LogsList = new List<Logs>();
                    return LogsList;
                }
            }
            else
            {
                string error = "LogFileNotExisting";
                LogsList = new List<Logs>();
                return LogsList;
            }
        }

        /// <summary>
        /// Write log(s) in daily log file
        /// </summary>
        /// <param name="LogsList">The list of log(s)</param>
        private static void WriteLog(List<Logs> LogsList)
        {
            LogFileExist();
            var MyIni = new IniFile();
            if (MyIni.Read("LogFormat") == ".xml")
            {
                LogFileExist();
                XmlSerializer serializer = new XmlSerializer(typeof(List<Logs>));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                TextWriter Filestream = new StreamWriter(path());

                serializer.Serialize(Filestream, LogsList, ns);

                Filestream.Close();
            }
            else
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var jsonStringState = JsonSerializer.Serialize(LogsList, options);
                File.WriteAllText(path(), jsonStringState);
            }
        }
    }
}
