using System.Windows;
using System.Windows.Controls;

namespace NexusLibrarySystem
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Opcional: carga inicial de la página Dashboard
            MainFrame.Navigate(new Views.DashboardPage());
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
            // Cierra la aplicación o vuelve a la ventana login
            Application.Current.Shutdown();
        }
    }
}

