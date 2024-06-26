﻿using System.ComponentModel.DataAnnotations;

namespace MoviesFair.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your review")]
        public string? Comment { get; set; }

        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReviewDate { get; set; }

        [Required]
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        [Required]
        public string  UserId { get; set; }

    }
}
