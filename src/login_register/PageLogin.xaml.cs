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

namespace MarkIt.login_register
{
    /// <summary>
    /// Interaction logic for PageLogin.xaml
    /// </summary>
    public partial class PageLogin : Page
    {
        public static bool KeepMeLogedIn { get; private set; } = false;
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
            if (TextBoxEmail.Text == "" && PasswordBoxPassword.Password == "") return;
            var errortype =  await WindowUserLogin.UserManager.SignInAndHandleErrors(TextBoxEmail.Text, PasswordBoxPassword.Password.ToString(), LoadingScreen);
            if(errortype == UserManager.ErrorType.OK)
            {
                if(CheckBoxRemember.IsChecked == true)
                {
                    WindowUserLogin.UserManager.WriteToRememberedUsers(MainWindow.supabase.Auth.CurrentSession);
                }
                WindowUserLogin.Guest = true;
                WindowUserLogin.window.Close();
            }
            else if(errortype == UserManager.ErrorType.PasswordFalse)
            {
                LabelPasswordNotCorrect.Visibility = Visibility.Visible;
                PasswordBoxPassword.BorderThickness = new Thickness(3);
                PasswordBoxPassword.BorderBrush = Brushes.LightCoral;
                TextBoxEmail.BorderThickness = new Thickness(3);
                TextBoxEmail.BorderBrush = Brushes.LightCoral;
            }
        }
        private void ButtonGuest_Click(object sender, RoutedEventArgs e)
        {
            WindowUserLogin.Guest = true;
            WindowUserLogin.window.Close();
            MainWindow.currentUser = new ClassUser("guest", "guest");
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

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                ButtonLogin_Click(null, null);
            }
        }
    }
}
