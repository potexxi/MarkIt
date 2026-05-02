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
            //string email = TextBoxEmail.Text;
            //ClassUserList? userList = await WindowUserLogin.UserManager.GetUsersFromServerAndHandleErrors(LoadingScreen);
            //if(userList == null)
            //{
            //    return;
            //}
            //bool exists = false;
            //foreach(ClassUser user in userList.Users)
            //{
            //    if (user.Email == WindowUserLogin.EmailManager.Email) { exists = true; break; }
            //}
            //if (!exists)
            //{
            //    MessageBox.Show("This user does not exist. Enter an existing user or create an account!", "No user", MessageBoxButton.OK, MessageBoxImage.Question);
            //    return;
            //}
            //if (await WindowUserLogin.EmailManager.SendEmailAndHandleErrors(email, LoadingScreen))
            //{
            //    WindowUserLogin.Navigate("PagePassword1", "PagePassword2");
            //    PageRecetPassword2.Timer.Start();
            //}
            try
            {
                LoadingScreen.Visibility = Visibility.Visible;
                await MainWindow.supabase.Auth.ResetPasswordForEmail(TextBoxEmail.Text);
                LoadingScreen.Visibility = Visibility.Hidden;
                WindowUserLogin.Navigate("PagePassword1", "PagePassword2");
                PageRecetPassword2.TimerResend.Start();
            }
            catch (Supabase.Gotrue.Exceptions.GotrueException ex)
            {
                LoadingScreen.Visibility = Visibility.Hidden;
                if (ex.InnerException is HttpRequestException)
                {
                    MessageBox.Show("Currently our server is offline, please try again later or continue as guest.", "Server offline", MessageBoxButton.OK, MessageBoxImage.Question);
                    Logger.logger.Error($"Server unreachable. {ex.Message}");
                    return;
                }
                else if (ex.Reason is Supabase.Gotrue.Exceptions.FailureHint.Reason.UserBadEmailAddress)
                {
                    MessageBox.Show("Bad Email-Address! Check if your Email-Address is in valid format.", "Bad Email", MessageBoxButton.OK, MessageBoxImage.Information);
                    Logger.logger.Debug($"Bad Email: {TextBoxEmail.Text}");
                    return;
                }
                else if (ex.Reason is Supabase.Gotrue.Exceptions.FailureHint.Reason.UserTooManyRequests)
                {
                    Logger.logger.Debug($"Too many requests {TextBoxEmail.Text}");
                    MessageBox.Show("Too many requests! Please try again later.", "Too many requests.", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                LoadingScreen.Visibility = Visibility.Hidden;
                MessageBox.Show(ex.ToString());
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            WindowUserLogin.Navigate("PagePassword1", "PageLogin");
        }
    }
}
