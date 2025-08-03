using System.Collections.Generic;

namespace NexusLibrarySystem.Models
{
    public class Student : User
    {
        public List<Loan> ActiveLoans { get; set; }

        public Student()
        {
            Role = "Student";
            ActiveLoans = new List<Loan>();
        }

        public bool HasPendingFines()
        {
            return ActiveLoans?.Exists(l => l.FineAmount > 0 && l.Status != "Paid") == true;
        }

        public override string GetDisplayInfo()
        {
            return $"{FullName} (Student)";
        }
    }
}
