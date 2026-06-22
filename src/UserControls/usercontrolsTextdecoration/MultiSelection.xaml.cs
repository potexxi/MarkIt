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
    /// Interaktionslogik für MultiSelection.xaml
    /// </summary>
    public partial class MultiSelection : UserControl
    {
        private string[] selection;
        public int selectionIndex = 0;
        private bool animation = MainWindow.GeneralSettings.iconAnimations;

        public MultiSelection()
        {
            InitializeComponent();
            updateIDX();
        }
        public void setSelection(string[] newselection)
        {
            selection = newselection;
            selectionIndex = 0;
            updateIDX();
        }
        public void updateSettings()
        {
            animation = MainWindow.GeneralSettings.iconAnimations;

            Rect_BG.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
            Rect_BG.Stroke = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
            Label_Selection.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
        }
        public void updateIDX()
        {
            if(selection != null && selectionIndex != null)
            Label_Selection.Content = selection[selectionIndex];
        }
        private void Rect_Hitbox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (animation)
            {
                Rect_BG.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                Label_Selection.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);
            }
            if (selection == null) // savety check (NOT FORM KI) but I asked a question and I he told me if selection is not set it could potentialy couse an error so I added this to make sure it doesnt through an error
                return;
            foreach(string sel in selection)
            {
                MultiSelectItem item = new MultiSelectItem();
                item.StringSelection = sel;
                item.Width = 300;
                item.Tag = sel;// help from chatgpt (but typed by me)
                item.PreviewMouseDown += Item_PreviewMouseDown; // help from chatgpt (but typed by me)
                Stackpannel_Items.Children.Add(item);
            }
            DropdownPopup.IsOpen = true; // chat helped me with popup to have it go over
        }

        private void Item_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            {
                for (int i = 0; i < selection.Length; i++)
                {
                    MultiSelectItem item = (MultiSelectItem)sender;
                    string tag = (string)item.Tag; // tag idear and help coding this from chatgpt
                    if (selection[i] == tag)
                    {
                        selectionIndex = i;
                        updateIDX();
                        Stackpannel_Items.Children.Clear();
                        DropdownPopup.IsOpen = false;
                        return;
                    }
                }
            }
        }

        private async void Rect_Hitbox_MouseLeave(object sender, MouseEventArgs e)
        { // chatGPT alot in this class like 90%
            Rect_BG.Fill = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Textcolor);// chatGPT promt everything + fix this pls
            Label_Selection.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
            await Task.Delay(150);

            if (!Rect_Hitbox.IsMouseOver && !DropdownPopup.IsMouseOver)
            {
        // chatGPT ende
                Stackpannel_Items.Children.Clear();
                DropdownPopup.IsOpen = false; // chatgpt this line
            }
        }
        private void DropdownItems_MouseLeave(object sender, MouseEventArgs e)
        {
            Stackpannel_Items.Children.Clear();
            DropdownPopup.IsOpen = false;
        }
    }
}
