using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NexusLibrarySystem.Models;

namespace NexusLibrarySystem.Views
{
    public partial class BooksPage : Page
    {
        private readonly User _currentUser;

        public BooksPage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadBooks();
            AdjustUIByRole();
        }

        private void LoadBooks(string filter = "")
        {
            try
            {
                List<Book> books = BookData.GetBooks(filter);
                BooksGrid.ItemsSource = books;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading books: " + ex.Message);
            }
        }

        private void AdjustUIByRole()
        {
            if (_currentUser.Role.ToLower() == "student")
            {
                LoanButton.Visibility = Visibility.Visible;
            }
            else if (_currentUser.Role.ToLower() == "admin")
            {
                AddBookButton.Visibility = Visibility.Visible;
                EditBookButton.Visibility = Visibility.Visible;
                DeleteBookButton.Visibility = Visibility.Visible;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text.Trim();
            LoadBooks(query);
        }

        private void LoanButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is Book selectedBook)
            {
                MessageBox.Show($"Book '{selectedBook.Title}' requested successfully.", "Loan Book");
                // Aquí puedes registrar en la base de datos un préstamo si deseas
            }
            else
            {
                MessageBox.Show("Please select a book to loan.", "No Book Selected");
            }
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Open form to add new book.");
            // Lógica real de agregar libro aquí
        }

        private void EditBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is Book selectedBook)
            {
                MessageBox.Show($"Editing book: {selectedBook.Title}");
                // Lógica real de edición aquí
            }
            else
            {
                MessageBox.Show("Select a book to edit.");
            }
        }

        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is Book selectedBook)
            {
                var confirm = MessageBox.Show($"Are you sure you want to delete '{selectedBook.Title}'?",
                                              "Confirm Deletion", MessageBoxButton.YesNo);

                if (confirm == MessageBoxResult.Yes)
                {
                    // Aquí deberías eliminarlo de la base de datos
                    MessageBox.Show("Book deleted (mock).");
                    LoadBooks();
                }
            }
            else
            {
                MessageBox.Show("Select a book to delete.");
            }
        }
    }
}
