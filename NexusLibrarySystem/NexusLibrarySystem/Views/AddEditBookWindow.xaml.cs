using System;
using System.Collections.Generic;
using System.Windows;
using NexusLibrarySystem.Models;

namespace NexusLibrarySystem.Views
{
    public partial class AddEditBookWindow : Window
    {
        public Book Book { get; private set; }
        private readonly bool isEditMode;

        public AddEditBookWindow(Book book = null)
        {
            InitializeComponent();
            LoadCategories();

            if (book != null)
            {
                isEditMode = true;
                TitleLabel.Text = "Edit Book";
                Book = book;

                TitleBox.Text = book.Title;
                AuthorBox.Text = book.Author;
                PublisherBox.Text = book.Publisher;
                YearBox.Text = book.PublicationYear;
                ISBNBox.Text = book.ISBN;
                InventoryBox.Text = book.Inventory.ToString();
                CategoryComboBox.SelectedValue = book.CategoryId;
            }
            else
            {
                isEditMode = false;
                TitleLabel.Text = "Add Book";
                Book = new Book();
            }
        }

        private void LoadCategories()
        {
            try
            {
                List<KeyValuePair<int, string>> categories = BookData.GetCategories();
                CategoryComboBox.ItemsSource = categories;
                CategoryComboBox.DisplayMemberPath = "Value";
                CategoryComboBox.SelectedValuePath = "Key";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load categories: " + ex.Message);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text) || string.IsNullOrWhiteSpace(AuthorBox.Text))
            {
                MessageBox.Show("Please fill in the title and author.");
                return;
            }

            if (CategoryComboBox.SelectedValue == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            if (!int.TryParse(InventoryBox.Text, out int inventory) || inventory < 0)
            {
                MessageBox.Show("Inventory must be a non-negative number.");
                return;
            }

            Book.Title = TitleBox.Text.Trim();
            Book.Author = AuthorBox.Text.Trim();
            Book.Publisher = PublisherBox.Text.Trim();
            Book.PublicationYear = YearBox.Text.Trim();
            Book.ISBN = ISBNBox.Text.Trim();
            Book.Inventory = inventory;
            Book.CategoryId = (int)CategoryComboBox.SelectedValue;

            bool success = false;

            try
            {
                if (isEditMode)
                {
                    success = BookData.UpdateBook(Book);
                }
                else
                {
                    success = BookData.AddBook(Book);
                }

                if (success)
                {
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Failed to save book to the database.", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the book:\n" + ex.Message, "Exception");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
