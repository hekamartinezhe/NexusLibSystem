using System.Collections.Generic;
using System.Windows.Controls;
using NexusLibrarySystem.Models;
using NexusLibrarySystem.Data;

namespace NexusLibrarySystem.Views
{
    public partial class ProfilePage : Page
    {
        private User _currentUser;

        public ProfilePage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadUserData();
        }

        private void LoadUserData()
        {
            // Actualizar estados y bloqueo antes de cargar
            LoanData.UpdateLoanStatusesAndBlockUsers();

            // Cargar préstamos del usuario
            List<Loan> loans = LoanData.GetLoansByUserId(_currentUser.userId);
            LoansDataGrid.ItemsSource = loans;

            // Bindear otros datos
            this.DataContext = _currentUser;
        }
    }
}
