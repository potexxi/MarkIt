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

namespace MarkIt.UserControls.usercontrolsColor
{
    /// <summary>
    /// Interaktionslogik für ColorDisplay.xaml
    /// </summary>
    public partial class ColorDisplay : UserControl
    {
        public Brush ChangeColor
        {
            get
            {
                return RectDisplay.Fill;
            }
            set
            {
                //chatgpt begin
                //promt: how can I make sure value is a brush
                if (value is SolidColorBrush brush)
                //chatgpt ende
                {
                    RectDisplay.Fill = value;
                }
            }
        }
        public ColorDisplay()
        {
            InitializeComponent();
        }
    }
}
