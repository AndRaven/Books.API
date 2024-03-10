
using Books.API.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;


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
}