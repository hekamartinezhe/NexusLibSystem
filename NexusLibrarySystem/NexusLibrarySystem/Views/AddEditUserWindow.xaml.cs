using System;
using System.Collections.Generic;
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

            MajorComboBox.ItemsSource = MajorData.GetAllMajors();

            if (_isEditMode)
            {
                Title = "Edit User";
                TxtFullName.Text = _editingUser.FullName;
                TxtEnrollment.Text = _editingUser.EnrollmentNum;
                RoleComboBox.SelectedItem = _editingUser.Role;
                MajorComboBox.SelectedValue = _editingUser.MajorId;
                TxtEnrollment.IsEnabled = false;
                PwdBox.Visibility = Visibility.Collapsed;

                if (_editingUser.Role == "Student")
                {
                    TxtQuadrimester.Text = _editingUser.Quadrimester?.ToString();
                    TxtQuadrimester.Visibility = Visibility.Visible;
                    LblQuadrimester.Visibility = Visibility.Visible;
                }
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
                MajorComboBox.SelectedItem == null ||
                (!_isEditMode && string.IsNullOrWhiteSpace(password)))
            {
                ShowError("All fields are required. Password is required for new users.");
                return;
            }

            int? quadrimester = null;
            if (role == "Student")
            {
                if (!int.TryParse(TxtQuadrimester.Text.Trim(), out int parsedQ) || parsedQ < 1 || parsedQ > 12)
                {
                    ShowError("Quadrimester must be a number between 1 and 12.");
                    return;
                }
                quadrimester = parsedQ;
            }

            int majorId = (int)MajorComboBox.SelectedValue;

            if (!_isEditMode && UserData.UserExists(enrollment))
            {
                ShowError("Enrollment number already exists.");
                return;
            }

            if (_isEditMode)
            {
                _editingUser.FullName = fullName;
                _editingUser.Role = role;
                _editingUser.Quadrimester = quadrimester;
                _editingUser.MajorId = majorId;

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
                    IsActive = true,
                    Quadrimester = quadrimester,
                    MajorId = majorId
                };

                bool added = UserData.AddUser(newUser, password);
                if (added)
                    DialogResult = true;
                else
                    ShowError("Failed to add user.");
            }
        }

        private void RoleComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            bool isStudent = RoleComboBox.SelectedItem?.ToString() == "Student";
            TxtQuadrimester.Visibility = isStudent ? Visibility.Visible : Visibility.Collapsed;
            LblQuadrimester.Visibility = isStudent ? Visibility.Visible : Visibility.Collapsed;
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
