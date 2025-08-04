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

        public virtual string GetDisplayInfo()
        {
            return $"{FullName} ({Role})";
        }
    }
}
