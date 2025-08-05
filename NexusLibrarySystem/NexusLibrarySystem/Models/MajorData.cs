using NexusLibrarySystem.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

public static class MajorData
{
    public static List<Major> GetAllMajors()
    {
        var majors = new List<Major>();
        using (var conn = Database.GetConnection())
        {
            conn.Open();
            string query = "SELECT majorId, majorName FROM Major";
            using (var cmd = new SqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    majors.Add(new Major
                    {
                        MajorId = reader.GetInt32(0),
                        MajorName = reader.GetString(1)
                    });
                }
            }
        }
        return majors;
    }
}
