using MarkIt.login_register;
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
using System.Windows.Shapes;

namespace MarkIt
{
    /// <summary>
    /// Interaction logic for WindowUserLogin.xaml
    /// </summary>
    public partial class WindowUserLogin : Window
    {
        public static Dictionary<string, Page> pages;
        public WindowUserLogin()
        {
            InitializeComponent();
            pages = new Dictionary<string, Page>();
            pages.Add("PageLogin", new PageLogin(MainFrame, this));
            pages.Add("PageRegister", new PageRegister(MainFrame));
            pages.Add("PagePassword1", new PageRecetPassword1(MainFrame));
            pages.Add("PagePassword2", new PageRecetPassword2(MainFrame));
            pages.Add("PagePassword3", new PageResetPassword3(MainFrame));
            pages.Add("Page2FA", new Page2FA(MainFrame, this));

            MainFrame.Navigate(pages["PageLogin"]);
        }
    }
}
