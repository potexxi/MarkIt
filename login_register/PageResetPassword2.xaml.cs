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
        private Frame _frame;
        public static DispatcherTimer Timer { get; private set; }
        private int timerCount = 90;
        public PageRecetPassword2(Frame frame)
        {
            InitializeComponent();
            _frame = frame;
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            timerCount -= 1;
            if (timerCount <= 0)
            {
                timerCount = 90;
                PageRecetPassword1.SendEmail(PageRecetPassword1.email);
            }
            LabelTimer.Content = $"Resend Code in: {timerCount}s";
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if ($"{PageRecetPassword1.code.ToString():D6}" == TextBoxCode.Text)
            {
                _frame.Navigate(WindowUserLogin.pages["PagePassword3"]);
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
            _frame.Navigate(WindowUserLogin.pages["PagePassword1"]);
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
