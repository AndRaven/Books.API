
using AutoMapper;
using Books.API.DataAccess.Entities;
using Books.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
// using Serilog;

[Authorize]
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
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBooks(string? genre, int? year, string? searchQuery)
    {

        Log.Information("Getting all books");

        var booksFromStore = await _bookService.GetBooksAsync(genre, year, searchQuery);

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
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBook(int id)
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

    [HttpPost]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBook(BookForCreationDto book)
    {
        Log.Information("Creating book with title {title}", book.Title);

        var bookToAdd = _mapper.Map<Book>(book);

        _bookService.AddBook(bookToAdd);

        await _bookService.SaveChangesAsync();

        var bookDtoToReturn = _mapper.Map<BookDto>(bookToAdd);

        var responseDto = new ResponseDto
        {
            Data = bookDtoToReturn,
            IsSuccess = false,
            StatusCode = 200,
            Message = "Book created successfully"
        };

        return CreatedAtRoute("GetBook", new { id = bookToAdd.Id }, responseDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateBook(int id, BookForUpdateDto book)
    {

        Log.Information("Updating book with id {id}", id);

        //get the Book that needs updated from the repository based on Id

        Book bookToUpdate = await _bookService.GetBookByIdAsync(id);

        if (bookToUpdate == null)
        {
            return NotFound();
        }

        //generate the updated Book entity from BookForUpdateDto
        _mapper.Map(book, bookToUpdate);

        await _bookService.SaveChangesAsync();

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