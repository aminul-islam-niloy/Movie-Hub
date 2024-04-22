using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MoviesFair.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        [Display(Name = "Genre")]
        [Required]
        public int GenreId { get; set; }

        [ForeignKey("GenreId")]
        public virtual Genre? Genre { get; set; }

        [Display(Name = "Category")]
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        [Required]
        public string? Image { get; set; }

        public string? Description { get; set; }
        public string? Language { get; set; }

        public int Year { get; set; }
        public ICollection<MovieImages>? MovieImages { get; set; }
        public ICollection<Review>? Reviews { get; set; }

    }
}
