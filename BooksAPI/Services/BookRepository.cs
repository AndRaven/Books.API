
using System.Reflection.Metadata.Ecma335;
using Books.API.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

public class BookRepository : IBookRepository
{
    private BooksDbContext _booksDbContext;
    public BookRepository(BooksDbContext booksDbContext)
    {
        _booksDbContext = booksDbContext ?? throw new ArgumentNullException(nameof(booksDbContext));
    }

    public void AddBook(Book book)
    {
        _booksDbContext.Books.Add(book);
    }

    public async Task<bool> CheckIfBookExistAsync(int bookId)
    {
        return await _booksDbContext.Books.AnyAsync(book => book.Id == bookId);
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return await _booksDbContext.Books.ToListAsync();
    }

    public async Task<Book> GetBookByIdAsync(int bookId)
    {
        return await _booksDbContext.Books.Where(book => book.Id == bookId).FirstOrDefaultAsync();
    }

    public void RemoveBook(Book book)
    {
        _booksDbContext.Books.Remove(book);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _booksDbContext.SaveChangesAsync() >= 0;
    }
}