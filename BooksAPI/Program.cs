
using System.Text;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;


//add Serilog
Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
             .Enrich.FromLogContext()
             .Enrich.WithProperty("ApplicationName", "Books.API")
             .Enrich.WithProperty("MachineName", Environment.MachineName)
             .WriteTo.Console()
             .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

//log to console only in Development environment
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if (env == Environments.Development)
{
    builder.Host.UseSerilog(
       (context, loggerConfiguration) => loggerConfiguration
           .MinimumLevel.Debug()
           .WriteTo.Console());
}
else
{
    builder.Host.UseSerilog(
   (context, loggerConfiguration) => loggerConfiguration
       .MinimumLevel.Debug()
       .WriteTo.Console()
       .WriteTo.ApplicationInsights(
        new TelemetryConfiguration
        {
            InstrumentationKey = builder.Configuration["ApplicationInsightsInstrumentationKey"]
        },
        TelemetryConverter.Traces));
}


// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//used for initial development, before using EF core migrations and SQLite, code first approach
//builder.Services.AddSingleton<BooksDataStore>();

var sqlLiteConnString = builder.Configuration.GetConnectionString("BooksDBConnectionString");
//register the DB context
builder.Services.AddDbContext<BooksDbContext>(
    dbContextOptions => dbContextOptions.UseSqlite(sqlLiteConnString)
    );

//add forwarded header options - for deployment
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

//add JWT authentication
builder.Services.
AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretKey"]))
        };
    });


var app = builder.Build();

app.UseForwardedHeaders();

app.UseMiddleware<CustomExceptionHandlingMidleware>();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//apply and database pending migrations
using (var scope = app.Services.CreateScope())
{
    var databaseContext = scope.ServiceProvider.GetRequiredService<BooksDbContext>();

    if (databaseContext.Database.GetPendingMigrations().Any())
    {
        databaseContext.Database.Migrate();
    }
}

app.Run();
