

using Books.API.DataAccess.Entities;

public class BooksDataStore
{
    public List<Book> Books { get; }
    public BooksDataStore()
    {
        Books = new List<Book>()
        {
            new Book("First Book", "John Doe")
            {
                Id = 1,
                Description = "This is a test book",
                Genre = "fiction",
                Year = 1960
            },
            new Book("Second Book", "John Doe")
            {
                Id = 2,
                Description = "This is a test book",
                Genre = "non-fiction",
                Year = 2000
            }
        };
    }
}