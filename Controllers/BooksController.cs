
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
        foreach (var book in booksFromStore)
        {
            bookDtos.Add(new BookDto
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

        var booksResponseDto = new ResponseDto
        {
            Data = bookDtos,
            IsSuccess = true,
            StatusCode = 200,
            Message = "Books retrieved successfully"
        };

        return Ok(booksResponseDto);

    }

    [HttpGet("{id}", Name = "GetBook")]
    public ActionResult<BookDto> GetBook(int id)
    {
        Log.Information("Getting book with id {id}", id);

        var bookFromStore = _bookService.GetBookById(id);

        if (bookFromStore == null)
        {
            var bookResponseDto = new ResponseDto
            {
                Data = null,
                IsSuccess = false,
                StatusCode = 404,
                Message = "Book not found"

            };

            return NotFound(bookResponseDto);
        }
        else
        {
            var bookResponseDto = new ResponseDto
            {
                Data = new BookDto
                {
                    Id = bookFromStore.Id,
                    Author = bookFromStore.Author,
                    Title = bookFromStore.Title,
                    Description = bookFromStore.Description,
                    Genre = bookFromStore.Genre,
                    Year = bookFromStore.Year,
                    Pages = bookFromStore.Pages
                },
                IsSuccess = true,
                StatusCode = 200,
                Message = "Book retrieved successfully"
            };

            return Ok(bookResponseDto);
        }
    }

    [HttpPost]
    public ActionResult<BookDto> CreateBook(BookForCreationDto book)
    {
        Log.Information("Creating book with title {title}", book.Title);

        var bookToAdd = new Book(book.Title,
                                book.Author,
                                book.Description ?? "description",
                                book.Genre ?? "fiction",
                                book.Year,
                                book.Pages);

        int maxId = _bookService.GetMaxId();
        bookToAdd.Id = maxId + 1;

        _bookService.AddBook(bookToAdd);

        var responseDto = new ResponseDto
        {
            Data = new BookDto
            {
                Id = bookToAdd.Id,
                Title = bookToAdd.Title,
                Author = bookToAdd.Author,
                Description = bookToAdd.Description,
                Year = bookToAdd.Year,
                Pages = bookToAdd.Pages,
                Genre = bookToAdd.Genre

            },
            IsSuccess = false,
            StatusCode = 404,
            Message = "Book not found"
        };

        return CreatedAtRoute("GetBook", new { id = bookToAdd.Id }, responseDto);
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

        var bookDto = new BookDto
        {
            Id = bookToUpdate.Id,
            Author = book.Author,
            Title = book.Title,
            Description = book.Description,
            Genre = book.Genre,
            Year = book.Year,
            Pages = book.Pages

        };

        var responseDto = new ResponseDto
        {
            Data = bookDto,
            IsSuccess = true,
            StatusCode = 200,
            Message = "Book updated successfully"
        };

        return Ok(responseDto);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteBook(int id)
    {
        Log.Information("Deleting book with id {id}", id);

        //check if book to update exists
        bool bookExists = _bookService.CheckIfBookExists(id);


        if (!bookExists)
        {

            var responseDto = new ResponseDto
            {
                Data = null,
                IsSuccess = false,
                StatusCode = 404,
                Message = "Book not found"
            };


            return NotFound(responseDto);
        }

        _bookService.DeleteBook(id);


        return NoContent();

    }


}