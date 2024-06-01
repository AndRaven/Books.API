using System.ComponentModel.DataAnnotations;

public class BookForUpdateDto
{
    [Required(ErrorMessage = "Please provide a book title")]
    [MaxLength(100)]
    public string Title { get; set; } = String.Empty;

    [Required(ErrorMessage = "Please provide a book author")]
    [MaxLength(100)]
    public string Author { get; set; } = String.Empty;
    public string? Description { get; set; } 
    public string? Url { get; set; }
    public string? Genre { get; set; }

    [Required(ErrorMessage = "Please provide a year")]
    [Range(1800, 2024)]
    public int Year { get; set; }
    public string? Language { get; set; }


    [Required(ErrorMessage = "Please provide number of pages")]
    [Range(1, 10000)]
    public int Pages { get; set; }
}