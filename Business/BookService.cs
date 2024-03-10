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
}