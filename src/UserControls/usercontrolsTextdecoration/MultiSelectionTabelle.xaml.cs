using MarkIt.UserControls.usercontrolsTextdecoration;
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
    /// Interaktionslogik für MultiSelectionTabelle.xaml
    /// </summary>
    public partial class MultiSelectionTabelle : UserControl
    {
        public int height = 7;
        public int width = 7;

        private int MAXheight = 7; // if you wannt bigger tabels you can change the size here
        private int MAXwidth = 7;


        private bool animation = MainWindow.GeneralSettings.iconAnimations;


        public MultiSelectionTabelle()
        {
            InitializeComponent();
        }

        public void updateSettings()
        {
            animation = MainWindow.GeneralSettings.iconAnimations;
            RectBackground.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
            RectBackground.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);

            el2.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);

            el1.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
            el2.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
            el3.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
            el4.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
        }

        private async void CB_Hitbox_MouseLeave(object sender, MouseEventArgs e)
        {
            updateSettings();

            await Task.Delay(150); // chatgpt anfang

            if (!CB_Hitbox.IsMouseOver && !PopupContent.IsMouseOver)
            {
                TablePopup.IsOpen = false;// chatgpt ende
            }
        }

        private async void Popup_MouseLeave(object sender, MouseEventArgs e)
        {
            await Task.Delay(150);// chatgpt anfang

            if (!CB_Hitbox.IsMouseOver && !PopupContent.IsMouseOver)
            {
                TablePopup.IsOpen = false;// chatgpt ende
                updateSettings();
            }
        }

        private void CB_Hitbox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (animation)
            {
                RectBackground.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                el2.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);

                el1.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
                el2.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
                el3.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
                el4.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
            }
            SP_TableGrid.Children.Clear();
            PopupContent.Background = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);

            for (int x = 0; x < MAXwidth; x++)
            {
                StackPanel SP_x = new StackPanel();
                SP_x.Orientation = Orientation.Horizontal;
                for (int y = 0; y < MAXheight; y++)
                {
                    Rectangle sel = new();
                    sel.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.BackgroundColor);
                    sel.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
                    sel.Height = 25;
                    sel.Width = 25;
                    sel.Tag = $"{x + 1},{y + 1}";
                    sel.MouseEnter += Sel_MouseEnter;
                    sel.MouseLeave += Sel_MouseLeave;
                    sel.MouseDown += Sel_MouseDown;
                    SP_x.Children.Add(sel);
                }
                SP_TableGrid.Children.Add(SP_x); //chatgpt helped me with the pop items
            }
            TablePopup.IsOpen = true;
        }

        private void Sel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle sel = (Rectangle)sender;
            string pos = (string)sel.Tag;
            string[] posList = pos.Split(",");
            Logger.logger.Debug($"New Tabel created with the size: Width={posList[1]} Height={posList[1]}");
            width = Convert.ToInt32(posList[1]);
            height = Convert.ToInt32(posList[0]);
            TablePopup.IsOpen = false;
        }

        private void Sel_MouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle sel = (Rectangle)sender;
            sel.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
            sel.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.BackgroundColor);
        }

        private void Sel_MouseEnter(object sender, MouseEventArgs e)
        {
            Rectangle sel = (Rectangle)sender;
            sel.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
            sel.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
        }
    }
}
