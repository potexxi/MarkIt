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
    /// Interaction logic for PageResetPassword3.xaml
    /// </summary>
    public partial class PageResetPassword3 : Page
    {
        private Frame _frame;
        public PageResetPassword3(Frame frame)
        {
            InitializeComponent();
            _frame = frame;
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            _frame.Navigate(WindowUserLogin.pages["PageLogin"]);
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            _frame.Navigate(WindowUserLogin.pages["PageLogin"]);
        }
    }
}
