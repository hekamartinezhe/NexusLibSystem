using NexusLibrarySystem.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NexusLibrarySystem.Models
{
    public static class BookData
    {
        public static List<Book> GetBooks(string filter = "")
        {
            var books = new List<Book>();

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string query = @"SELECT bookId, isbn, title, author, publisher, publicationYear, inventory, categoryId
                                 FROM Books";

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    query += @" WHERE title LIKE @filter OR
                                     author LIKE @filter OR
                                     isbn LIKE @filter OR
                                     publisher LIKE @filter OR
                                     publicationYear LIKE @filter";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(filter))
                        cmd.Parameters.AddWithValue("@filter", $"%{filter}%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(new Book
                            {
                                Id = reader.GetInt32(0),
                                ISBN = reader.GetString(1),
                                Title = reader.GetString(2),
                                Author = reader.GetString(3),
                                Publisher = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                PublicationYear = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                Inventory = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                                CategoryId = reader.GetInt32(7)
                            });
                        }
                    }
                }
            }

            return books;
        }

        public static bool AddBook(Book book)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string query = @"INSERT INTO Books 
                                (isbn, title, author, publisher, publicationYear, categoryId, inventory)
                                 VALUES 
                                (@isbn, @title, @author,
                                 @publisher, @year, @categoryId, @inventory)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@isbn", book.ISBN);
                    cmd.Parameters.AddWithValue("@title", book.Title);
                    cmd.Parameters.AddWithValue("@author", book.Author);
                    cmd.Parameters.AddWithValue("@publisher", book.Publisher ?? "");
                    cmd.Parameters.AddWithValue("@year", book.PublicationYear ?? "");
                    cmd.Parameters.AddWithValue("@categoryId", book.CategoryId);
                    cmd.Parameters.AddWithValue("@inventory", book.Inventory);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool UpdateBook(Book book)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string query = @"UPDATE Books SET
                                    isbn = @isbn,
                                    title = @title,
                                    author = @author,
                                    publisher = @publisher,
                                    publicationYear = @year,
                                    categoryId = @categoryId,
                                    inventory = @inventory
                                 WHERE bookId = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", book.Id);
                    cmd.Parameters.AddWithValue("@isbn", book.ISBN);
                    cmd.Parameters.AddWithValue("@title", book.Title);
                    cmd.Parameters.AddWithValue("@author", book.Author);
                    cmd.Parameters.AddWithValue("@publisher", book.Publisher ?? "");
                    cmd.Parameters.AddWithValue("@year", book.PublicationYear ?? "");
                    cmd.Parameters.AddWithValue("@categoryId", book.CategoryId);
                    cmd.Parameters.AddWithValue("@inventory", book.Inventory);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool DeleteBook(int bookId)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string query = "DELETE FROM Books WHERE bookId = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", bookId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public static int GetAvailableBooksCount()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Books WHERE quantity > 0";
                using (var cmd = new SqlCommand(query, conn))
                {
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public static Book GetMostLoanedBook()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = @"
            SELECT TOP 1 b.bookId, b.title, b.author, COUNT(*) AS LoanCount
            FROM Loans l
            INNER JOIN Books b ON l.bookId = b.bookId
            GROUP BY b.bookId, b.title, b.author
            ORDER BY LoanCount DESC";

                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Book
                        {
                            BookId = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Author = reader.GetString(2),
                            LoanCount = reader.GetInt32(3)
                        };
                    }
                }
            }
            return null;
        }

        public static List<Book> GetTopLoanedBooks(int top)
        {
            var books = new List<Book>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = @"
            SELECT TOP (@top) b.bookId, b.title, b.author, COUNT(*) AS LoanCount
            FROM Loans l
            INNER JOIN Books b ON l.bookId = b.bookId
            GROUP BY b.bookId, b.title, b.author
            ORDER BY LoanCount DESC";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@top", top);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(new Book
                            {
                                BookId = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Author = reader.GetString(2),
                                LoanCount = reader.GetInt32(3)
                            });
                        }
                    }
                }
            }

            return books;
        }
        public static List<Book> GetAvailableBooks()
        {
            var books = new List<Book>();
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = @"SELECT bookId, isbn, title, author, publisher, publicationYear, inventory
                         FROM Books
                         WHERE inventory > 0";

                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            Id = reader.GetInt32(0),
                            ISBN = reader.GetString(1),
                            Title = reader.GetString(2),
                            Author = reader.GetString(3),
                            Publisher = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            PublicationYear = reader.IsDBNull(5) ? "" : reader.GetString(5),
                            Inventory = reader.IsDBNull(6) ? 0 : reader.GetInt32(6)
                        });
                    }
                }
            }
            return books;
        }

        public static List<Book> GetMostBorrowedBooks()
        {
            var books = new List<Book>();
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = @"
            SELECT TOP 5 b.bookId, b.title, b.author, COUNT(*) AS LoanCount
            FROM Loans l
            INNER JOIN Books b ON l.bookId = b.bookId
            GROUP BY b.bookId, b.title, b.author
            ORDER BY LoanCount DESC";

                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Author = reader.GetString(2),
                            LoanCount = reader.GetInt32(3)
                        });
                    }
                }
            }
            return books;
        }


        /// <summary>
        /// Obtiene todas las categorías (ID + nombre) desde la base de datos.
        /// </summary>
        public static List<KeyValuePair<int, string>> GetCategories()
        {
            var categories = new List<KeyValuePair<int, string>>();

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT categoryId, categoryName FROM Categories";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new KeyValuePair<int, string>(
                            reader.GetInt32(0),
                            reader.GetString(1)
                        ));
                    }
                }
            }

            return categories;
        }
    }
}
