using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace NexusLibrarySystem.Views
{
    public partial class BooksPage : Page
    {
        private List<Book> mockBooks;

        public BooksPage()
        {
            InitializeComponent();

            // Datos simulados (sin base de datos)
            mockBooks = new List<Book>
            {
                new Book { Id = 1, Title = "Cien años de soledad", Author = "Gabriel García Márquez" },
                new Book { Id = 2, Title = "Don Quijote de la Mancha", Author = "Miguel de Cervantes" },
                new Book { Id = 3, Title = "El Principito", Author = "Antoine de Saint-Exupéry" },
                new Book { Id = 4, Title = "La sombra del viento", Author = "Carlos Ruiz Zafón" }
            };

            BooksGrid.ItemsSource = mockBooks;
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();

            var resultado = mockBooks
                .Where(b => b.Title.ToLower().Contains(query))
                .ToList();

            BooksGrid.ItemsSource = resultado;
        }
    }

    // Clase Book temporal (puedes moverla a Models/Book.cs si ya existe)
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
