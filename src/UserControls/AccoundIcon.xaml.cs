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
    /// Interaktionslogik für AccoundIcon.xaml
    /// </summary>
    public partial class AccoundIcon : UserControl
    {
        private bool hover = false;
        public double position { get; set; } = 0;
        public AccoundIcon()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromTicks(50000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (hover)
            {
                ElliepseHead.Margin = new Thickness(0, position, 0, 0); // new thinkness von chatgpt
                ElliepseBody.Height = 25 + position*2;
                if(position > -3)
                    position -= 0.15;
            }
            else if (position < 0)
            {
                position += 0.15;
                ElliepseHead.Margin = new Thickness(0, position, 0, 0); // new thinkness von chatgpt

                ElliepseBody.Height = 25 + position*2;
            }
        }

        private void RectHover_MouseEnter(object sender, MouseEventArgs e)
        {
            ElliepseBody.Fill = Brushes.LightGray;
            ElliepseHead.Fill = Brushes.LightGray;
            hover = true;
        }

        private void RectHover_MouseLeave(object sender, MouseEventArgs e)
        {
            ElliepseBody.Fill = Brushes.Black;
            ElliepseHead.Fill = Brushes.Black;
            hover = false;
        }
    }
}
