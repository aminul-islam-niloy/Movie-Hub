using System.ComponentModel.DataAnnotations;

namespace MoviesFair.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        public string? CategoryName { get; set; }


    }
}
