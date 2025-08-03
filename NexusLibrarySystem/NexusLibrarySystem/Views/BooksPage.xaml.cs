using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using NexusLibrarySystem.Models;
using NexusLibrarySystem.Data;

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
                AddBookButton.Visibility = Visibility.Collapsed;
                EditBookButton.Visibility = Visibility.Collapsed;
                DeleteBookButton.Visibility = Visibility.Collapsed;
            }
            else if (_currentUser.Role.ToLower() == "admin")
            {
                AddBookButton.Visibility = Visibility.Visible;
                EditBookButton.Visibility = Visibility.Visible;
                DeleteBookButton.Visibility = Visibility.Visible;
                LoanButton.Visibility = Visibility.Collapsed;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text.Trim();
            LoadBooks(query);
        }

        private void LoanButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_currentUser.IsActive)
            {
                MessageBox.Show("You cannot loan books until all your fines are paid.", "User Blocked");
                return;
            }

            if (BooksGrid.SelectedItem is Book selectedBook)
            {
                bool success = LoanData.RegisterLoan(_currentUser.userId, selectedBook.Id);
                if (success)
                {
                    MessageBox.Show($"Book '{selectedBook.Title}' loaned successfully.", "Loan Book");
                }
                else
                {
                    MessageBox.Show("Error registering the loan.", "Error");
                }
            }
            else
            {
                MessageBox.Show("Please select a book to loan.", "No Book Selected");
            }
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditBookWindow(null); // Add mode
            bool? result = addWindow.ShowDialog();

            if (result == true)
            {
                LoadBooks();
                MessageBox.Show("Book added successfully.", "Success");
            }
        }

        private void EditBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is Book selectedBook)
            {
                var editWindow = new AddEditBookWindow(selectedBook); // Edit mode
                bool? result = editWindow.ShowDialog();

                if (result == true)
                {
                    LoadBooks();
                    MessageBox.Show("Book updated successfully.", "Success");
                }
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
                var confirm = MessageBox.Show(
                    $"Are you sure you want to delete '{selectedBook.Title}'?",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (confirm == MessageBoxResult.Yes)
                {
                    bool success = BookData.DeleteBook(selectedBook.Id);
                    if (success)
                    {
                        LoadBooks();
                        MessageBox.Show("Book deleted successfully.", "Deleted");
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete the book.", "Error");
                    }
                }
            }
            else
            {
                MessageBox.Show("Select a book to delete.");
            }
        }
    }
}
