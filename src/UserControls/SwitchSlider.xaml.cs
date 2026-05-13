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
        private double position = 40;
        private DispatcherTimer timer = new DispatcherTimer();
        public bool IsOn { get; private set; } = false;
        private bool hover;
        public SwitchSlider()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(15);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            RectDisplay.Margin = new Thickness(position, 0, 0, 0);
            if (position < 110 && IsOn)
            {
                position += 6;
                RectDisplay.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            else if (position > 35 && !IsOn)
            {
                position -= 6;
                RectDisplay.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            else if(!hover)
            {
                timer.Tick -= Timer_Tick;
                timer.Stop();
            }
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsOn) { IsOn = false; LabelDisplay.Content = "OFF"; }
            else if (!IsOn) { IsOn = true; LabelDisplay.Content = "ON"; }
        }

        private void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            hover = true;
            RectBackground.Fill = Brushes.DarkGray;
            RectBorder.Stroke = Brushes.LightGray;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Rectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            hover = false;
            RectBackground.Fill = Brushes.LightGray;
            RectBorder.Stroke = Brushes.Black;
        }
    }
}
