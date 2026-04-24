using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Renci.SshNet;

namespace MarkIt.login_register
{
    /// <summary>
    /// Interaction logic for PageLogin.xaml
    /// </summary>
    public partial class PageLogin : Page
    {
        public static bool KeepMeLogedIn { get; private set; } = false;
        private bool _isloading = false;
        public PageLogin()
        {
            InitializeComponent();
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            label.Foreground = Brushes.Yellow;
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            label.Foreground = Brushes.White;
        }

        private void Label_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowUserLogin.Navigate("PageLogin", "PagePassword1");
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            WindowUserLogin.Navigate("PageLogin", "PageRegister");
        }

        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClassUser user = await CheckUserExists();
                if (user.Email != "error")
                {
                    if(CheckBoxRemember.IsChecked == true)
                    {
                        KeepMeLogedIn = true;
                    }
                    MainWindow.currentUser = user;
                    PageRecetPassword1.SendEmail(user.Email, "2fa");
                    WindowUserLogin.Navigate("PageLogin", "Page2FA");
                    Page2FA.Timer.Start();
                }
            }
            catch (Exception ex)
            {
                LabelPasswordNotCorrect.Visibility = Visibility.Visible;
                PasswordBoxPassword.BorderThickness = new Thickness(3);
                PasswordBoxPassword.BorderBrush = Brushes.LightCoral;
                TextBoxEmail.BorderThickness = new Thickness(3);
                TextBoxEmail.BorderBrush = Brushes.LightCoral;
            }
        }

        private async Task<ClassUser> CheckUserExists()
        {
            ClassUserList? userList;
            try
            {
                LoadingScreen.Visibility = Visibility.Visible;
                userList = await GetUsersFromServer();
                LoadingScreen.Visibility= Visibility.Hidden;
                foreach (ClassUser user in userList.Users)
                {
                    if (user.Email == TextBoxEmail.Text)
                    {
                        if (user.Password == PasswordBoxPassword.Password)
                        {
                            return user;
                        }
                        else
                        {
                            throw new Exception("password false");
                        }
                    }
                }
                throw new Exception("user does not exist");
            }
            catch(Exception ex) 
            {
                LoadingScreen.Visibility = Visibility.Hidden;
                if (ex.Message == "server")
                    MessageBox.Show("Currently our server is offline, please try again later or continue as guest.", "Server offline", MessageBoxButton.OK, MessageBoxImage.Question);
                else if (ex.Message == "users file")
                    MessageBox.Show("Our server caused a fatal error, please try again later.", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (ex.Message == "user does not exist")
                    throw new Exception(ex.Message);
                return new ClassUser(-1, "error", "error");
            }
        }

        private void ButtonGuest_Click(object sender, RoutedEventArgs e)
        {
            WindowUserLogin.Guest = true;
            WindowUserLogin.window.Close();
            MainWindow.currentUser = new ClassUser(-1, "guest", "guest");
        }

        public static Task<ClassUserList?> GetUsersFromServer()
        {
            // code inspired by StackOverflow/Autocompletion
            return Task.Run(() =>
            {
                ConnectionInfo connection;
                try
                {

                    PrivateKeyFile privateKey = new PrivateKeyFile(ServerSettings.KeyFilePath);

                    PrivateKeyAuthenticationMethod privateKeyAuth = new PrivateKeyAuthenticationMethod(ServerSettings.Username, privateKey);
                    connection = new ConnectionInfo(ServerSettings.PublicIp, ServerSettings.Port, ServerSettings.Username, privateKeyAuth);
                    Logger.logger.Debug("Successfully created connection with private key.");
                }
                catch
                {
                    Logger.logger.Error("Privat key authentication or server unreachable");
                    throw new Exception("server");
                }
                try
                {
                    using (SftpClient client = new SftpClient(connection))
                    {
                        try
                        {
                            client.Connect();
                        }
                        catch
                        {
                            Logger.logger.Error("Server unreachable.");
                            throw new Exception("server");
                        }

                        using (var stream = client.OpenRead("/files/users.json"))
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string content = reader.ReadToEnd();
                            ClassUserList? userList = JsonSerializer.Deserialize<ClassUserList>(content);
                            Logger.logger.Debug("Successfully got users from server.");
                            client.Disconnect();
                            return userList;
                        }
                    }
                }
                catch (Exception e)
                {
                    if (e.Message == "server")
                    {
                        throw new Exception("server");
                    }
                    Logger.logger.Fatal("No file \"users.json\".");
                    throw new Exception("users file");
                }
            });
        }

        public static void WriteUsersToServer(ClassUserList userList)
        {
            // code inspired by StackOverflow/Autocompletion
            ConnectionInfo connection;
            try
            {
                PrivateKeyFile privateKey = new PrivateKeyFile(ServerSettings.KeyFilePath);

                PrivateKeyAuthenticationMethod privateKeyAuth = new PrivateKeyAuthenticationMethod(ServerSettings.Username, privateKey);
                connection = new ConnectionInfo(ServerSettings.PublicIp, ServerSettings.Port, ServerSettings.Username, privateKeyAuth);
                Logger.logger.Debug("Successfully created connection with private key.");
            }
            catch
            {
                Logger.logger.Error("Privat key authentication or server unreachable.");
                throw new Exception("server");
            }
            try
            {
                using (SftpClient client = new SftpClient(connection))
                {
                    try
                    {
                        client.Connect();
                    }
                    catch
                    {
                        Logger.logger.Error("Server unreachable.");
                        throw new Exception("server");
                    }
                    JsonSerializerOptions jsonoptions = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };
                    using (var stream = client.OpenWrite("files/users.json"))
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        string json = JsonSerializer.Serialize(userList, options: jsonoptions);
                        writer.Write(json);
                        Logger.logger.Debug("Successfully wrote users to server.");

                    }
                    client.Disconnect();
                }
            }
            catch(Exception e)
            {
                if (e.Message == "server")
                {
                    throw new Exception("server");
                }
                Logger.logger.Error("No file \"users.json\".");
                throw new Exception("users file");
            }

        }

        public static ClassUserList? GetRemeberedUsers()
        {
            if (File.Exists("sources/remembered.json"))
            {
                try
                {
                    using (StreamReader reader = new StreamReader("sources/remembered.json"))
                    {
                        return JsonSerializer.Deserialize<ClassUserList>(reader.ReadToEnd());
                    }
                }
                catch
                {
                    Logger.logger.Warning("remembered.json incorrect format.");
                    return null;
                }
            }
            else
            {
                Logger.logger.Information("No remembered users, continue to login.");
                return null;
            }
        }

        public static void WriteToRememberedUsers(ClassUser user)
        {
            ClassUserList? userList = GetRemeberedUsers();
            if(userList == null)
            {
                userList = new ClassUserList();
            }
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            userList.Users.Add(user);
            using(StreamWriter writer = new StreamWriter("sources/remembered.json"))
            {
                writer.Write(JsonSerializer.Serialize(userList, options: options));
            }
        }

        private void TextBoxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxEmail.BorderThickness = new Thickness(1);
            TextBoxEmail.BorderBrush = Brushes.Gray;
            LabelPasswordNotCorrect.Visibility = Visibility.Hidden;
            PasswordBoxPassword.BorderThickness = new Thickness(1);
            PasswordBoxPassword.BorderBrush = Brushes.Gray;
        }

        private void PasswordBoxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            LabelPasswordNotCorrect.Visibility = Visibility.Hidden;
            PasswordBoxPassword.BorderThickness = new Thickness(1);
            PasswordBoxPassword.BorderBrush = Brushes.Gray;
            TextBoxEmail.BorderThickness = new Thickness(1);
            TextBoxEmail.BorderBrush = Brushes.Gray;
        }
    }
}
