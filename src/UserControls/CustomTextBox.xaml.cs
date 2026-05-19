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
    /// Interaktionslogik für CustomTextBox.xaml
    /// </summary>
    public partial class CustomTextBox : UserControl
    {
        // user usable funktions values chnages
        public string CustomContent
        {
            get { return TextBoxCustom.Text; }
            set { TextBoxCustom.Text = value; }
        }
        public int MaxCharLength
        {
            get { return TextBoxCustom.MaxLength; }
            set { TextBoxCustom.MaxLength = value; }
        }
        public Brush CustomBackground
        {
            get { return RectTextbox.Fill; }
            set { RectTextbox.Fill = value; }
        }

        public bool disableHover { get; set; } = false;

        public event TextChangedEventHandler TextChanged; // custom event with help from chatgpt (this line)
        private void TextBoxCustom_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextChanged != null)
                TextChanged(sender, e);
        }
        public CustomTextBox()
        {
            InitializeComponent();
        }

        private void TextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!disableHover)
            {
                RectTextbox.Fill = Brushes.DarkGray;
                RectTextbox.Stroke = Brushes.LightGray;
            }
        }
        private void TextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!disableHover)
            {
                RectTextbox.Fill = Brushes.LightGray;
                RectTextbox.Stroke = Brushes.Black;
            }
        }

    }
}
