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
        public static DispatcherTimer Timer { get; private set; }
        private int timerCount = 90;

        public Page2FA()
        {
            InitializeComponent();
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            timerCount = 90;
            LabelTimer.Content = $"Resend Code in: {timerCount}s";
            Timer.Stop();
            WindowUserLogin.Navigate("Page2FA", "PageLogin");
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if($"{PageRecetPassword1.code:D6}" == TextBoxCode.Text)
            {
                WindowUserLogin.Guest = true;
                if(PageLogin.KeepMeLogedIn == true)
                {
                    UserManager.WriteToRememberedUsers(MainWindow.currentUser);
                }
                Timer.Stop();
                LabelTimer.Content = $"Resend Code in: {timerCount}s";
                timerCount = 90;
                WindowUserLogin.window.Close();
            }
            else
            {
                TextBoxCode.BorderThickness = new Thickness(3);
                TextBoxCode.BorderBrush = Brushes.LightCoral;
                LabelFalse.Visibility = Visibility.Visible;
            }
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            timerCount -= 1;
            if (timerCount <= 0)
            {
                timerCount = 90;
                PageRecetPassword1.SendEmail(MainWindow.currentUser.Email, "2fa");
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
