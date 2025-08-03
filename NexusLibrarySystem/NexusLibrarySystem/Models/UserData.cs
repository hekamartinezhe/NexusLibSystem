using System.Collections.Generic;
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
                string query = @"SELECT userId, fullName, userRole, enrollmentNum, isActive
                                 FROM Users 
                                 WHERE enrollmentNum = @enrollment AND pswdHash = @password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@enrollment", enrollment);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string role = reader.GetString(2).ToLower();

                            if (role == "admin")
                            {
                                return new Admin
                                {
                                    UserId = reader.GetInt32(0),
                                    FullName = reader.GetString(1),
                                    Role = "Admin",
                                    EnrollmentNum = reader.GetString(3),
                                    IsActive = reader.GetBoolean(4)
                                };
                            }
                            else
                            {
                                return new Student
                                {
                                    UserId = reader.GetInt32(0),
                                    FullName = reader.GetString(1),
                                    Role = "Student",
                                    EnrollmentNum = reader.GetString(3),
                                    IsActive = reader.GetBoolean(4)
                                };
                            }
                        }
                    }
                }
            }

            return null;
        }

        public static List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT userId, fullName, userRole, enrollmentNum, isActive FROM Users";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string role = reader.GetString(2).ToLower();

                        if (role == "admin")
                        {
                            users.Add(new Admin
                            {
                                UserId = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Role = "Admin",
                                EnrollmentNum = reader.GetString(3),
                                IsActive = reader.GetBoolean(4)
                            });
                        }
                        else
                        {
                            users.Add(new Student
                            {
                                UserId = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Role = "Student",
                                EnrollmentNum = reader.GetString(3),
                                IsActive = reader.GetBoolean(4)
                            });
                        }
                    }
                }
            }

            return users;
        }
    }
}
