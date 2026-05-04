using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
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
using static MarkIt.UserManager;

namespace MarkIt.login_register
{
    /// <summary>
    /// Interaction logic for PageRecetPassword1.xaml
    /// </summary>
    public partial class PageRecetPassword1 : Page
    {

        public PageRecetPassword1()
        {
            InitializeComponent();
        }

        private async void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxEmail.Text == "") return;
            try
            {
                LoadingScreen.Visibility = Visibility.Visible;
                await MainWindow.supabase.Auth.ResetPasswordForEmail(TextBoxEmail.Text);
                MainWindow.currentUser = new ClassUser(TextBoxEmail.Text, "reset");
                LoadingScreen.Visibility = Visibility.Hidden;
                PageRecetPassword2.StartPasswordListener();
                WindowUserLogin.Navigate("PagePassword1", "PagePassword2");
            }
            catch (Supabase.Gotrue.Exceptions.GotrueException ex)
            {
                LoadingScreen.Visibility = Visibility.Hidden;
                WindowMessageBox box;
                if (ex.InnerException is HttpRequestException)
                {
                    box = new WindowMessageBox("Server offline", "Currently our server is offline, please try again later or continue as guest.");
                    box.ShowDialog();
                    Logger.logger.Error($"Server unreachable. {ex.Message}");
                    return;
                }
                else if (ex.Reason is Supabase.Gotrue.Exceptions.FailureHint.Reason.UserBadEmailAddress)
                {
                    box = new WindowMessageBox("Bad Email", "Bad Email-Address! Check if your Email-Address is in valid format.");
                    box.ShowDialog();
                    Logger.logger.Debug($"Bad Email: {TextBoxEmail.Text}");
                    return;
                }
                else if (ex.Reason is Supabase.Gotrue.Exceptions.FailureHint.Reason.UserTooManyRequests)
                {
                    Logger.logger.Debug($"Too many requests {TextBoxEmail.Text}");
                    box = new WindowMessageBox("Too many requests.", "Too many requests! Please try again later.");
                    box.ShowDialog();
                    return;
                }
                LoadingScreen.Visibility = Visibility.Hidden;
                box = new WindowMessageBox("Unknown", ex.ToString());
                box.ShowDialog();
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            WindowUserLogin.Navigate("PagePassword1", "PageLogin");
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                ButtonSend_Click(sender, e);
            }
        }
    }
}
