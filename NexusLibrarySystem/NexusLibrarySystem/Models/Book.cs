public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public string PublicationYear { get; set; }  // o int, según tu SQL
    public int CategoryId { get; set; }
}