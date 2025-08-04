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
                var result = MessageBox.Show($"Are you sure you wanna delete user '{selectedUser.FullName}'?",
                                             "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    bool success = UserData.MarkUserAsDeleted(selectedUser.UserId);
                    if (success)
                    {
                        LoadUsers(); // actualiza el DataGrid sin ese usuario
                        MessageBox.Show("User Deleted.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("There was an error while deleting.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Select a user.", "Delete User", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }



        private void DeactivateUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                if (!selectedUser.IsActive)
                {
                    MessageBox.Show("El usuario ya está inactivo.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var result = MessageBox.Show($"¿Seguro que quieres desactivar al usuario '{selectedUser.FullName}'?",
                                             "Confirmar desactivación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    bool success = UserData.DeactivateUser(selectedUser.UserId);
                    if (success)
                    {
                        LoadUsers();
                        MessageBox.Show("Usuario desactivado correctamente.", "Desactivado", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error al desactivar usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona un usuario para desactivar.", "Desactivar usuario", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ActivateUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                if (selectedUser.IsActive)
                {
                    MessageBox.Show("El usuario ya está activo.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var result = MessageBox.Show($"¿Seguro que quieres activar al usuario '{selectedUser.FullName}'?",
                                             "Confirmar activación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    bool success = UserData.ActivateUser(selectedUser.UserId);
                    if (success)
                    {
                        LoadUsers();
                        MessageBox.Show("Usuario activado correctamente.", "Activado", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error al activar usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona un usuario para activar.", "Activar usuario", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ViewLoans_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                var loansWindow = new LoansManagementWindow(selectedUser);
                loansWindow.Owner = Window.GetWindow(this);
                loansWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Selecciona un usuario para ver sus préstamos.", "Ver Préstamos", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
