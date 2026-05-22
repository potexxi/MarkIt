using MarkIt.settings;
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
        private DispatcherTimer timer = new DispatcherTimer();
        private bool _animation = false;
        static public Brush hovercolor = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground); // chatgpt this specific line might be used very often
        static public Brush defaultcolor = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
        static public Brush Backgroundcolor = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
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
                    timer.Interval = TimeSpan.FromMilliseconds(Convert.ToInt32(MainWindow.GeneralSettings.animationFPS));
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
        public AccoundIcon()
        {
            InitializeComponent();
            updateSettings();
        }

        public void updateSettings()
        {
            hovercolor = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
            defaultcolor = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
            Backgroundcolor = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
            animation = MainWindow.GeneralSettings.iconAnimations;
            timer.Interval = TimeSpan.FromMilliseconds(Convert.ToInt32(MainWindow.GeneralSettings.animationFPS));
            ElliepseBody.Fill = defaultcolor;
            ElliepseHead.Fill = defaultcolor;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (hover)
            {
                ElliepseHead.Margin = new Thickness(0, position, 0, 0); // new thinkness von chatgpt
                ElliepseBody.Height = 25 + position * 2;
                if (position > -3)
                    position -= 0.15;
            }
            else if (position < 0)
            {
                position += 0.15;
                ElliepseHead.Margin = new Thickness(0, position, 0, 0); // new thinkness von chatgpt
                ElliepseBody.Height = 25 + position * 2;
            }
        }

        private void RectHover_MouseEnter(object sender, MouseEventArgs e)
        {
            if (animation)
            {
                ElliepseBody.Fill = hovercolor;
                ElliepseHead.Fill = hovercolor;
                hover = true;
            }
        }

        private void RectHover_MouseLeave(object sender, MouseEventArgs e)
        {
            if (animation)
            {
                ElliepseBody.Fill = defaultcolor;
                ElliepseHead.Fill = defaultcolor;
                hover = false;

            }
        }
    }
}
