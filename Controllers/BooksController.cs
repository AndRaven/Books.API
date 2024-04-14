
using Books.API.DataAccess.Entities;
using Books.API.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
// using Serilog;


[ApiController]
[Route("api/books")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<BookDto>> GetBooks()
    {

        Log.Information("Getting all books");
        var booksFromStore = _bookService.GetBooks();

        var bookDtos = new List<BookDto>();

        //convert books to bookDtos
        foreach(var book in booksFromStore)
        {
           bookDtos.Add( new BookDto 
           {
              Id = book.Id,
              Author = book.Author,
              Title = book.Title,
              Description = book.Description,
              Genre = book.Genre,
              Year = book.Year,
              Pages = book.Pages
           });
        }

        return Ok(bookDtos);

    }

    [HttpGet("{id}", Name = "GetBook")]
    public ActionResult<BookDto> GetBook(int id)
    {
       Log.Information("Getting book with id {id}", id);

       var bookFromStore = _bookService.GetBookById(id);

       if (bookFromStore == null)
       {
           return NotFound();
       }
       else
       {
           return Ok(new BookDto
           {
              Id = bookFromStore.Id, 
              Author = bookFromStore.Author,
              Title = bookFromStore.Title,
              Description = bookFromStore.Description,
              Genre = bookFromStore.Genre,
              Year = bookFromStore.Year,
              Pages = bookFromStore.Pages
           });
       }
    }

    [HttpPost]
    public ActionResult<BookDto> CreateBook(BookForCreationDto book)
    {
        Log.Information("Creating book with title {title}", book.Title);

        var bookToAdd = new Book(book.Title, 
                                book.Author, 
                                book.Description ?? "description", 
                                book.Genre ?? "ficbtion", 
                                book.Year, 
                                book.Pages);

        int maxId = _bookService.GetMaxId();
        bookToAdd.Id = maxId + 1;

        _bookService.AddBook(bookToAdd);

        return CreatedAtRoute("GetBook", new { id = bookToAdd.Id }, new BookDto
        {
            Id = bookToAdd.Id,
            Title = bookToAdd.Title,
            Author = bookToAdd.Author,
            Description = bookToAdd.Description,
            Year = bookToAdd.Year,
            Pages = bookToAdd.Pages,
            Genre = bookToAdd.Genre
        });
    }

    [HttpPut("{id}")]
    public ActionResult<BookDto> UpdateBook(int id, BookForUpdateDto book)
    {

        Log.Information("Updating book with id {id}", id);

        //check if book to update exists
        bool bookExists = _bookService.CheckIfBookExists(id);


        if (!bookExists)
        {
            //throw new ApplicationException($"Book with id {id} not found");
            return NotFound();
        }
        
        var bookToUpdate = new Book(book.Title, book.Author, book.Description, book.Genre, book.Year, book.Pages);
        bookToUpdate.Id = id;
        
        _bookService.UpdateBook(id, bookToUpdate);

        return Ok(new BookDto
        {
              Id = bookToUpdate.Id,
              Author = book.Author,
              Title = book.Title,
              Description = book.Description,
              Genre = book.Genre,
              Year = book.Year,
              Pages = book.Pages

        });
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteBook(int id)
    {
        Log.Information("Deleting book with id {id}", id);
 
         //check if book to update exists
        bool bookExists = _bookService.CheckIfBookExists(id);


        if (!bookExists)
        {
            return NotFound();
        }

        _bookService.DeleteBook(id);

        return NoContent();

    }


}