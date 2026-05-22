using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace MarkIt.windows
{
    /// <summary>
    /// Interaktionslogik für WindowInfoTab.xaml
    /// </summary>
    public partial class WindowInfoTab : Window
    {
        public WindowInfoTab()
        {
            InitializeComponent();
            Background.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.BackgroundColor);
            SolidColorBrush font = (SolidColorBrush)FindResource("LabelForeground");
            font.Color = (Color)ColorConverter.ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            // CHATGPT-Anfang
            // Prompt: wie hyperlink in XAML
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)
            {
                UseShellExecute = true
            });

            e.Handled = true;
            // CHATGPT-Ende
        }

        private void CustomButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
