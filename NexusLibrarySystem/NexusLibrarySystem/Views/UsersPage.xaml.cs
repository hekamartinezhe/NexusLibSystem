using System.Windows.Controls;
using System.Collections.Generic;
using NexusLibrarySystem.Models;
using NexusLibrarySystem.Data;

namespace NexusLibrarySystem.Views
{
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            List<User> users = UserData.GetAllUsers();
            UsersDataGrid.ItemsSource = users;
        }

        private void RefreshUsers_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadUsers();
        }
    }
}

