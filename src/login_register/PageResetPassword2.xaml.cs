using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Windows.Threading;

namespace MarkIt.login_register
{
    /// <summary>
    /// Interaction logic for PageRecetPassword2.xaml
    /// </summary>
    public partial class PageRecetPassword2 : Page
    {
        public PageRecetPassword2()
        {
            InitializeComponent();
        }

        // Claude Anfang
        // Prompt: wie kann ich von meiner password reset site an meine wpf app ein request senden also dass das password
        // veraendert wurde
        public static async void StartPasswordListener()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:9876/password-changed/");
            listener.Start();

            await Task.Run(async () =>
            {
                var ctx = await listener.GetContextAsync();

                // CORS Header damit Browser nicht blockiert
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Close();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Your password has been succesfully changed. Please login again!", "Password reset", MessageBoxButton.OK, MessageBoxImage.Information);
                    Logger.logger.Debug($"User changed password via Link: {MainWindow.currentUser.Email}");
                    WindowUserLogin.Navigate("PagePassword3", "PageLogin");
                    listener.Stop();
                    listener.Close();
                });
            });
        }
        // Claude Ende

        private async void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // ChatGPT anfang
                // Prompt: c# supabase checken ob ein code == code in verify email ist
                var result = await MainWindow.supabase.Auth.VerifyOTP(
                    email: MainWindow.currentUser.Email,
                    token: TextBoxCode.Text,
                    type: Supabase.Gotrue.Constants.EmailOtpType.Email);
                // ChatGPT ende
                if (result != null)
                {
                    WindowUserLogin.Navigate("PagePassword2", "PagePassword3");
                }
            }
            catch
            {
                TextBoxCode.BorderThickness = new Thickness(3);
                TextBoxCode.BorderBrush = Brushes.LightCoral;
                LabelFalse.Visibility = Visibility.Visible;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            WindowUserLogin.Navigate("PagePassword2", "PagePassword1");
        }

        private void TextBoxCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxCode.BorderThickness = new Thickness(1);
            TextBoxCode.BorderBrush = Brushes.Gray;
            LabelFalse.Visibility= Visibility.Hidden;
        }
    }
}
