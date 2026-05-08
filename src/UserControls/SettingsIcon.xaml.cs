using MarkIt.windows;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MarkIt.UserControls
{
    /// <summary>
    /// Interaktionslogik für SettingsIcon.xaml
    /// </summary>
    public partial class SettingsIcon : UserControl
    {
        private double rotation { get; set; } = 0;
        private DispatcherTimer timer = new DispatcherTimer();
        private bool _animation = false;
        //chatgpt
        private RotateTransform rotation1;
        //chatgpt ende
        static public SolidColorBrush hovercolor = Brushes.LightGray; // autocomplition
        static public SolidColorBrush defaultcolor = Brushes.Black;
        static public SolidColorBrush Backgroundcolor = Brushes.Gray;
        public SettingsIcon()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(15);
            timer.Tick -= Timer_Tick;
            timer.Tick += Timer_Tick;

            HoleGrid.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5); // chatgpt weil ursprung von grid nichtmehr passt
            //chatgpt to improve performance
            rotation1 = new RotateTransform(0);
            HoleGrid.RenderTransform = rotation1;
            //chatgpt end
            EllipseBack.Fill = Backgroundcolor; // might be changed later on
            _animation = true;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            rotation = (rotation + 3) % 360; // the % 360 is also from chatgpt
            rotation1.Angle = rotation;//chatgpt um performence zu verbessern & max
        }

        private void RectSettingsIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_animation)
            {
                EllipseCenter.Fill = hovercolor;
                Rect1.Fill = hovercolor;
                Rect2.Fill = hovercolor;
                Rect3.Fill = hovercolor;
                Rect4.Fill = hovercolor;
                timer.Start();
            }
        }

        private void RectSettingsIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_animation)
            {
                EllipseCenter.Fill = defaultcolor;
                Rect1.Fill = defaultcolor;
                Rect2.Fill = defaultcolor;
                Rect3.Fill = defaultcolor;
                Rect4.Fill = defaultcolor;
                timer.Stop();
            }
        }

        private void RectSettingsIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowSettings ws = new WindowSettings();
            ws.ShowDialog();
        }
    }
}
