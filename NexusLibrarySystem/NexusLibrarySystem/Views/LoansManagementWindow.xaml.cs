using System.Collections.Generic;
using System.Linq;
using System.Windows;
using NexusLibrarySystem.Data;
using NexusLibrarySystem.Models;

namespace NexusLibrarySystem.Views
{
    public partial class LoansManagementWindow : Window
    {
        private readonly User _user;
        private List<Loan> _loans;

        public LoansManagementWindow(User user)
        {
            InitializeComponent();
            _user = user;
            TxtUserInfo.Text = $"Préstamos del usuario: {_user.FullName} ({_user.EnrollmentNum})";
            LoadLoans();
        }

        private void LoadLoans()
        {
            try
            {
                _loans = LoanData.GetLoansByUserId(_user.UserId);
                LoansDataGrid.ItemsSource = _loans;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error al cargar los préstamos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReturnBook_Click(object sender, RoutedEventArgs e)
        {
            if (LoansDataGrid.SelectedItem is Loan selectedLoan)
            {
                if (selectedLoan.ReturnDate != null)
                {
                    MessageBox.Show("Este libro ya fue devuelto.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var result = MessageBox.Show($"¿Marcar libro '{selectedLoan.BookTitle}' como devuelto?", "Confirmar devolución", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    bool success = LoanData.MarkLoanReturned(selectedLoan.LoanId);
                    if (success)
                    {
                        MessageBox.Show("Libro marcado como devuelto.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadLoans();
                    }
                    else
                    {
                        MessageBox.Show("Error al devolver el libro.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona un préstamo.", "Devolver libro", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RenewLoan_Click(object sender, RoutedEventArgs e)
        {
            if (LoansDataGrid.SelectedItem is Loan selectedLoan)
            {
                if (selectedLoan.Renewed)
                {
                    MessageBox.Show("Este préstamo ya fue renovado.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (selectedLoan.ReturnDate != null)
                {
                    MessageBox.Show("No puedes renovar un préstamo devuelto.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var result = MessageBox.Show($"¿Renovar préstamo del libro '{selectedLoan.BookTitle}' por 7 días más?", "Confirmar renovación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    bool success = LoanData.RenewLoan(selectedLoan.LoanId);
                    if (success)
                    {
                        MessageBox.Show("Préstamo renovado.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadLoans();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo renovar el préstamo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona un préstamo para renovar.", "Renovar préstamo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
