using Books.API.DataAccess.Entities;

public interface IBookService
{
    Book CreateBook(string title, string author, string description, string genre, int year, int pages);

    void UpdateBook(int bookId, Book book);

    void DeleteBook(int bookId);

    void AddBook(Book book);

    bool CheckIfBookExists(int bookId);

    IEnumerable<Book> GetBooks();

    Book? GetBookById(int bookId);

    int GetMaxId();
}