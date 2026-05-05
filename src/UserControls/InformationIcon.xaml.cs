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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MarkIt.UserControls
{
    /// <summary>
    /// Interaktionslogik für InformationIcon.xaml
    /// </summary>
    public partial class InformationIcon : UserControl
    {
        private double position { get; set; } = 0;
        private bool hover = false;
        private bool up = true;
        public InformationIcon()
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
                EllispeI.Margin = new Thickness(0, position, 0, 0); // new thinkness von chatgpt
                if(up)
                {
                    position += 0.25;
                    if (position > 3.5)
                        up = false;
                }
                else
                {
                    position -= 0.25;
                    if (position < -3.5)
                        up = true;
                }
            }
            if((position <= -0.3 || position >= 0.3 )&& !hover)
            {
                EllispeI.Margin = new Thickness(0, position, 0, 0); // new thinkness von chatgpt
                if (up)
                {
                    position += 0.5;
                    if (position > 3.5)
                        up = false;
                }
                else
                {
                    position -= 0.5;
                    if (position < -3.5)
                        up = true;
                }
            }
        }

        private void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            hover = true;
            EllispeI.Fill = Brushes.White;
        }

        private void Rectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            hover = false;
            EllispeI.Fill = Brushes.Black;
        }
    }
}
