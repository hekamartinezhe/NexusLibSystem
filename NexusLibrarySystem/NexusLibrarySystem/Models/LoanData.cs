using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NexusLibrarySystem.Models;

namespace NexusLibrarySystem.Data
{
    public static class LoanData
    {
        public static List<Loan> GetLoansByUserId(int userId)
        {
            var loans = new List<Loan>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT l.LoanId, l.UserId, l.BookId, l.LoanDate, l.DueDate, l.ReturnDate, l.Status, l.FineAmount,
                           b.Title AS BookTitle, u.FullName AS UserName
                    FROM Loans l
                    INNER JOIN Books b ON l.BookId = b.BookId
                    INNER JOIN Users u ON l.UserId = u.userId
                    WHERE l.UserId = @UserId";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        loans.Add(new Loan
                        {
                            LoanId = (int)reader["LoanId"],
                            UserId = (int)reader["UserId"],
                            BookId = (int)reader["BookId"],
                            LoanDate = (DateTime)reader["LoanDate"],
                            DueDate = (DateTime)reader["DueDate"],
                            ReturnDate = reader["ReturnDate"] as DateTime?,
                            Status = reader["Status"].ToString(),
                            FineAmount = (decimal)reader["FineAmount"],
                            BookTitle = reader["BookTitle"].ToString(),
                            UserName = reader["UserName"].ToString()
                        });
                    }
                }
            }

            return loans;
        }

        public static void UpdateLoanStatusesAndBlockUsers()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                // 1. Actualiza solo préstamos aún activos que ya están vencidos y sin devolución
                string updateOverdue = @"
            UPDATE Loans
            SET Status = 'Overdue',
                FineAmount = DATEDIFF(DAY, DueDate, GETDATE()) * 10
            WHERE Status = 'OnLoan' 
              AND DueDate < GETDATE() 
              AND ReturnDate IS NULL";

                using (var cmd = new SqlCommand(updateOverdue, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // 2. Solo bloquea usuarios que estén activos y tengan multas activas
                string blockUsers = @"
            UPDATE Users
            SET IsActive = 0
            WHERE IsActive = 1 AND EXISTS (
                SELECT 1 FROM Loans
                WHERE Loans.userId = Users.userId
                  AND Loans.FineAmount > 0
                  AND Loans.ReturnDate IS NULL
            )";

                using (var cmd = new SqlCommand(blockUsers, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static bool RegisterLoan(int userId, int bookId)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string insertLoan = @"
                    INSERT INTO Loans (UserId, BookId, LoanDate, DueDate, Status, FineAmount)
                    VALUES (@UserId, @BookId, GETDATE(), DATEADD(DAY, 7, GETDATE()), 'OnLoan', 0)";

                using (var cmd = new SqlCommand(insertLoan, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@BookId", bookId);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }
    }
}
