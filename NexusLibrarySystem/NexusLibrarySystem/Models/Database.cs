using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using NexusLibrarySystem.Models;

namespace NexusLibrarySystem
{
    public static class Database
    {
        // Obtiene la cadena de conexión desde App.config
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["NexusDB"].ConnectionString;

        /// <summary>
        /// Valida el login del usuario comparando matrícula y contraseña hasheada con la base de datos.
        /// </summary>
        public static User ValidateLogin(string enrollment, string password)
        {
            string hashedPassword = HashPassword(password);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT userId, fullName, userRole FROM Users WHERE enrollementNum = @enrollment AND pswdHash = @password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@enrollment", enrollment);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Role = reader.GetString(2)
                            };
                        }
                    }
                }
            }

            return null; // Usuario no encontrado o contraseña incorrecta
        }

        /// <summary>
        /// Hashea una cadena de texto usando SHA-256.
        /// </summary>
        public static string HashPassword(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();

                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
