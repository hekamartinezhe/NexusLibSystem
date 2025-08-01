using System.Windows;

namespace NexusLibrarySystem
{
    public partial class MainWindow : Window
    {
        private readonly string _userRole;
        private readonly string _userName;

        public MainWindow(string userRole, string userName = "User")
        {
            InitializeComponent();

            _userRole = userRole;
            _userName = userName;

            DataContext = this;  // Para el binding del nombre de usuario en la UI

            AdjustMenuByRole();

            MainFrame.Navigate(new Views.DashboardPage());
        }

        public string NombreUsuario => _userName;

        private void AdjustMenuByRole()
        {
            if (_userRole.ToLower() != "admin")
            {
                BtnUsers.Visibility = Visibility.Collapsed;
                BtnBooks.Content = "Browse Books";
            }
            else
            {
                BtnUsers.Visibility = Visibility.Visible;
                BtnBooks.Content = "Books";
                BtnProfile.Visibility = Visibility.Collapsed;
            }
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.DashboardPage());
        }

        private void Books_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.BooksPage());
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.UsersPage());
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.ProfilePage());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
