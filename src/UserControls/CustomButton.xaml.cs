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
    /// Interaktionslogik für CustomButton.xaml
    /// </summary>
    public partial class CustomButton : UserControl
    {
        public CustomButton()
        {
            InitializeComponent();
        }

        public string CustomContent
        {
            set{ButtonContent.Content = value;}
            get
            {
                if (ButtonContent != null)
                    return (string)ButtonContent.Content;
                else return "null";
            }
        }

        private void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            RectButton.Fill = Brushes.DarkGray;
            RectButton.Stroke = Brushes.LightGray;
        }

        private void Rectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            RectButton.Fill = Brushes.LightGray;
            RectButton.Stroke = Brushes.Black;
        }
    }
}
