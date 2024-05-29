using Books.API.DataAccess.Entities;

public interface IBookService
{
    void DeleteBook(Book book);

    void AddBook(Book book);

    Task<bool> CheckIfBookExistsAsync(int bookId);

    Task<IEnumerable<Book>> GetBooksAsync();

    Task<Book?> GetBookByIdAsync(int bookId);

    Task SaveChangesAsync();
}