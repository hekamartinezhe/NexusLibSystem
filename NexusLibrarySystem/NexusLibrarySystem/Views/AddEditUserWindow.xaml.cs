using System.Windows;
using NexusLibrarySystem.Models;
using NexusLibrarySystem.Data;


namespace NexusLibrarySystem.Views
{
    public partial class AddEditUserWindow : Window
    {
        private readonly User _editingUser;
        private readonly bool _isEditMode;

        public AddEditUserWindow(User userToEdit = null)
        {
            InitializeComponent();

            RoleComboBox.ItemsSource = new string[] { "Admin", "Student" };
            _editingUser = userToEdit;
            _isEditMode = _editingUser != null;

            if (_isEditMode)
            {
                Title = "Edit User";
                TxtFullName.Text = _editingUser.FullName;
                TxtEnrollment.Text = _editingUser.EnrollmentNum;
                RoleComboBox.SelectedItem = _editingUser.Role;
                TxtEnrollment.IsEnabled = false;
                PwdBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                Title = "Add New User";
                RoleComboBox.SelectedIndex = 0;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string fullName = TxtFullName.Text.Trim();
            string enrollment = TxtEnrollment.Text.Trim();
            string role = RoleComboBox.SelectedItem?.ToString();
            string password = PwdBox.Password;

            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(enrollment) ||
                string.IsNullOrWhiteSpace(role) ||
                (!_isEditMode && string.IsNullOrWhiteSpace(password)))
            {
                ShowError("All fields are required. Password is required for new users.");
                return;
            }

            // Validar duplicados en número de control solo en modo "Add"
            if (!_isEditMode && UserData.UserExists(enrollment))
            {
                ShowError("Enrollment number already exists. Please use a different one.");
                return;
            }

            if (_isEditMode)
            {
                _editingUser.FullName = fullName;
                _editingUser.Role = role;
                _editingUser.IsActive = true;

                bool updated = UserData.UpdateUser(_editingUser);

                if (updated)
                    DialogResult = true;
                else
                    ShowError("Failed to update user.");
            }
            else
            {
                var newUser = new User
                {
                    FullName = fullName,
                    EnrollmentNum = enrollment,
                    Role = role,
                    IsActive = true
                };

                bool added = UserData.AddUser(newUser, password);

                if (added)
                    DialogResult = true;
                else
                    ShowError("Failed to add user. Enrollment number may already exist.");
            }
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ShowError(string message)
        {
            LblError.Text = message;
            LblError.Visibility = Visibility.Visible;
        }
    }
}
