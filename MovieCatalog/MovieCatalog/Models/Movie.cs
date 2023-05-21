using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title length cannot be longer than 100.")]
        public string Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Genre length cannot be longer than 100.")]
        public string Genre { get; set; }

        [Required]
        [Range(1800, 2100, ErrorMessage = "The year must be a number between 1800-2100.")]
        public int Year { get; set; }

        public Movie(string title, string genre, int year)
        {
            Title = title;
            Genre = genre;
            Year = year;
        }
    }

}
