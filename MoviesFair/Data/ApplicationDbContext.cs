using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoviesFair.Models;

namespace MoviesFair.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Genre>? Genres { get; set; }
        public DbSet<Category>? Category { get; set; }
        public DbSet<Movie>? Movies { get; set; }
        public DbSet<Review>? Reviews { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<MovieImages>? MovieImages { get; set; }

    }
}