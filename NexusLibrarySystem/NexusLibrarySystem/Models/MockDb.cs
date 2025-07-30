// Models/MockDatabase.cs
namespace NexusLibrarySystem.Models
{
    public class MockDb
    {
        public static User FindUser(string enrollment)
        {
            if (enrollment == "admin")
                return new User { EnrollmentNumber = "admin", Password = "123456", Role = "Admin" };

            if (enrollment == "student001")
                return new User { EnrollmentNumber = "student", Password = "123456", Role = "Student" };

            return null;
        }
    }
}