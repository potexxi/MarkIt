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

namespace MarkIt.UserControls.usercontrolsTextdecoration
{
    /// <summary>
    /// Interaktionslogik für CB_Bold.xaml
    /// </summary>
    public partial class CB_Bold : UserControl
    {
        static public Brush hovercolor = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground); // chatgpt this specific line might be used very often
        static public Brush defaultcolor = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
        static public Brush Backgroundcolor = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
        public bool animation = false;

        public CB_Bold()
        {
            InitializeComponent();
        }

        public void updateSettings()
        {
            animation = MainWindow.GeneralSettings.iconAnimations;
            RectBackground.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
            RectBackground.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);

            Rect_B.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
            Ellipse_Bl.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
            Ellipse_Bu.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);

            Rectbackground.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
            Logger.logger.Verbose("[UC] Updated Settings Bold");
        }

        private void CB_Hitbox_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void CB_Hitbox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (animation)
            {
                RectBackground.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                RectBackground.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);

                Rect_B.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
                Ellipse_Bl.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
                Ellipse_Bu.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);

                Rectbackground.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
            }
        }

        private void CB_Hitbox_MouseLeave(object sender, MouseEventArgs e)
        {
            updateSettings();
        }
    }
}
