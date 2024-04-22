using System.ComponentModel.DataAnnotations;

namespace MoviesFair.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Genre Name")]
        public string? GenreName { get; set; }

    }
}
