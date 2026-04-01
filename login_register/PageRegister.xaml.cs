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

namespace MarkIt.login_register
{
    /// <summary>
    /// Interaction logic for PageRegister.xaml
    /// </summary>
    public partial class PageRegister : Page
    {
        private Frame _frame;
        public PageRegister(Frame frame)
        {
            InitializeComponent();
            _frame = frame;
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            //PageLogin pageLogin = new PageLogin(_frame);
            _frame.Navigate(WindowUserLogin.pages["PageLogin"]);
        }
    }
}
