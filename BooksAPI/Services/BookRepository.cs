
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

    public async Task<IEnumerable<Book>> GetAllBooksAsync(string? genre, int? year, string? searchQuery)
    {
        //first apply filter by genre or year and then apply search

        IQueryable<Book> books = _booksDbContext.Books as IQueryable<Book>;

        if (!string.IsNullOrEmpty(genre) && !string.IsNullOrEmpty(year.ToString()))
        {
            return await books.Where(book => book.Genre == genre && book.Year == year).ToListAsync();
        }
        else
          if (!string.IsNullOrEmpty(genre))
        {
            return await books.Where(book => book.Genre == genre).ToListAsync();
        }
        else
            if (!string.IsNullOrEmpty(year.ToString()))
        {
            return await books.Where(book => book.Year == year).ToListAsync();
        }

        //apply case-insensitive search
        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.Trim();
            searchQuery = searchQuery.ToUpper();

            return await books.Where(
            book => book.Title.ToUpper().Contains(searchQuery) ||
            book.Author.ToUpper().Contains(searchQuery) ||
            (!string.IsNullOrEmpty(book.Description) && book.Description.ToUpper().Contains(searchQuery))).
            ToListAsync();
        }

        return await books.ToListAsync();

    }

    public async Task<Book?> GetBookByIdAsync(int bookId)
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

    private void MoreWaysToGetDataWithLinq()
    {
        var bookId = 10;
        //get a Book by ID

        Book? book = _booksDbContext.Books.FirstOrDefault(book => book.Id == bookId);

    }
}