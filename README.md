Books.API is a RESTful API for managing books built with NET Core 6.0 and Entity Framework Core 6.0.

Database:

Logging is done with Serilog.

Enpoints provided are:

- retrieve all books : GET /api/books
- add a new book: POST /api/books
- update existing book: PUT /api/books/{id}
- delete existing book: DELETE /api/books/{id}

The API can be run on a local machine or in a Docker container.

API deployed to Azure AppService
