using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
                Button button = new Button();
                button.Content = user.Email;
                button.Height = 40;
                button.FontSize = 15;
                button.FontFamily = new FontFamily("Leelawadee UI");
                button.Margin = new Thickness(0, 6, 0, 0);
                button.Cursor = Cursors.Hand;
                button.Tag = user;
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
