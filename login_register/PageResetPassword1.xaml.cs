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
using System.Net.Mail;
using System.Net.Mime;

namespace MarkIt.login_register
{
    /// <summary>
    /// Interaction logic for PageRecetPassword1.xaml
    /// </summary>
    public partial class PageRecetPassword1 : Page
    {

        public PageRecetPassword1()
        {
            InitializeComponent();
        }

        private async void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            string email = TextBoxEmail.Text;
            ClassUserList? userList = await WindowUserLogin.UserManager.GetUsersFromServerAndHandleErrors(LoadingScreen);
            if(userList == null)
            {
                return;
            }
            bool exists = false;
            foreach(ClassUser user in userList.Users)
            {
                if (user.Email == WindowUserLogin.EmailManager.Email) { exists = true; break; }
            }
            if (!exists)
            {
                MessageBox.Show("This user does not exist. Enter an existing user or create an account!", "No user", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }
            if (await WindowUserLogin.EmailManager.SendEmailAndHandleErrors(email, LoadingScreen))
            {
                WindowUserLogin.Navigate("PagePassword1", "PagePassword2");
                PageRecetPassword2.Timer.Start();
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            WindowUserLogin.Navigate("PagePassword1", "PageLogin");
        }
    }
}
