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
        private static HttpListener _listener;

        public static async void StartPasswordListener()
        {
            // alten Listener stoppen
            if (_listener != null)
            {
                try
                {
                    _listener.Stop();
                    _listener.Close();
                }
                catch { }
                _listener = null;
            }

            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:9876/password-changed/");
            _listener.Start();

            await Task.Run(async () =>
            {
                var ctx = await _listener.GetContextAsync();

                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Close();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    new WindowMessageBox(
                        "Password reset",
                        "Your password has been successfully changed. Please login again!"
                    ).ShowDialog();

                    Logger.logger.Debug($"User changed password via Link: {MainWindow.currentUser.Email}");

                    _listener.Stop();
                    _listener.Close();
                    _listener = null;

                    if (WindowUserLogin.Login)
                        WindowUserLogin.Navigate("PagePassword3", "PageLogin");
                    else
                        WindowUserLogin.window.Close();
                });
            });
        }
        // Claude Ende

        private async void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxCode.Text == "") return;
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
            if (WindowUserLogin.Login)
                WindowUserLogin.Navigate("PagePassword2", "PagePassword1");
            else
                WindowUserLogin.window.Close();
        }

        private void TextBoxCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxCode.BorderThickness = new Thickness(1);
            TextBoxCode.BorderBrush = Brushes.Gray;
            LabelFalse.Visibility= Visibility.Hidden;
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                ButtonOK_Click(sender, e);
            }
        }
    }
}
