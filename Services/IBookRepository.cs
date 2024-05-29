using Books.API.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllBooksAsync();

    void AddBook(Book book);

    void RemoveBook(Book book);

    Task<Book> GetBookByIdAsync(int bookId);

    Task<bool> SaveChangesAsync();

    Task<bool> CheckIfBookExistAsync(int bookId);


}