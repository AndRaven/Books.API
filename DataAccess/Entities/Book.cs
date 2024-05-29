using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Books.API.DataAccess.Entities
{
    public class Book
    {
        public Book(string title, string author)
        {
            Title = title;
            Author = author;
        }

        public Book(string title, string author, string description, string genre, int year, int pages)
        {
            Title = title;
            Author = author;
            Description = description;
            Genre = genre;
            Year = year;
            Pages = pages;

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Author { get; set; } = String.Empty;
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? Genre { get; set; }
        public int Year { get; set; }
        public string? Language { get; set; }
        public int Pages { get; set; }
    }
}