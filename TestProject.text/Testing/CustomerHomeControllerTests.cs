using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesFair.Areas.Customer.Controllers;
using MoviesFair.Data;
using MoviesFair.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoviesFair.Areas;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.text.Testing
{
    public class CustomerHomeControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<ILogger<HomeController>> _loggerMock;
        private readonly Mock<IMemoryCache> _memoryCacheMock;

        public CustomerHomeControllerTests()
        {
            // Use InMemoryDatabase to mock a real database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            _loggerMock = new Mock<ILogger<HomeController>>();
            _memoryCacheMock = new Mock<IMemoryCache>();

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _context.Genres.Add(new Genre { GenreName = "Action" });
          
            _context.SaveChanges();
        }

        [Fact]
        public void MoviesByGenre_ReturnsViewResult_WithListOfMovies()
        {
            // Arrange
            var controller = new HomeController(_loggerMock.Object, _context, _memoryCacheMock.Object);  

            // Act
            var result = controller.MoviesByGenre("Action") as ViewResult;

            // Assert
            Assert.NotNull(result);
            var movies = Assert.IsAssignableFrom<List<Movie>>(result.Model);
            Assert.Equal(2, movies.Count); // Should return 2 movies
        }
    }
}
