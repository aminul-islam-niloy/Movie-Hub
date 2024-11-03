using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesFair.Models
{
    public class Favorite
    {
        public int Id { get; set; }

        [Required]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }

        // Foreign key for the ApplicationUser 
        [Required]
        public string UserId { get; set; }

        //[ForeignKey("UserId")]
        //public ApplicationUser ApplicationUser { get; set; }
    }
}
