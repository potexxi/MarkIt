using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MarkIt.login_register
{
    public partial class PageRememberedUser : Page
    {
        private ClassUserList _userList;

        public PageRememberedUser(ClassUserList userList)
        {
            InitializeComponent();
            _userList = userList;
            LoadUserButtons();
        }

        private void LoadUserButtons()
        {
            StackPanelUsers.Children.Clear();
            foreach (ClassUser user in _userList.Users)
            {
                Button button = new Button
                {
                    Content = user.Email,
                    Height = 40,
                    FontSize = 15,
                    FontFamily = new System.Windows.Media.FontFamily("Leelawadee UI"),
                    Margin = new Thickness(0, 6, 0, 0),
                    Cursor = System.Windows.Input.Cursors.Hand,
                    // from Stackoverflow
                    Tag = user
                };
                button.Click += UserButton_Click;
                StackPanelUsers.Children.Add(button);
            }
        }

        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ClassUser user = (ClassUser)button.Tag;
            MainWindow.currentUser = user;
            WindowUserLogin.Guest = true;
            WindowUserLogin.window.Close();
        }

        private void ButtonOtherUser_Click(object sender, RoutedEventArgs e)
        {
            WindowUserLogin.Navigate("PageRemember", "PageLogin");
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            WindowUserLogin.Navigate("PageRemember", "PageRegister");
        }

        private void ButtonGuest_Click(object sender, RoutedEventArgs e)
        {
            WindowUserLogin.Guest = true;
            MainWindow.currentUser = new ClassUser(-1, "guest", "guest");
            WindowUserLogin.window.Close();
        }
    }
}
