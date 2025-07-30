using System.Windows;
using NexusLibrarySystem;

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

            // TODO: Use real authentication logic here
            if ((enrollment.Equals("admin", System.StringComparison.OrdinalIgnoreCase) && password == "admin") ||
                (enrollment.StartsWith("S") && password == "student"))
            {
                string role = enrollment.Equals("admin", System.StringComparison.OrdinalIgnoreCase) ? "Admin" : "Student";

                // Ashow main window with the user's role
                MainWindow mainWindow = new MainWindow(role);
                mainWindow.Show();

                // Close the login window
                this.Close();
            }
            else
            {
                LblError.Text = "Invalid enrollment number or password.";
            }
        }
    }
}
