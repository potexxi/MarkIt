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
                ElliepseBody.Margin = new Thickness(0, 0, 0, 0);
            }
        }

        private void RectHover_MouseEnter(object sender, MouseEventArgs e)
        {
            hover = true;
        }

        private void RectHover_MouseLeave(object sender, MouseEventArgs e)
        {
            hover = false;
        }
    }
}
