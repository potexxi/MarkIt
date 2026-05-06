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

namespace MarkIt.UserControls
{
    /// <summary>
    /// Interaktionslogik für SwitchSlider.xaml
    /// </summary>
    public partial class SwitchSlider : UserControl
    {
        private double position = -45;
        private bool IsOn = false;
        public SwitchSlider()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromTicks(25000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            RectDisplay.Margin = new Thickness(position, 0, 0, 0);
            if (position < 35 && IsOn)
            {
                position += 5;
                RectDisplay.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            else if (position > -35 && !IsOn)
            {
                position -= 5;
                RectDisplay.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsOn) IsOn = false;
            else if (!IsOn) IsOn = true;
        }
    }
}
