using NexusLibrarySystem.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NexusLibrarySystem.Models
{
    public static class BookData
    {
        public static List<Book> GetBooks(string titleFilter = "")
        {
            var books = new List<Book>();

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT bookId, title, author FROM Books";

                if (!string.IsNullOrWhiteSpace(titleFilter))
                    query += " WHERE title LIKE @title";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(titleFilter))
                        cmd.Parameters.AddWithValue("@title", $"%{titleFilter}%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(new Book
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Author = reader.GetString(2)
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
                string query = @"INSERT INTO Books (title, author, publisher, publicationYear, categoryId)
                                 VALUES (@title, @autor, @publisher, @year, @categoryId)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", book.Title);
                    cmd.Parameters.AddWithValue("@autor", book.Author);
                    cmd.Parameters.AddWithValue("@publisher", book.Publisher);
                    cmd.Parameters.AddWithValue("@year", book.PublicationYear);
                    cmd.Parameters.AddWithValue("@categoryId", book.CategoryId);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool UpdateBook(Book book)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string query = @"UPDATE Books
                                 SET title = @title,
                                     author = @autor,
                                     publisher = @publisher,
                                     publicationYear = @year,
                                     categoryId = @categoryId
                                 WHERE bookId = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", book.Id);
                    cmd.Parameters.AddWithValue("@title", book.Title);
                    cmd.Parameters.AddWithValue("@autor", book.Author);
                    cmd.Parameters.AddWithValue("@publisher", book.Publisher);
                    cmd.Parameters.AddWithValue("@year", book.PublicationYear);
                    cmd.Parameters.AddWithValue("@categoryId", book.CategoryId);

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
    }
}
