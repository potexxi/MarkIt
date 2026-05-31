using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Interaktionslogik für CustomLine.xaml
    /// </summary>
    public partial class CustomLine : UserControl
    {
        private int _fontsize = 40;
        public int fontsize
        {
            get
            {
                return _fontsize;
            }
            set
            {
                _fontsize = value;
                TextboxLineText.FontSize = _fontsize;
            }
        }
        public TextBox CT_TextBox
        {
            get
            {
                return TextboxLineText;
            }
            set
            {
                TextboxLineText = value;
            }
        }
        public CustomLine()
        {
            InitializeComponent();
        }
    }
}
