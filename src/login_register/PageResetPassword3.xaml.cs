using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace MarkIt.login_register
{
    /// <summary>
    /// Interaction logic for PageResetPassword3.xaml
    /// </summary>
    public partial class PageResetPassword3 : Page
    {
        public PageResetPassword3()
        {
            InitializeComponent();
        }

        private async void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            if (Password1.Password == "" && Password2.Password == "") return;
            WindowMessageBox box;
            if (Password1.Password != Password2.Password)
            {
                Password1.BorderThickness = new Thickness(3);
                Password1.BorderBrush = Brushes.LightCoral;
                LabelPasswordFalse1.Visibility = Visibility.Visible;
                Password2.BorderThickness = new Thickness(3);
                Password2.BorderBrush = Brushes.LightCoral;
                LabelPasswordFalse2.Visibility = Visibility.Visible;
            }
            else
            {
                LoadingScreen.Visibility = Visibility.Visible;
                try
                {
                    await MainWindow.supabase.Auth.Update(new Supabase.Gotrue.UserAttributes { Password = Password2.Password });
                }
                catch(Supabase.Gotrue.Exceptions.GotrueException ex)
                {
                    if (ex.Reason is Supabase.Gotrue.Exceptions.FailureHint.Reason.UserBadPassword)
                    {
                        box = new WindowMessageBox("Passwords", "Password should be different!");
                        box.ShowDialog();
                        LoadingScreen.Visibility = Visibility.Hidden;
                        return;
                    }
                }
                ClassUser newUser = new ClassUser(MainWindow.currentUser.Email, Password2.Password);
                MainWindow.currentUser = newUser;
                LoadingScreen.Visibility = Visibility.Hidden;
                box = new WindowMessageBox("Password reset", "Your password has been succesfully changed. Please login again!");
                box.ShowDialog();
                Logger.logger.Debug($"User changed password: {MainWindow.currentUser.Email}");
                if (WindowUserLogin.Login)
                    WindowUserLogin.Navigate("PagePassword3", "PageLogin");
                else
                    WindowUserLogin.window.Close();
                return;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (WindowUserLogin.Login)
                WindowUserLogin.Navigate("PagePassword3", "PageLogin");
            else
                WindowUserLogin.window.Close();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password1.BorderBrush = Brushes.Gray;
            Password1.BorderThickness = new Thickness(1);
            LabelPasswordFalse1.Visibility = Visibility.Hidden;
            Password2.BorderBrush = Brushes.Gray;
            Password2.BorderThickness = new Thickness(1);
            LabelPasswordFalse2.Visibility = Visibility.Hidden;
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                ButtonReset_Click(sender, e);
            }
        }
    }
}
