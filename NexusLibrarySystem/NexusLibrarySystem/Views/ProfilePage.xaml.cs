using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using NexusLibrarySystem.Models;
using NexusLibrarySystem.Data;

namespace NexusLibrarySystem.Views
{
    public partial class ProfilePage : Page
    {
        private User _currentUser;
        private List<Loan> _loans;

        public ProfilePage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadUserData();
        }

        private void LoadUserData()
        {
            LoanData.UpdateLoanStatusesAndBlockUsers();

            _loans = LoanData.GetLoansByUserId(_currentUser.UserId);
            LoansDataGrid.ItemsSource = _loans;

            this.DataContext = _currentUser;

            RenewButton.IsEnabled = false;
        }

        private void LoansDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedLoan = LoansDataGrid.SelectedItem as Loan;

            if (selectedLoan != null && CanRenew(selectedLoan))
            {
                RenewButton.IsEnabled = true;
            }
            else
            {
                RenewButton.IsEnabled = false;
            }
        }

        private bool CanRenew(Loan loan)
        {
            // Solo se puede renovar si está 'OnLoan' y no ha sido renovado antes
            return loan.Status == "OnLoan" && !loan.Renewed;
        }

        private void RenewButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedLoan = LoansDataGrid.SelectedItem as Loan;
            if (selectedLoan == null)
                return;

            var result = MessageBox.Show("¿Deseas renovar este préstamo por 7 días más?", "Confirmar renovación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
                return;

            bool success = LoanData.RenewLoan(selectedLoan.LoanId);

            if (success)
            {
                MessageBox.Show("Préstamo renovado con éxito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadUserData(); // Recarga la lista y desactiva botón
            }
            else
            {
                MessageBox.Show("No se pudo renovar el préstamo. Puede que ya haya sido renovado o esté en estado inválido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
