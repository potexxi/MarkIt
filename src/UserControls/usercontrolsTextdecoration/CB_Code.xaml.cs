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
    /// Interaktionslogik für CB_Code.xaml
    /// </summary>
    public partial class CB_Code : UserControl
    {
        static public Brush hovercolor = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground); // chatgpt this specific line might be used very often
        static public Brush defaultcolor = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
        static public Brush Backgroundcolor = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);


        public CB_Code()
        {
            InitializeComponent();
        }

        private void CB_Hitbox_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void CB_Hitbox_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void CB_Hitbox_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
