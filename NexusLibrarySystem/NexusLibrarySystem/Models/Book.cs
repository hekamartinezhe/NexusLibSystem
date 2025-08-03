public class Book
{
    public int Id { get; set; }
    public string ISBN { get; set; }           // Nuevo campo visible
    public string Title { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public string PublicationYear { get; set; }
    public int Inventory { get; set; }          // Nuevo campo
    public int CategoryId { get; set; }
}