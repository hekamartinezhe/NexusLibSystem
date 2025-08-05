using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using NexusLibrarySystem.Data;
using NexusLibrarySystem.Models;

namespace NexusLibrarySystem.Views
{
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
        }

        private void LoadAvailableBooksReport_Click(object sender, RoutedEventArgs e)
        {
            var data = BookData.GetAvailableBooks();
            ReportDataGrid.ItemsSource = data;
        }

        private void LoadMostBorrowedBooksReport_Click(object sender, RoutedEventArgs e)
        {
            var data = BookData.GetMostBorrowedBooks();
            ReportDataGrid.ItemsSource = data;
        }

        private void LoadFrequentUsersReport_Click(object sender, RoutedEventArgs e)
        {
            var data = LoanData.GetFrequentUsers();
            ReportDataGrid.ItemsSource = data;
        }
    }
}
