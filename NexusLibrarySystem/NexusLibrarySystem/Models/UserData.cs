using System.Data.SqlClient;
using NexusLibrarySystem.Models;

namespace NexusLibrarySystem.Data
{
    public static class UserData
    {
        public static User ValidateLogin(string enrollment, string password)
        {
            string hashedPassword = Database.HashPassword(password);

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string query = @"SELECT userId, fullName, userRole 
                                 FROM Users 
                                 WHERE enrollementNum = @enrollment AND pswdHash = @password";

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

            return null;
        }
    }
}

