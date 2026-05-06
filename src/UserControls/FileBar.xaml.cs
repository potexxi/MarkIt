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

namespace MarkIt.UserControls
{
    /// <summary>
    /// Interaktionslogik für FileBar.xaml
    /// </summary>
    public partial class FileBar : UserControl
    {
        private ColorTheme colortheme;
        public FileBar(List<string> history, ColorTheme colortheme)
        {
            InitializeComponent();
            DrawHistory(history, colortheme);
        }

        public void DrawHistory(List<string> history, ColorTheme colortheme)
        {
            this.colortheme = colortheme;
            BorderMain.Background = (Brush)new BrushConverter().ConvertFromString(colortheme.BackgroundColor);
            foreach(string item in history)
            {
                Label label = new Label();
                label.Foreground = (Brush)new BrushConverter().ConvertFromString(colortheme.Foreground);
                label.MouseEnter += Label_MouseEnter;
                label.MouseLeave += Label_MouseLeave;
                label.Cursor = Cursors.Hand;
                label.FontFamily = new FontFamily("Leelawadee UI");
                label.FontSize = 12;
                string[] parts = item.Split("/");
                label.Content = string.Join("/" ,parts.Skip(2));
                StackPanelHistory.Children.Add(label);
            }
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            label.Foreground = (Brush)new BrushConverter().ConvertFromString(colortheme.Foreground);
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            label.Foreground = (Brush) new BrushConverter().ConvertFromString(colortheme.HoverColor);
        }
    }
}
