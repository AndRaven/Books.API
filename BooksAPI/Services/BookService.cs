using Microsoft.AspNetCore.Authorization.Infrastructure;

using Books.API.DataAccess.Entities;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
    }

    public void AddBook(Book book)
    {
        //check to see if book with same title or author already exists
        _bookRepository.AddBook(book);
    }

    public async Task<Book?> GetBookByIdAsync(int bookId)
    {
        return await _bookRepository.GetBookByIdAsync(bookId);
    }


    public async Task<IEnumerable<Book>> GetBooksAsync(string? genre, int? year, string? searchQuery, int pageNumber, int pageSize)
    {
        //we can add business logic here
        //but pagination and search capabilities should be in the repository, as close to the data as possible
        //we don;t want to load all books at once and then apply filtering
        return await _bookRepository.GetAllBooksAsync(genre, year, searchQuery, pageNumber, pageSize);
    }

    public void DeleteBook(Book book)
    {
        _bookRepository.RemoveBook(book);
    }

    public async Task<bool> CheckIfBookExistsAsync(int bookId)
    {
        return await _bookRepository.CheckIfBookExistAsync(bookId);
    }

    public async Task SaveChangesAsync()
    {
        await _bookRepository.SaveChangesAsync();
    }
}