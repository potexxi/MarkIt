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
        public static Frame frame;
        public static bool Guest = false;
        public static WindowUserLogin window;
        public WindowUserLogin()
        {
            InitializeComponent();
            window = this;
            frame = MainFrame;
            pages = new Dictionary<string, Page>();
            pages.Add("PageLogin", new PageLogin());
            pages.Add("PageRegister", new PageRegister());
            pages.Add("PagePassword1", new PageRecetPassword1());
            pages.Add("PagePassword2", new PageRecetPassword2());
            pages.Add("PagePassword3", new PageResetPassword3());
            pages.Add("Page2FA", new Page2FA());

            MainFrame.Navigate(pages["PageLogin"]);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(!Guest)
                Application.Current.Shutdown();
        }
    }
}
