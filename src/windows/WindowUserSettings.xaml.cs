using MarkIt.login_register;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace MarkIt.windows
{
    /// <summary>
    /// Interaktionslogik für WindowUserSettings.xaml
    /// </summary>
    public partial class WindowUserSettings : Window
    {
        public WindowUserSettings()
        {
            InitializeComponent();
            if(MainWindow.currentUser.Email != "guest")
            {
                LabelEmail.Content = MainWindow.currentUser.Email;
            }
            else
            {
                GuestAsUser();
            }
        }

        private void GuestAsUser()
        {
            LabelEmail.Content = "Guest";
            LabelPassword.Opacity = 0.3;
            LabelAccount.Opacity = 0.3;
            Label label = new Label();
            label.Content = "You need to be logged in, to use these features";
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.FontSize = 18;
            StackPanelGuest.Children.Add(label);
            ButtonLogout.IsEnabled = false;
            ButtonPassword.IsEnabled = false;
            ButtonLogout.Opacity = 0.3;
            ButtonPassword.Opacity = 0.3;
        }

        private async void ButtonPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowUserLogin window = new WindowUserLogin();
            PageRecetPassword2.ChangeEmail = false;
            LoadingScreen.Visibility = Visibility.Visible;
            try
            {
                await MainWindow.supabase.Auth.ResetPasswordForEmail(MainWindow.currentUser.Email);
            }
            catch
            {
                LoadingScreen.Visibility = Visibility.Hidden;
                WindowMessageBox box = new WindowMessageBox("Take a break.", "Take a short break before you continue.");
                box.ShowDialog();
                return;
            }
            WindowUserLogin.Navigate("", "PagePassword2");
            WindowUserLogin.Login = false;
            PageRecetPassword2.StartPasswordListener();
            window.ShowDialog();
            LoadingScreen.Visibility = Visibility.Hidden;
            WindowUserLogin.Login = true;
        }

        private void ButtonLogout_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // ChatGPT-Anfang
            // prompt: wie kann ich eine application neustarten in c# code
            Process.Start(Environment.ProcessPath!);
            Application.Current.Shutdown();
            // ChatGPT-Ende
        }

        private void ButtonBack_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
