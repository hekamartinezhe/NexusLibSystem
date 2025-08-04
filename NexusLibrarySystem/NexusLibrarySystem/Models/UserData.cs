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
            var users = new List<User>();

            using (var conn = Database.GetConnection())
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

        public static bool AddUser(User user, string plainPassword)
        {
            string hashedPassword = Database.HashPassword(plainPassword);

            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO Users (fullName, userRole, enrollmentNum, pswdHash, isActive)
                                 VALUES (@fullName, @role, @enrollmentNum, @pswdHash, @isActive)";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@fullName", user.FullName);
                    cmd.Parameters.AddWithValue("@role", user.Role);
                    cmd.Parameters.AddWithValue("@enrollmentNum", user.EnrollmentNum);
                    cmd.Parameters.AddWithValue("@pswdHash", hashedPassword);
                    cmd.Parameters.AddWithValue("@isActive", user.IsActive);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public static bool UpdateUser(User user)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = @"UPDATE Users
                                 SET fullName = @fullName,
                                     userRole = @role,
                                     enrollmentNum = @enrollmentNum,
                                     isActive = @isActive
                                 WHERE userId = @userId";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@fullName", user.FullName);
                    cmd.Parameters.AddWithValue("@role", user.Role);
                    cmd.Parameters.AddWithValue("@enrollmentNum", user.EnrollmentNum);
                    cmd.Parameters.AddWithValue("@isActive", user.IsActive);
                    cmd.Parameters.AddWithValue("@userId", user.UserId);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public static bool DeleteUser(int userId)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Users WHERE userId = @userId";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public static bool UserExists(string enrollmentNum)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE enrollmentNum = @enrollment";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@enrollment", enrollmentNum);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}

