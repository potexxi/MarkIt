using MarkIt.login_register;
using Supabase.Gotrue;
using System.Windows;
using System.Windows.Controls;

namespace MarkIt
{
    /// <summary>
    /// Interaction logic for WindowUserLogin.xaml
    /// </summary>
    public partial class WindowUserLogin : Window
    {
        public static Dictionary<string, Page>? pages;
        public static Frame? FrameMain;
        public static bool Guest = false;
        public static WindowUserLogin? window;
        public static UserManager UserManager = new UserManager();
        public WindowUserLogin()
        {
            InitializeComponent();
            window = this;
            FrameMain = MainFrame;
            pages = new Dictionary<string, Page>();
            pages.Add("PageLogin", new PageLogin());
            List<Session>? userList = UserManager.GetRemeberedUsers();
            //UserManager.GetUsersSupa();
            if (userList == null || userList.Count == 0)
            {
                FrameMain.Navigate(pages["PageLogin"]);
            }
            else
            {
                FrameMain.Navigate(new PageRememberedUser(userList));
            }   
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(!Guest)
                Environment.Exit(0);
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
