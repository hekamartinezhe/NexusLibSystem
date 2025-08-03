
public class User
{
    public int UserId { get; set; }                // userId
    public string FullName { get; set; }       // fullName
    public string EnrollmentNum { get; set; }  // enrollmentNum (no se usa aquí, pero podrías agregarlo)
    public string Role { get; set; }           // userRole
    public bool IsActive { get; set; } = true;

}