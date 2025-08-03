using System;

namespace NexusLibrarySystem.Models
{
    public class Loan
    {
        public int LoanId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }

        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public string Status { get; set; } // "Active", "Returned", "Overdue"

        public decimal FineAmount { get; set; } // Adeudo acumulado

        // Propiedades para mostrar en la UI
        public string BookTitle { get; set; }
        public string UserName { get; set; }
    }
}
