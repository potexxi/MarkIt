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
            ClassUser? user = await CheckUserExists();
            if(user == null)
            {
                return;
            }
            else
            {
                if (CheckBoxRemember.IsChecked == true)
                {
                    KeepMeLogedIn = true;
                }
                MainWindow.currentUser = user;
                PageRecetPassword1.SendEmail(user.Email, "2fa");
                WindowUserLogin.Navigate("PageLogin", "Page2FA");
                Page2FA.Timer.Start();
            }
        }

        private async Task<ClassUser?> CheckUserExists()
        {
            LoadingScreen.Visibility = Visibility.Visible;
            var (userList, errortype) = await UserManager.GetUsersFromServer();
            LoadingScreen.Visibility= Visibility.Hidden;
            if(userList == null)
            {
                if(errortype == UserManager.ErrorType.ServerUnreachable || errortype == UserManager.ErrorType.PrivatKeyAuth)
                {
                    MessageBox.Show("Currently our server is offline, please try again later or continue as guest.", "Server offline", MessageBoxButton.OK, MessageBoxImage.Question);
                    return null;
                }
                else if(errortype == UserManager.ErrorType.UsersFile)
                {
                    MessageBox.Show("Our server caused a fatal error, please try again later.", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
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
                        break;
                    }
                }
            }
            LabelPasswordNotCorrect.Visibility = Visibility.Visible;
            PasswordBoxPassword.BorderThickness = new Thickness(3);
            PasswordBoxPassword.BorderBrush = Brushes.LightCoral;
            TextBoxEmail.BorderThickness = new Thickness(3);
            TextBoxEmail.BorderBrush = Brushes.LightCoral;
            return null;
        }

        private void ButtonGuest_Click(object sender, RoutedEventArgs e)
        {
            WindowUserLogin.Guest = true;
            WindowUserLogin.window.Close();
            MainWindow.currentUser = new ClassUser(-1, "guest", "guest");
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
