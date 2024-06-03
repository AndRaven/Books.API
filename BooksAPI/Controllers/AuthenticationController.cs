
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

public class AuthenticateRequestBody
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class User
{
    public User(Guid userId, string username, string password)
    {
        UserId = userId;
        Username = username;
        Password = password;
    }

    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

}

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{

    private readonly IConfiguration _configuration;
    public AuthenticationController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("authenticate")]
    public IActionResult Authenticate(AuthenticateRequestBody authenticateRequestBody)
    {
        var user = ValidateUser(authenticateRequestBody.Username, authenticateRequestBody.Password);

        if (user == null)
        {
            return BadRequest(new { message = "Username or password is incorrect" });
        }

        //generate a token
        //first create the signing credentials
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretKey"]));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // var jwtToken = new JwtSecurityToken(
        //     issuer: _configuration["Authentication:Issuer"],
        //     audience: _configuration["Authentication:Audience"],
        //     expires: DateTime.Now.AddHours(1),
        //     signingCredentials: signingCredentials
        // );

        var token = new JwtSecurityToken(
            _configuration["Authentication:Issuer"],
            audience: _configuration["Authentication:Audience"],
            claims: null,
            notBefore: null,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: signingCredentials
        );

        //write the token to the JWT token handler to generate the token string
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(tokenString);
    }

    private User ValidateUser(string username, string password)
    {
        //users would be fetched from a database and we can check the username and password against the database
        //for simplicity, we are hardcoding the user
        //validate user
        return new User(Guid.NewGuid(), "admin", "admin");
    }

}