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
        public FileBar(List<string> history)
        {
            InitializeComponent();
            DrawHistory(history);
        }

        public void DrawHistory(List<string> history)
        {
            BorderMain.Background = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.BackgroundColor);
            LabelRecent.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
            foreach (string item in history)
            {
                Label label = new Label();
                label.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                label.MouseEnter += Label_MouseEnter;
                label.MouseLeave += Label_MouseLeave;
                label.Cursor = Cursors.Hand;
                label.FontFamily = new FontFamily("Leelawadee UI");
                label.FontSize = 14;
                string[] parts = item.Split("/");
                label.Content = string.Join("/" ,parts.Skip(2));
                StackPanelHistory.Children.Add(label);
            }
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            label.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            label.Foreground = (Brush) new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
        }

        private void Rectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Parent is Panel panel)
            {
                panel.Children.Remove(this);
            }
        }

        public void SetSize(double width, double height)
        {
            this.Height = height;
            this.Width = width;
            RectBackground.Height = height;
            RectBackground.Width = width;
            BorderMain.Width = 0.3 * width;
            BorderMain.Height = 0.97 * height - 50;
            ScrollViewerMain.Height = 0.78 * (0.97 * height - 50);
            ScrollViewerMain.Width = 0.3 * width;
        }
    }
}
