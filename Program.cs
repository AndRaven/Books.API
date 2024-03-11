// using Serilog;
// using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

//add Serilog

// var logger = new LoggerConfiguration()
//              .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
//              .Enrich.FromLogContext()
//              .Enrich.WithProperty("ApplicationName", "Books.API")
//              .Enrich.WithProperty("MachineName", Environment.MachineName)
//              .WriteTo.Console()
//              .CreateLogger();

// builder.Logging.ClearProviders();
// builder.Logging.AddSerilog(logger);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddSingleton<BooksDataStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
