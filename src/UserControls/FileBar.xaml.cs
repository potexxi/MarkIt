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
        private string selectedPath;
        public FileBar(List<string> history)
        {
            InitializeComponent();
            DrawHistory(history);
        }

        public void DrawHistory(List<string> history)
        {
            StackPanelHistory.Children.Clear();
            BorderMain.Background = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.BackgroundColor);
            LabelRecent.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
            if(history == null || history.Count == 0)
            {
                Label label = new Label();
                label.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                label.Content = "No recent files...";
                label.Margin = new Thickness(10, 0, 0, 0);
                label.FontFamily = new FontFamily("Leelawadee UI");
                label.FontSize = 14;
                StackPanelHistory.Children.Add(label);
            }
            foreach (string item in history)
            {
                Label label = new Label();
                label.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                label.MouseEnter += Label_MouseEnter;
                label.MouseLeave += Label_MouseLeave;
                label.PreviewMouseDoubleClick += LabelHistory_PreviewMouseDoubleClick;
                label.PreviewMouseDown += LabelHistory_PreviewMouseDown;
                label.Margin = new Thickness(10, -5, 0, 0);
                label.Cursor = Cursors.Hand;
                label.FontFamily = new FontFamily("Leelawadee UI");
                label.FontSize = 14;
                string[] parts = item.Split("/");
                label.Content = string.Join("/" ,parts.Skip(2));
                StackPanelHistory.Children.Add(label);
            }
        }

        private void LabelHistory_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach(Label label1 in  StackPanelHistory.Children)
            {
                label1.Background = Brushes.Transparent;
            }
            Label label = (Label)sender;
            label.Background = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.IconsColor);
            selectedPath = label.Content.ToString();
            ButtonDel.IsEnabled = false;
        }

        private void LabelHistory_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label label = (Label)sender;
            selectedPath = label.Content.ToString();
            MainWindow.FileManager.LoadFromFile(selectedPath);
            if (this.Parent is Panel panel)
            {
                panel.Children.Remove(this);
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

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            // TODO, but only from file tree deletable
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            if(selectedPath != "" && selectedPath != null)
            {
                MainWindow.FileManager.LoadFromFile(selectedPath);
                if (this.Parent is Panel panel)
                {
                    panel.Children.Remove(this);
                }
            }
        }
    }
}
