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
        public PageRegister()
        {
            InitializeComponent();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            WindowUserLogin.Navigate("PageRegister", "PageLogin");
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            if(TextBoxPassword1.Password == TextBoxPassword2.Password)
            {

            }
            else
            {
                TextBoxPassword1.BorderBrush = Brushes.LightCoral;
                TextBoxPassword2.BorderBrush = Brushes.LightCoral;
                TextBoxPassword1.BorderThickness = new Thickness(3);
                TextBoxPassword2.BorderThickness = new Thickness(3);
                LabelPasswordNotCorrect1.Visibility = Visibility.Visible;
                LabelPasswordNotCorrect2.Visibility = Visibility.Visible;
            }
        }

        private void TextBoxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            TextBoxPassword1.BorderBrush = Brushes.Gray;
            TextBoxPassword2.BorderBrush = Brushes.Gray;
            TextBoxPassword1.BorderThickness = new Thickness(1);
            TextBoxPassword2.BorderThickness = new Thickness(1);
            LabelPasswordNotCorrect1.Visibility = Visibility.Hidden;
            LabelPasswordNotCorrect2.Visibility = Visibility.Hidden;
        }
    }
}
