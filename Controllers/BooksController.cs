
using AutoMapper;
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
    private readonly IMapper _mapper;

    public BookController(IBookService bookService, IMapper mapper)
    {
        _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));

        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
    {

        Log.Information("Getting all books");

        var booksFromStore = await _bookService.GetBooksAsync();

        //use AutoMapper to map between Book Entity and BookDto

        var bookDtos = _mapper.Map<IEnumerable<BookDto>>(booksFromStore);

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
    public async Task<ActionResult<BookDto>> GetBook(int id)
    {
        Log.Information("Getting book with id {id}", id);

        var bookFromStore = await _bookService.GetBookByIdAsync(id);

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
                Data = _mapper.Map<BookDto>(bookFromStore),
                IsSuccess = true,
                StatusCode = 200,
                Message = "Book retrieved successfully"
            };

            return Ok(bookResponseDto);
        }
    }

    // [HttpPost]
    // public ActionResult<BookDto> CreateBook(BookForCreationDto book)
    // {
    //     Log.Information("Creating book with title {title}", book.Title);

    //     var bookToAdd = new Book(book.Title,
    //                             book.Author,
    //                             book.Description ?? "description",
    //                             book.Genre ?? "fiction",
    //                             book.Year,
    //                             book.Pages);

    //     int maxId = _bookService.GetMaxId();
    //     bookToAdd.Id = maxId + 1;

    //     _bookService.AddBook(bookToAdd);

    //     var responseDto = new ResponseDto
    //     {
    //         Data = new BookDto
    //         {
    //             Id = bookToAdd.Id,
    //             Title = bookToAdd.Title,
    //             Author = bookToAdd.Author,
    //             Description = bookToAdd.Description,
    //             Year = bookToAdd.Year,
    //             Pages = bookToAdd.Pages,
    //             Genre = bookToAdd.Genre

    //         },
    //         IsSuccess = false,
    //         StatusCode = 404,
    //         Message = "Book not found"
    //     };

    //     return CreatedAtRoute("GetBook", new { id = bookToAdd.Id }, responseDto);
    // }

    // [HttpPut("{id}")]
    // public ActionResult<BookDto> UpdateBook(int id, BookForUpdateDto book)
    // {

    //     Log.Information("Updating book with id {id}", id);

    //     //check if book to update exists
    //     bool bookExists = _bookService.CheckIfBookExists(id);


    //     if (!bookExists)
    //     {
    //         //throw new ApplicationException($"Book with id {id} not found");
    //         return NotFound();
    //     }

    //     var bookToUpdate = new Book(book.Title, book.Author, book.Description, book.Genre, book.Year, book.Pages);
    //     bookToUpdate.Id = id;

    //     _bookService.UpdateBook(id, bookToUpdate);

    //     var bookDto = new BookDto
    //     {
    //         Id = bookToUpdate.Id,
    //         Author = book.Author,
    //         Title = book.Title,
    //         Description = book.Description,
    //         Genre = book.Genre,
    //         Year = book.Year,
    //         Pages = book.Pages

    //     };

    //     var responseDto = new ResponseDto
    //     {
    //         Data = bookDto,
    //         IsSuccess = true,
    //         StatusCode = 200,
    //         Message = "Book updated successfully"
    //     };

    //     return Ok(responseDto);
    // }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBook(int id)
    {
        Log.Information("Deleting book with id {id}", id);

        //check if book to update exists
        bool bookExists = await _bookService.CheckIfBookExistsAsync(id);


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

        var bookInStore = await _bookService.GetBookByIdAsync(id);
        _bookService.DeleteBook(bookInStore);

        _bookService.SaveChangesAsync();

        return NoContent();

    }


}