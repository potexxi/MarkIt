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
    /// Interaktionslogik für MultiSelectItem.xaml
    /// </summary>
    public partial class MultiSelectItem : UserControl
    {

        public string StringSelection
        {
            get
            {
                if(Label_Item != null)
                    return Label_Item.Content.ToString();
                else
                {
                    return "ERROR";
                }
            }
            set
            {
                Label_Item.Content = (string)value;
            }
        }
        public MultiSelectItem()
        {
            InitializeComponent();
            setColor();
        }

        public void setColor()
        {
            Rect_BG.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
            Rect_BG.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
            Label_Item.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
        }

        private void Rect_hitbox_MouseEnter(object sender, MouseEventArgs e)
        {
            Rect_BG.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
            Label_Item.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
        }

        private void Rect_hitbox_MouseLeave(object sender, MouseEventArgs e)
        {
            Rect_BG.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
            Label_Item.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
        }
    }
}
