using System;
using System.Windows;
using NexusLibrarySystem.Data;
using NexusLibrarySystem.Models;

namespace NexusLibrarySystem.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string enrollment = TxtEnrollment.Text.Trim();
            string password = TxtPassword.Password.Trim();
             //Console.WriteLine($"Enrollment: '{enrollment}'\nPassword: '{password}'\nHash: '{Database.HashPassword(password)}'");
            if (string.IsNullOrWhiteSpace(enrollment) || string.IsNullOrWhiteSpace(password))
            {
                LblError.Text = "Please enter both enrollment number and password.";
                return;
            }
           

            User user = UserData.ValidateLogin(enrollment, password);


            if (user != null)
            {
                if (!user.IsActive)
                {
                    LblError.Text = "Your account has been suspended due to late fees, contact library administration.";
                 

                }

                MainWindow mainWindow = new MainWindow(user);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                LblError.Text = "Invalid enrollment number or password.";
            }
        }
    }
}
