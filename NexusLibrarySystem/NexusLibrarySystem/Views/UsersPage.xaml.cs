using System.Windows;
using System.Windows.Controls;
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
            try
            {
                var users = UserData.GetAllUsers();
                UsersDataGrid.ItemsSource = users;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshUsers_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddEditUserWindow();
            if (window.ShowDialog() == true)
            {
                LoadUsers();
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                var window = new AddEditUserWindow(selectedUser);
                if (window.ShowDialog() == true)
                {
                    LoadUsers();
                }
            }
            else
            {
                MessageBox.Show("Please select a user to edit.", "Edit User", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                var result = MessageBox.Show($"Are you sure you want to delete user '{selectedUser.FullName}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    bool success = UserData.DeleteUser(selectedUser.UserId);
                    if (success)
                    {
                        LoadUsers();
                        MessageBox.Show("User deleted successfully.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.", "Delete User", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
