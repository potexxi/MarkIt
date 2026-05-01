using Supabase.Gotrue;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace MarkIt.login_register
{
    public partial class PageRememberedUser : Page
    {
        private List<Session> _sessions;

        public PageRememberedUser(List<Session> sessions)
        {
            InitializeComponent();
            _sessions = sessions;
            LoadUserButtons();
        }

        private void LoadUserButtons()
        {
            StackPanelUsers.Children.Clear();
            foreach (Session session in _sessions)
            {
                Button button = new Button();
                button.Content = session.User.Email;
                button.Height = 40;
                button.FontSize = 15;
                button.FontFamily = new FontFamily("Leelawadee UI");
                button.Margin = new Thickness(0, 6, 0, 0);
                button.Cursor = Cursors.Hand;
                button.Tag = session;
                button.Click += UserButton_Click;
                StackPanelUsers.Children.Add(button);
            }
        }

        private async void UserButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Session session = (Session)button.Tag;
            MainWindow.currentSession = (Session)button.Tag;
            try
            {
                LoadingScreen.Visibility = Visibility.Visible;
                await MainWindow.supabase.Auth.SetSession(session.AccessToken, session.RefreshToken);
                LoadingScreen.Visibility = Visibility.Hidden;
                WindowUserLogin.Guest = true;
                WindowUserLogin.window.Close();
            }
            catch(Exception ex)
            {
                File.Delete("sources/remembered.json");
                LoadingScreen.Visibility = Visibility.Hidden;
                MessageBox.Show("Our server caused a fatal error. Please relogin!", "Relogin", MessageBoxButton.OK, MessageBoxImage.Information);
                Logger.logger.Debug($"Remembered Users: {ex.Message}");
                WindowUserLogin.Navigate("Remembered", "PageLogin");
            }
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
