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
        private bool hover = false;
        private DispatcherTimer timer = new DispatcherTimer();
        private bool _animation = false;
        public bool animation
        {
            get
            {
                return _animation;
            }
            set
            {
                if (value)
                {
                    _animation = true;
                    timer.Interval = TimeSpan.FromTicks(50000);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                }
                else
                {
                    _animation = false;
                    if (timer.IsEnabled)
                        timer.Stop();
                }
            }
        }
        static public SolidColorBrush hovercolor = Brushes.LightGray; // autocomplition
        static public SolidColorBrush defaultcolor = Brushes.Black;
        static public SolidColorBrush Backgroundcolor = Brushes.Gray;
        public SettingsIcon()
        {
            InitializeComponent();
            EllipseBack.Fill = Backgroundcolor; // might be changed later on
            animation = true;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (hover)
            {
                rotation += 1;
                Rect3.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5); // chatgpt weil ursprung von rect 3 und rect 4 nicht passen
                Rect4.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5); // chat gpt ende
                Rect1.RenderTransform = new RotateTransform(rotation);// chatgpt this line promt: how do I make a rotation in wpf backend
                Rect2.RenderTransform = new RotateTransform(rotation + 90);
                Rect4.RenderTransform = new RotateTransform(rotation + 45);
                Rect3.RenderTransform = new RotateTransform(rotation + 45);
            }
        }

        private void RectSettingsIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            if (animation)
            {
                EllipseCenter.Fill = hovercolor;
                Rect1.Fill = hovercolor;
                Rect2.Fill = hovercolor;
                Rect3.Fill = hovercolor;
                Rect4.Fill = hovercolor;

                hover = true;
            }
        }

        private void RectSettingsIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            if (animation)
            {
                EllipseCenter.Fill = defaultcolor;
                Rect1.Fill = defaultcolor;
                Rect2.Fill = defaultcolor;
                Rect3.Fill = defaultcolor;
                Rect4.Fill = defaultcolor;

                hover = false;
            }
        }
    }
}
