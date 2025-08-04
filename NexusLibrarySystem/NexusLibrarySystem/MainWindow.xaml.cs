using NexusLibrarySystem.Models;
using NexusLibrarySystem.Views;
using System.Windows;

namespace NexusLibrarySystem
{
    public partial class MainWindow : Window
    {
        private readonly User _currentUser;
        public string FullName => _currentUser.FullName;

        public MainWindow(User user)
        {
            InitializeComponent();

            _currentUser = user;
            DataContext = this;


            AdjustMenuByRole();

            MainFrame.Navigate(new DashboardPage());
        }

        public string NombreUsuario => _currentUser.FullName;

        private void AdjustMenuByRole()
        {
            if (_currentUser.Role.ToLower() != "admin")
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
            MainFrame.Navigate(new DashboardPage());
        }

        private void Books_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BooksPage(_currentUser));
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UsersPage());
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProfilePage(_currentUser));
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            this.Close();
        }
    }
}
