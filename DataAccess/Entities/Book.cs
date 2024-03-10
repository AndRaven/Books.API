namespace Books.API.DataAccess.Entities
{
    public class Book
    {
        public Book(string title, string author)
        {
            Title = title;
            Author = author;
        }

        public Book(string title, string author, string description, string genre, int year, int pages)
        {
            Title = title;
            Author = author;
            Description = description;
            Genre = genre;
            Year = year;
            Pages = pages;
           
        }
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? Genre { get; set; }
        public int Year { get; set; }
        public string? Language { get; set; }
        public int? Pages { get; set; }
    }
}