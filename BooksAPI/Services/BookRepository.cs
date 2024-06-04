
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

    public async Task<IEnumerable<Book>> GetAllBooksAsync(string? genre, int? year, string? searchQuery, int pageNumber, int pageSize)
    {
        //but pagination and search capabilities should be in the repository, as close to the data as possible
        //we don't want to load all books at once and then apply filtering
        //we only apply the iterator ToListAsync() at the end of the method which triggers the query to be executed

        IQueryable<Book> books = _booksDbContext.Books as IQueryable<Book>;

        if (!string.IsNullOrEmpty(genre) && !string.IsNullOrEmpty(year.ToString()))
        {
            books = books.Where(book => book.Genre == genre && book.Year == year);
        }
        else
          if (!string.IsNullOrEmpty(genre))
        {
            books = books.Where(book => book.Genre == genre);
        }
        else
            if (!string.IsNullOrEmpty(year.ToString()))
        {
            books = books.Where(book => book.Year == year);
        }

        //apply case-insensitive search
        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.Trim();
            searchQuery = searchQuery.ToUpper();

            books = books.Where(
            book => book.Title.ToUpper().Contains(searchQuery) ||
            book.Author.ToUpper().Contains(searchQuery) ||
            (!string.IsNullOrEmpty(book.Description) && book.Description.ToUpper().Contains(searchQuery)));
        }

        //add pagination
        var booksToReturn = await books.
                                    OrderBy(book => book.Title).
                                    Skip((pageNumber - 1) * pageSize).
                                    Take(pageSize).
                                    ToListAsync();
        //return all books
        return booksToReturn;
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