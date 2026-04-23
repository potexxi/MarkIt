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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarkIt
{
    /// <summary>
    /// Interaction logic for WindowUserLogin.xaml
    /// </summary>
    public partial class WindowUserLogin : Window
    {
        public static Dictionary<string, Page> pages;
        public static Frame FrameMain;
        public static bool Guest = false;
        public static WindowUserLogin window;
        public WindowUserLogin()
        {
            InitializeComponent();
            window = this;
            FrameMain = MainFrame;
            pages = new Dictionary<string, Page>();
            pages.Add("PageLogin", new PageLogin());
            FrameMain.Navigate(pages["PageLogin"]);
            // TODO: check for remembered users and build a ui, to select a user, like in netflix
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(!Guest)
                Application.Current.Shutdown();
        }

        public static void Navigate(string currentpage, string nextpage)
        {
            if (!pages.ContainsKey(nextpage))
            {
                if (nextpage == "PageRegister")
                    pages.Add("PageRegister", new PageRegister());
                else if (nextpage == "PagePassword1")
                    pages.Add("PagePassword1", new PageRecetPassword1());
                else if (nextpage == "PagePassword2")
                    pages.Add("PagePassword2", new PageRecetPassword2());
                else if (nextpage == "PagePassword3")
                    pages.Add("PagePassword3", new PageResetPassword3());
                else if (nextpage == "Page2FA")
                    pages.Add("Page2FA", new Page2FA());
            }
            FrameMain.Navigate(pages[nextpage]);
        }
    }
}
