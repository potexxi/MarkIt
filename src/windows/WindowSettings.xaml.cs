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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MarkIt.windows
{
    /// <summary>
    /// Interaktionslogik für WindowSettings.xaml
    /// </summary>
    public partial class WindowSettings : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        static public string color_Background = "";
        static public string color_Middle = "";
        static public string color_Forground = "";

        public WindowSettings()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMicroseconds(15);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
        }

        private void SwitchSlider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool test = AnimationSetting.IsOn; // to test if it works (it does!)
        }
        private void updateColorDisplays()
        {
            if (color_Background != "")
                CD_Backgorund.ChangeColor = (Brush)new BrushConverter().ConvertFromString(color_Background); // color
            if (color_Forground != "")
                CD_Forground.ChangeColor = (Brush)new BrushConverter().ConvertFromString(color_Forground); // color
            if (color_Middle != "")
                CD_Middle.ChangeColor = (Brush)new BrushConverter().ConvertFromString(color_Middle); // color
        }

        private void Background_CustomButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowColorpicker cp = new WindowColorpicker(1);
            cp.ShowDialog();
            updateColorDisplays();
        }

        private void Middle_CustomButton_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            WindowColorpicker cp = new WindowColorpicker(3);
            cp.ShowDialog();
            updateColorDisplays();
        }

        private void Forground_CustomButton_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            WindowColorpicker cp = new WindowColorpicker(2);
            cp.ShowDialog();
            updateColorDisplays();
        }
    }
}
