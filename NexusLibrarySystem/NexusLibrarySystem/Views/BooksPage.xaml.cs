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
        public BooksPage()
        {
            InitializeComponent();
            CargarLibros(); // Carga todos los libros al iniciar
        }

        private void CargarLibros(string filtro = "")
        {
            try
            {
                List<Book> libros = Database.GetBooks(filtro);
                BooksGrid.ItemsSource = libros;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar libros: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            string filtro = SearchBox.Text.Trim();
            CargarLibros(filtro);
        }
    }
}
