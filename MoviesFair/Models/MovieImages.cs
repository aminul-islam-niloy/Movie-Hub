using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesFair.Models
{
    public class MovieImages
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movie? Movie { get; set; }
        public string? ImagePath { get; set; }

    }
}
