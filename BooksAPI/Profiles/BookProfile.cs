using AutoMapper;
using Books.API.DataAccess.Entities;
using Books.API.Models;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<BookDto, Book>();
        CreateMap<BookForUpdateDto, Book>();
        CreateMap<BookForCreationDto, Book>();
    }
}