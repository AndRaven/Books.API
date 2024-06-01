using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BooksAPI.Tests;

[TestFixture]
public class BooksControllerTests
{
    private BookController _bookController;

    private Mock<IBookService> _bookServiceMock;
    private Mock<IBookRepository> _bookRepositoryMock;

    private Mock<IMapper> _mapperMock;

    [SetUp]
    public void Setup()
    {
        _bookServiceMock = new Mock<IBookService>();
        _mapperMock = new Mock<IMapper>();
        _bookRepositoryMock = new Mock<IBookRepository>();

        _bookController = new BookController(_bookServiceMock.Object, _mapperMock.Object);
    }

    [Test]
    public void CheckBookDoesNotExist()
    {
        _bookServiceMock.Setup(service => service.CheckIfBookExistsAsync(10)).ReturnsAsync(false);

        var result = _bookController.GetBook(10);

        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
    }
}