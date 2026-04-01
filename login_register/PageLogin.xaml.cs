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
using Renci.SshNet;

namespace MarkIt.login_register
{
    /// <summary>
    /// Interaction logic for PageLogin.xaml
    /// </summary>
    public partial class PageLogin : Page
    {
        private Frame _frame;
        private WindowUserLogin _userLogin;
        
        public PageLogin(Frame frame, WindowUserLogin window)
        {
            InitializeComponent();
            _frame = frame;
            _userLogin = window;
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
            _frame.Navigate(WindowUserLogin.pages["PagePassword1"]);
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            _frame.Navigate(WindowUserLogin.pages["PageRegister"]);
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClassUser user = CheckUserExists();
                if (user.Email != "error")
                {
                    MainWindow.currentUser = user;
                    PageRecetPassword1.SendEmail(user.Email);
                    Page2FA.Timer.Start();
                    _frame.Navigate(WindowUserLogin.pages["Page2FA"]);
                }
            }
            catch (Exception ex)
            {
                if(ex.Message == "password false")
                {
                    LabelPasswordNotCorrect.Visibility = Visibility.Visible;
                    PasswordBoxPassword.BorderThickness = new Thickness(3);
                    PasswordBoxPassword.BorderBrush = Brushes.LightCoral;
                }
                else if(ex.Message == "user does not exist")
                {
                    LabelEmailNotFound.Visibility = Visibility.Visible;
                    TextBoxEmail.BorderThickness = new Thickness(3);
                    TextBoxEmail.BorderBrush = Brushes.LightCoral;
                }
            }
        }

        private ClassUser CheckUserExists()
        {
            ClassUserList userList;
            try
            {
                userList = GetUsersFromServer(10220, "potexxi.duckdns.org", "markit", "sources/markitkey");
            }
            catch(Exception ex) 
            {
                if (ex.Message == "server")
                    MessageBox.Show("Currently our server are offline, please try again later or continue as guest.", "Server offline", MessageBoxButton.OK, MessageBoxImage.Question);
                else if (ex.Message == "users file")
                    MessageBox.Show("File on the server not found, please try again.", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                return new ClassUser(-1, "error", "error");
            }
            foreach(ClassUser user in userList.Users)
            {
                if (user.Email == TextBoxEmail.Text)
                {
                    if(user.Password == PasswordBoxPassword.Password)
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

        private void ButtonGuest_Click(object sender, RoutedEventArgs e)
        {
            _userLogin.Close();
            MainWindow.currentUser = new ClassUser(-1, "guest", "guest");
        }

        public ClassUserList GetUsersFromServer(int port, string publicIP, string username, string privateKeyFilePath)
        {
            // code inspired by StackOverflow/Autocompletion
            ConnectionInfo connection;
            try
            {

                PrivateKeyFile privateKey = new PrivateKeyFile(privateKeyFilePath);
                
                PrivateKeyAuthenticationMethod privateKeyAuth = new PrivateKeyAuthenticationMethod(username, privateKey);
                connection = new ConnectionInfo(publicIP, port, username, privateKeyAuth);
                MainWindow.logger.Debug("Successfully created connection with private key.");
            }
            catch
            {
                MainWindow.logger.Error("Privat key authentication or server unreachable");
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
                        MainWindow.logger.Error("Server unreachable.");
                        throw new Exception("server");
                    }

                    using (var stream = client.OpenRead("/files/users.json"))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string content = reader.ReadToEnd();
                        ClassUserList classUserList = JsonSerializer.Deserialize<ClassUserList>(content);
                        MainWindow.logger.Debug("Successfully got users from server.");
                        client.Disconnect();
                        return classUserList;
                    }
                }
            }
            catch(Exception e) 
            {
                if (e.Message == "server")
                {
                    throw new Exception("server");
                }
                MainWindow.logger.Error("No file \"users.json\".");
                throw new Exception("users file");
            }
            
        }

        public void WriteUsersToServer(int port, string publicIP, string username, string privateKeyFilePath, ClassUserList userList)
        {
            // code inspired by StackOverflow/Autocompletion
            ConnectionInfo connection;
            try
            {
                PrivateKeyFile privateKey = new PrivateKeyFile(privateKeyFilePath);

                PrivateKeyAuthenticationMethod privateKeyAuth = new PrivateKeyAuthenticationMethod(username, privateKey);
                connection = new ConnectionInfo(publicIP, port, username, privateKeyAuth);
                MainWindow.logger.Debug("Successfully created connection with private key.");
            }
            catch
            {
                MainWindow.logger.Error("Privat key authentication or server unreachable.");
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
                        MainWindow.logger.Error("Server unreachable.");
                        throw new Exception("server");
                    }

                    using (var stream = client.OpenWrite("files/users.json"))
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        string json = JsonSerializer.Serialize(userList);
                        writer.Write(json);
                        MainWindow.logger.Debug("Successfully wrote users to server.");
                        
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
                MainWindow.logger.Error("No file \"users.json\".");
                throw new Exception("users file");
            }

        }

        private void TextBoxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            LabelEmailNotFound.Visibility = Visibility.Hidden;
            TextBoxEmail.BorderThickness = new Thickness(1);
            TextBoxEmail.BorderBrush = Brushes.Gray;
        }

        private void PasswordBoxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            LabelPasswordNotCorrect.Visibility = Visibility.Hidden;
            PasswordBoxPassword.BorderThickness = new Thickness(1);
            PasswordBoxPassword.BorderBrush = Brushes.Gray;
        }
    }
}
