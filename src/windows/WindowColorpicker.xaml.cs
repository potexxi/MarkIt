using MarkIt.settings;
using MarkIt.UserControls.usercontrolsColor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MarkIt.windows
{
    /// <summary>
    /// Interaktionslogik für WindowColorpicker.xaml
    /// </summary>
    public partial class WindowColorpicker : Window
    {
        private DispatcherTimer timer = new();
        private int layer = 0;
        public WindowColorpicker(int layer)
        {
            this.layer = layer;
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(15);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            byte rc = Convert.ToByte(Convert.ToInt32(Math.Round(r.Value,0))); // autocompletion
            byte gc = Convert.ToByte(Convert.ToInt32(Math.Round(g.Value, 0)));
            byte bc = Convert.ToByte(Convert.ToInt32(Math.Round(b.Value, 0)));
            ColorDisplay.ChangeColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(rc,gc,bc));
        }

        private void CustomButton_PreviewMouseLeftButtonDown2(object sender, MouseButtonEventArgs e)
        {
            Logger.logger.Debug("Colorpicker closed");
            Close();
        }

        private void CustomButton_PreviewMouseLeftButtonDown1(object sender, MouseButtonEventArgs e)
        {
            if (layer == 1)
                WindowSettings.color_Background = ColorTheme.RGBToHEX(Convert.ToInt32(r.Value), Convert.ToInt32(g.Value), Convert.ToInt32(b.Value));
            if (layer == 2)
                WindowSettings.color_Forground = ColorTheme.RGBToHEX(Convert.ToInt32(r.Value), Convert.ToInt32(g.Value), Convert.ToInt32(b.Value));
            if (layer == 3)
                WindowSettings.color_Middle = ColorTheme.RGBToHEX(Convert.ToInt32(r.Value), Convert.ToInt32(g.Value), Convert.ToInt32(b.Value));
            Logger.logger.Debug("Colorpicker closed with settings save");
            Close();
        }
    }
}
