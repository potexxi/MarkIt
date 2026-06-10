using Microsoft.Win32;
using Supabase;
using Supabase.Storage;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MarkIt
{
    public class FileManager
    {
        private string userEmail;
        public string userPath {  get; private set; }
        public string LastContent {  get; private set; }
        public List<FileHistoryItem> FileHistory { get; private set; }
        public string? CurrentFilePath;
        public FileType fileType { get; set; }
        public enum FileType
        {
            Root,
            Local,
            Cloud
        }

        public FileManager(string userEmail) 
        { 
            if(userEmail == "guest")
                this.userEmail = "guest@guest";
            else
                this.userEmail = userEmail;
            createUserFolder();
            InitFileHistory();
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

        public async void InitFileHistory()
        {
            try
            {
                if (!File.Exists(userPath + "/file-history.json"))
                {
                    using (File.Create(userPath + "/file-history.json")) { }
                    FileHistory = new List<FileHistoryItem>();
                }
                else
                {
                    string json = File.ReadAllText(userPath + "/file-history.json");
                    if (json == "")
                    {
                        FileHistory = new List<FileHistoryItem>();
                        return;
                    }
                    FileHistory = JsonSerializer.Deserialize<List<FileHistoryItem>>(json);
                    foreach (FileHistoryItem item in FileHistory)
                    {
                        if (!File.Exists(item.Path) && item.Type == FileType.Local)
                        {
                            FileHistory.Remove(item);
                        }
                        else if(item.Type == FileType.Cloud)
                        {
                            try
                            {
                                await MainWindow.supabase.Storage.From("MarkIt").Download(item.Path, (Supabase.Storage.TransformOptions?)null);
                            }
                            catch
                            {
                                FileHistory.Remove(item);
                            }
                        }
                    }
                    return;
                }
            }
            catch
            {
                File.Delete(userPath + "/file-history.json");
                using (File.Create(userPath + "/file-history.json")) { }
                FileHistory = new List<FileHistoryItem>();
            }
        }

        public void SaveToFile(string path, string content)
        {
            string folder = Path.GetDirectoryName(path);
            Directory.CreateDirectory(folder);
            using (StreamWriter rd = new StreamWriter(path))
            {
                rd.Write(content);
            }
            CurrentFilePath = path;
            AddToHistory(new FileHistoryItem(path, FileType.Local));
        }

        public string? LoadFromFile(string path)
        {
            try
            {
                using(StreamReader sr = new StreamReader(path))
                {
                    CurrentFilePath = path;
                    AddToHistory(new FileHistoryItem(path, FileType.Local));
                    string content = sr.ReadToEnd();
                    LastContent = content;
                    sr.Close();
                    fileType = FileType.Local;
                    return content;
                }
            }
            catch
            {
                Logger.logger.Debug($"File not found: {path}");
                return null;
            }
        }

        public string GetAbsolutPath(string path)
        {
            return Directory.GetParent(path).FullName;
        }

        public void AddToHistory(FileHistoryItem item)
        {
            if (item.Path == "" || item.Path == null)
            {
                Logger.logger.Debug("Tried to save empty FilePath.");
                return;
            }
            foreach(FileHistoryItem item2 in FileHistory)
            {
                if(item.Path == item2.Path)
                {
                    FileHistory.Remove(item2);
                    break;
                }
            }
            //if (FileHistory.Contains(item))
            //    FileHistory.Remove(item);
            FileHistory.Insert(0, item);
            if(FileHistory.Count > 5)
            {
                FileHistory.RemoveAt(5);
            }
            saveHistory();
        }

        public void RemoveFromHistory(FileHistoryItem item)
        {
            if (FileHistory.Contains(item))
                FileHistory.Remove(item);
        }

        private void saveHistory()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            File.WriteAllText(userPath + "/file-history.json", JsonSerializer.Serialize(FileHistory, options));
        }

        public async Task<bool> DeleteFromServer(string path, Grid loadingScreen)
        {
            try
            {
                // Von Supabase DOCS:  https://supabase.com/docs/reference/csharp/storage-from-download
                // Danach CHAtGPT
                // Prompt: await MainWindow.supabase.Storage.From("MarkIt").Remove(new List<string> { path });
                // wieso loescht das nicht die folder, weil die folder sind in supabase ja nur relative files
                // Anfang
                var files = await MainWindow.supabase.Storage
                    .From("MarkIt")
                    .List(path);

                var paths = files.Select(f => $"{path}/{f.Name}").ToList();
                if(paths.Count > 0)
                {
                    await MainWindow.supabase.Storage
                        .From("MarkIt")
                        .Remove(paths);
                }
                await MainWindow.supabase.Storage
                    .From("MarkIt")
                    .Remove(path);
                // Ende
                return true;
            }
            catch (Exception ex) 
            {
                Logger.logger.Error(ex.ToString());
                return false;
            }
        }

        public async Task<bool> Upload(string path, string content, Grid loadingscreen)
        {
            loadingscreen.Visibility = Visibility.Visible;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(content);
                await MainWindow.supabase.Storage.From("MarkIt").Upload(bytes, path);
                Supabase.Storage.FileOptions options = new Supabase.Storage.FileOptions { Upsert = true };
                await MainWindow.supabase.Storage.From("MarkIt").Upload(bytes, path, options);
                loadingscreen.Visibility = Visibility.Hidden;
                return true;
            }
            catch(Exception ex)
            {
                loadingscreen.Visibility = Visibility.Hidden;
                Logger.logger.Warning($"Couldn't upload file: {ex.Message}");
                WindowMessageBox box = new WindowMessageBox("Upload error", "File could not be uploaded. Plesae try again later.");
                box.ShowDialog();
                return false;
            }
        }

        public async Task<string>? Download(string path, Grid loadingscreen)
        {
            loadingscreen.Visibility = Visibility.Visible;
            try
            {
                byte[] content_byte = await MainWindow.supabase.Storage.From("MarkIt").Download(path, null);
                AddToHistory(new FileHistoryItem(path, FileType.Cloud));
                loadingscreen.Visibility = Visibility.Hidden;
                fileType = FileType.Cloud;
                return Encoding.UTF8.GetString(content_byte);
            }
            catch (Exception ex)
            {
                loadingscreen.Visibility = Visibility.Hidden;
                Logger.logger.Warning($"Download file: {ex.Message}");
                WindowMessageBox box = new WindowMessageBox("Download Error", "File could not be downloaded. Please try again later.");
                box.ShowDialog();
                return null;
            }
        }

        public bool CreateNewFile()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Create new File",
                FileName = "NewMarkItFile.md",
                // Filter von CHATGPT
                Filter = "Markdown (*.md)|*.md|Textdateien (*.txt)|*.txt|Alle Dateien (*.*)|*.*",
                InitialDirectory = GetAbsolutPath(userPath)
            };
            bool? result = sfd.ShowDialog();
            if (result == false || result == null)
            {
                return false;
            }
            CurrentFilePath = sfd.FileName;
            SaveToFile(sfd.FileName, "");
            MainWindow.CurrentWorkSheet.LoadFromString("");
            return true;
        }
    }
}
