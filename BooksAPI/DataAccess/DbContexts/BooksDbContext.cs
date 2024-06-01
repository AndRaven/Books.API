using System.Data;
using Books.API.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

public class BooksDbContext : DbContext
{
    public BooksDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasData(
            new Book("The Night Circus",
                    "Erin Morgenstern",
                    "Written in rich, seductive prose, this spell-casting novel is a feast for the senses and the heart.",
                    "fiction", 2011, 506)
            {
                Id = 1,
                Language = "English"
            },
              new Book("Cloud Cuckoo Land",
                    "Anthony Doer",
                    "When everything is lost, it’s our stories that survive.",
                    "fiction", 2021, 626)
              {
                  Id = 2,
                  Language = "English"
              }
        );
    }


}