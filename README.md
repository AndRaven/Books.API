# Books API Overview

Books.API is a RESTful API for managing books (retrieving book related data, updating book information or adding books) built with NET Core 6.0 and Entity Framework Core 6.0.

The API is intended to be part of a network of APIs that will be used to provide the back-end functionality for a book challenge tracking web application.

# Books API Endpoints

| Endpoint        | Method | Description                                                                                                                          |
| :-------------- | :----: | :----------------------------------------------------------------------------------------------------------------------------------- |
| /api/books      |  GET   | returns JSON structure with all books                                                                                                |
| /api/books/{id} |  GET   | Returns a JSON response with a single book based on {id}                                                                             |
| api/books       |  POST  | Creates a new book based on payload provided in the request body                                                                     |
| api/books/{id}  |  PUT   | Updates a point of interest based on {pointOfInterestId} and {cityId} and returns a JSON response with the updated point of interest |

# Books API Design considerations

Database: in the 1st iteration, the API leverages a SQLite database and EF Core 6. Future iterations will use a cloud based database offering (Azure SQL)

Logging is done with Serilog.

API documentation leverages Swagger Open API.

Patterns used:

- Repository Pattern - abstraction layer on top of the database access layer and database context; increases testability and separates business logic from data access logic
- Dependency Injection

Endpoints responses have been wrapped up in a generic  _ResponseDto_  that will contain the BookDtos as part of a  _Data_ field. This approcah will provide consistency for API clients.

# Securing the API

The API endpoints are secured at the Application level with JWT Bearer Token.

For now, the JWT token can be obtained by calling the /api/authentication/authenticate endpoint on the same API.

In future API development iterations, the token will be obtained from a separate Identity API which leverages Micrososft Identity to handle users' registration and login.

# Running the API and deployment

The API can be run on a local machine or in a Docker container.

GitHub actions have been configured for CI/CD pipeline and the API gets deployed to Azure AppService.
