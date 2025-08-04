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
                    SELECT l.LoanId, l.UserId, l.BookId, l.LoanDate, l.DueDate, l.ReturnDate, l.Status, l.FineAmount, l.renewed,
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
                            Renewed = (bool)reader["renewed"],
                            BookTitle = reader["BookTitle"].ToString(),
                            UserName = reader["UserName"].ToString()
                        });
                    }
                }
            }

            return loans;
        }

        public static bool RenewLoan(int loanId)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                // Comprobar estado y si ya fue renovado
                string selectQuery = @"SELECT status, renewed FROM Loans WHERE loanId = @loanId";

                using (var selectCmd = new SqlCommand(selectQuery, conn))
                {
                    selectCmd.Parameters.AddWithValue("@loanId", loanId);
                    using (var reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string status = reader.GetString(0);
                            bool renewed = reader.GetBoolean(1);

                            if (status != "OnLoan" || renewed)
                                return false;
                        }
                        else
                        {
                            return false; // préstamo no encontrado
                        }
                    }
                }

                // Actualizar préstamo: extender dueDate 7 días y marcar renovado
                string updateQuery = @"
                    UPDATE Loans 
                    SET dueDate = DATEADD(DAY, 7, dueDate),
                        renewed = 1
                    WHERE loanId = @loanId";

                using (var updateCmd = new SqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@loanId", loanId);
                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public static void UpdateLoanStatusesAndBlockUsers()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                // Actualiza préstamos vencidos sin devolución
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

                // Bloquea usuarios activos con multas activas
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

                // 1. Check inventory
                string checkInventory = "SELECT Inventory FROM Books WHERE BookId = @BookId";
                using (var checkCmd = new SqlCommand(checkInventory, conn))
                {
                    checkCmd.Parameters.AddWithValue("@BookId", bookId);
                    int inventory = (int)checkCmd.ExecuteScalar();

                    if (inventory <= 0)
                        return false; // Cannot loan, no copies available
                }

                // 2. Insert loan
                string insertLoan = @"
            INSERT INTO Loans (UserId, BookId, LoanDate, DueDate, Status, FineAmount, renewed)
            VALUES (@UserId, @BookId, GETDATE(), DATEADD(DAY, 7, GETDATE()), 'OnLoan', 0, 0)";

                using (var cmd = new SqlCommand(insertLoan, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@BookId", bookId);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows <= 0)
                        return false;
                }

                // 3. Decrease book inventory
                string updateInventory = @"
            UPDATE Books
            SET Inventory = Inventory - 1
            WHERE BookId = @BookId AND Inventory > 0";

                using (var updateCmd = new SqlCommand(updateInventory, conn))
                {
                    updateCmd.Parameters.AddWithValue("@BookId", bookId);
                    updateCmd.ExecuteNonQuery();
                }

                return true;
            }
        }
    }
   }
