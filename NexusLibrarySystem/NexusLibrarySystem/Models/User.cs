namespace NexusLibrarySystem.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string EnrollmentNum { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? MajorId { get; set; } // Could be null if the user has no major
        public int? Quadrimester { get; set; } // Could be null if the user is not enrolled in a quadrimester
        public int LoanCount { get; set; }
        public virtual string GetDisplayInfo()
        {
            return $"{FullName} ({Role})";
        }
    }
}
