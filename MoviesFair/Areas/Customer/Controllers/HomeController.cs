using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesFair.Data;

namespace MoviesFair.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var moviesQuery = _context.Movies.Include(c => c.Genre).Include(c => c.Category).AsQueryable();

            var movies = moviesQuery.ToList();

            // Pass genre list to the view
            ViewData["GenreTypeSearchId"] = new SelectList(_context.Genres.ToList(), "Id", "GenreName");

            return View(movies);
        }



        [HttpPost]
        public IActionResult Index(string searchString, int genreId = 0)
        {
            var moviesQuery = _context.Movies.Include(c => c.Genre).Include(c => c.Category).AsQueryable();

            // Filter by genre if selected
            if (genreId != 0)
            {
                moviesQuery = moviesQuery.Where(m => m.GenreId == genreId);
            }

            // Filter by search string
            if (!string.IsNullOrEmpty(searchString))
            {
                moviesQuery = moviesQuery.Where(m => m.Name.Contains(searchString));
            }

            var movies = moviesQuery.ToList();

            // Pass genre list to the view
            ViewData["GenreTypeSearchId"] = new SelectList(_context.Genres.ToList(), "Id", "GenreName");

            return View(movies);
        }

    }
}
