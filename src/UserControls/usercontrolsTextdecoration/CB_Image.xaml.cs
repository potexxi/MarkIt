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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarkIt.UserControls.usercontrolsTextdecoration
{
    /// <summary>
    /// Interaktionslogik für CB_Image.xaml
    /// </summary>
    public partial class CB_Image : UserControl
    {
        private bool animation = MainWindow.GeneralSettings.iconAnimations;
        public CB_Image()
        {
            InitializeComponent();
        }

        public void updateSettings()
        {
            animation = MainWindow.GeneralSettings.iconAnimations;
            RectBackground.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
            RectBackground.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);

            el1.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
            el2.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
            el3.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
            Logger.logger.Verbose("[UC] Updated Settings Image");

        }
        private void CB_Hitbox_MouseLeave(object sender, MouseEventArgs e)
        {
            updateSettings();
        }

        private void CB_Hitbox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (animation)
            {
                RectBackground.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                el1.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
                el2.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
                el3.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
            }
        }

        private void CB_Hitbox_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
