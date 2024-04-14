using Microsoft.AspNetCore.Authorization.Infrastructure;

using Books.API.DataAccess.Entities;

public class BookService : IBookService
{
    private readonly BooksDataStore _booksDataStore;

    public BookService(BooksDataStore booksDataStore)
    {
        _booksDataStore = booksDataStore ?? throw new ArgumentNullException(nameof(booksDataStore));
    } 

    public Book CreateBook(string title, string author, string description, string genre, int year, int pages)
    {
        //add logic to check that book title and author does not already exist in the database
        //if it does, throw an exception

        return new Book(title, author, description, genre, year, pages);
    }



    public void AddBook(Book book)
    {
        _booksDataStore.Books.Add(book);
    }

    public Book? GetBookById(int bookId)
    {
        return _booksDataStore.Books.FirstOrDefault(b => b.Id == bookId);
    }

    public bool CheckIfBookExists(int bookId)
    {
       return _booksDataStore.Books.Any( b => b.Id == bookId );
    }

    public IEnumerable<Book> GetBooks()
    {
        return _booksDataStore.Books;
    }

    public void UpdateBook(int bookId, Book book)
    {
        var bookFound = GetBookById(bookId);

        if (bookFound != null)
        {
            bookFound.Title = book.Title;
            bookFound.Author = book.Author;
            bookFound.Description = book.Description;
            bookFound.Genre = book.Genre;
            bookFound.Year = book.Year;
            bookFound.Pages = book.Pages;
        }


    }

    public void DeleteBook(int bookId)
    {
        var bookFound = GetBookById(bookId);

        if (bookFound != null)
            _booksDataStore.Books.Remove(bookFound);
    }

    public int GetMaxId()
    {
        return _booksDataStore.Books.Max(b => b.Id);
    }
}