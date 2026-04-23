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
        public PageResetPassword3()
        {
            InitializeComponent();
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            if(Password1.Password != Password2.Password)
            {
                Password1.BorderThickness = new Thickness(3);
                Password1.BorderBrush = Brushes.LightCoral;
                LabelPasswordFalse1.Visibility = Visibility.Visible;
                Password2.BorderThickness = new Thickness(3);
                Password2.BorderBrush = Brushes.LightCoral;
                LabelPasswordFalse2.Visibility = Visibility.Visible;
            }
            else
            {
                ClassUserList userList = PageLogin.GetUsersFromServer();
                foreach(ClassUser user in userList.Users)
                {
                    if (user.Email == PageRecetPassword1.email)
                    {
                        user.Password = Password2.Password;
                    }
                }
                PageLogin.WriteUsersToServer(userList);
                WindowUserLogin.Navigate("PagePassword3", "PageLogin");
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            WindowUserLogin.Navigate("PagePassword3", "PageLogin");
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password1.BorderBrush = Brushes.Gray;
            Password1.BorderThickness = new Thickness(1);
            LabelPasswordFalse1.Visibility = Visibility.Hidden;
            Password2.BorderBrush = Brushes.Gray;
            Password2.BorderThickness = new Thickness(1);
            LabelPasswordFalse2.Visibility = Visibility.Hidden;
        }
    }
}
