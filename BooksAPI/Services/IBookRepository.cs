using Books.API.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllBooksAsync(string? genre, int? year, string? searchQuery, int pageNumber, int pageSize);

    void AddBook(Book book);

    void RemoveBook(Book book);

    Task<Book?> GetBookByIdAsync(int bookId);

    Task<bool> SaveChangesAsync();

    Task<bool> CheckIfBookExistAsync(int bookId);


}