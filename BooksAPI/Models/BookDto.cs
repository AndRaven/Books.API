namespace Books.API.Models;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Author { get; set; } = String.Empty;
    public string? Description { get; set; } 
    public string? Url { get; set; }
    public string? Genre { get; set; }
    public int Year { get; set; }
    public string? Language { get; set; }
    public int Pages { get; set; }
}