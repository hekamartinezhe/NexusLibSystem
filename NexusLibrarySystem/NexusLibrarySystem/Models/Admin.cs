namespace NexusLibrarySystem.Models
{
    public class Admin : User
    {
        public Admin()
        {
            Role = "Admin";
        }

        public override string GetDisplayInfo()
        {
            return $"{FullName} (Administrator)";
        }

        public void ReactivateUser(User user)
        {
            user.IsActive = true;
        }

        public void ChangeUserRole(User user, string newRole)
        {
            if (newRole == "Admin" || newRole == "Student")
                user.Role = newRole;
        }
    }
}
