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
using System.Windows.Threading;

namespace MarkIt.login_register
{
    /// <summary>
    /// Interaction logic for Page2FA.xaml
    /// </summary>
    public partial class Page2FA : Page
    {
        public static DispatcherTimer TimerResend { get; private set; }
        public static DispatcherTimer TimerCheckVerified;
        private int timerCount = 90;

        public Page2FA()
        {
            InitializeComponent();
            TimerResend = new DispatcherTimer();
            TimerResend.Interval = TimeSpan.FromSeconds(1);
            TimerResend.Tick += Timer_Tick;
            TimerCheckVerified = new DispatcherTimer();
            TimerCheckVerified.Interval = TimeSpan.FromSeconds(1);
            TimerCheckVerified.Tick += TimerCheckVerified_Tick;
        }

        private async void TimerCheckVerified_Tick(object? sender, EventArgs e)
        {
            try
            {
                var signin = await MainWindow.supabase.Auth.SignIn(MainWindow.currentUser.Email, MainWindow.currentUser.Password);
                WindowUserLogin.Guest = true;
                TimerResend.Stop();
                TimerCheckVerified.Stop();
                LabelTimer.Content = $"Resend Code in: {timerCount}s";
                timerCount = 90;
                WindowUserLogin.window.Close();
            }
            catch(Supabase.Gotrue.Exceptions.GotrueException ex)
            {
                if(ex.Reason is Supabase.Gotrue.Exceptions.FailureHint.Reason.UserEmailNotConfirmed)
                {
                    Logger.logger.Debug("User not confirmed.");
                }
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            timerCount = 90;
            LabelTimer.Content = $"Resend Code in: {timerCount}s";
            TimerResend.Stop();
            TimerCheckVerified.Stop();
            WindowUserLogin.Navigate("Page2FA", "PageLogin");
        }

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
                    TimerResend.Stop();
                    TimerCheckVerified.Stop();
                    LabelTimer.Content = $"Resend Code in: {timerCount}s";
                    timerCount = 90;
                    WindowUserLogin.Navigate("Page2FA", "PageLogin");
                }
            }
            catch
            {
                TextBoxCode.BorderThickness = new Thickness(3);
                TextBoxCode.BorderBrush = Brushes.LightCoral;
                LabelFalse.Visibility = Visibility.Visible;
            }
        }
        private async void Timer_Tick(object? sender, EventArgs e)
        {
            timerCount -= 1;
            if (timerCount <= 0)
            {
                timerCount = 90;
                var options = new Supabase.Gotrue.SignInWithPasswordlessEmailOptions(email: MainWindow.currentUser.Email);
                await MainWindow.supabase.Auth.SignInWithOtp(options);
            }
            LabelTimer.Content = $"Resend Code in: {timerCount}s";
        }

        private void TextBoxCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxCode.BorderThickness = new Thickness(1);
            TextBoxCode.BorderBrush = Brushes.Gray;
            LabelFalse.Visibility = Visibility.Hidden;
        }
    }
}
