using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesFair.Models
{
    public class Favorite
    {
        public int Id { get; set; }

        // Foreign key for the Movie
        [Required]
        public int MovieId { get; set; }

        // Navigation property to the Movie
        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }

        // Foreign key for the ApplicationUser (assuming you are using ASP.NET Core Identity)
        [Required]
        public string UserId { get; set; }

        // Navigation property to the ApplicationUser
        //[ForeignKey("UserId")]
        //public ApplicationUser ApplicationUser { get; set; }
    }
}
