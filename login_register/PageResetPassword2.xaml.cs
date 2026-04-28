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
    /// Interaction logic for PageRecetPassword2.xaml
    /// </summary>
    public partial class PageRecetPassword2 : Page
    {
        public static DispatcherTimer Timer { get; private set; }
        public static int timerCount = 90;
        public PageRecetPassword2()
        {
            InitializeComponent();
            timerCount = 90;
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
        }

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            timerCount -= 1;
            if (timerCount <= 0)
            {
                timerCount = 90;
                await WindowUserLogin.EmailManager.SendEmailAndHandleErrors(WindowUserLogin.EmailManager.Email, LoadingScreen);
            }
            LabelTimer.Content = $"Resend Code in: {timerCount}s";
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if ($"{WindowUserLogin.EmailManager.Code.ToString():D6}" == TextBoxCode.Text)
            {
                WindowUserLogin.Navigate("PagePassword2", "PagePassword3");
                timerCount = 90;
                Timer.Stop();
            }
            else
            {
                TextBoxCode.BorderThickness = new Thickness(3);
                TextBoxCode.BorderBrush = Brushes.LightCoral;
                LabelFalse.Visibility = Visibility.Visible;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            WindowUserLogin.Navigate("PagePassword2", "PagePassword1");
            timerCount = 90;
            LabelTimer.Content = $"Resend Code in: {timerCount}s";
            Timer.Stop();
        }

        private void TextBoxCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxCode.BorderThickness = new Thickness(1);
            TextBoxCode.BorderBrush = Brushes.Gray;
            LabelFalse.Visibility= Visibility.Hidden;
        }
    }
}
