using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MarkIt
{
    public class FileManager
    {
        private string userEmail;
        private string userPath;
        public List<string> FileHistory { get; private set; }
        public string? CurrentFilePath;
        public FileManager(string userEmail) 
        { 
            if(userEmail == "guest")
                this.userEmail = "guest@guest";
            else
                this.userEmail = userEmail;
            createUserFolder();
            setFileHistory();
            Logger.logger.Debug("Initialized FileManager");
        }

        private void createUserFolder()
        {
            string folderName = userEmail.Split("@")[0];
            userPath = "files/" + folderName;
            if (Directory.Exists(userPath))
            {
                return;
            }
            Directory.CreateDirectory(userPath);
            Logger.logger.Debug($"Created {userPath}");
        }

        private void setFileHistory()
        {
            if (File.Exists(userPath + "/file-history.json"))
            {
                string json = File.ReadAllText(userPath + "/file-history.json");
                if(json == "")
                {
                    FileHistory = new List<string>();
                    return;
                }
                FileHistory = JsonSerializer.Deserialize<List<string>>(json);
                return;
            }
            else
            {
                File.Create(userPath + "/file-history.json");
                FileHistory = new List<string>();
            }
        }

        public void SaveToFile(string filename, string content)
        {
            string folder = Path.GetDirectoryName(userPath + $"/{filename}");
            Directory.CreateDirectory(folder);
            using (StreamWriter rd = new StreamWriter(userPath + $"/{filename}"))
            {
                rd.Write(content);
            }
            CurrentFilePath = userPath + $"/{filename}";
            AddToHistory(userPath + $"/{filename}");
        }

        public string? LoadFromFile(string filename)
        {
            try
            {
                using(StreamReader sr = new StreamReader(userPath + $"/{filename}"))
                {
                    CurrentFilePath = userPath + $"/{filename}";
                    AddToHistory(userPath + $"/{filename}");
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                Logger.logger.Debug($"File not found: {userPath}/{filename}");
                return null;
            }
        }

        public void AddToHistory(string filepath)
        {
            if(filepath == "" || filepath == null)
            {
                Logger.logger.Debug("Wanted to save empty FilePath.");
                return;
            }
            if (FileHistory.Contains(filepath))
                FileHistory.Remove(filepath);
            FileHistory.Insert(0, filepath);
            if(FileHistory.Count > 5)
            {
                FileHistory.RemoveAt(5);
            }
            saveHistory();
        }

        private void saveHistory()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            using(StreamWriter sw = new StreamWriter(userPath + "/file-history.json"))
            {
                sw.Write(JsonSerializer.Serialize(FileHistory, options));
            }
        }
    }
}
