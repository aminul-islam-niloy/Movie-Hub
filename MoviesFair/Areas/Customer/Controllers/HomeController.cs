using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesFair.Data;
using MoviesFair.Models.View_Model;
using X.PagedList;

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

        public IActionResult Index(int? page)
        {
            var moviesQuery = _context.Movies.Include(c => c.Genre).Include(c => c.Category).AsQueryable();

            var movies = moviesQuery.ToList().ToPagedList(page ?? 1, 4);

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

        public IActionResult Details(int? id)
        {
            ViewData["GenreTypeSearchId"] = new SelectList(_context.Genres.ToList(), "Id", "GenreName");

            if (id == null)
            {
                return NotFound();
            }

            var Specificmovie = _context.Movies
                .Include(c => c.Genre)
                .Include(c => c.Category)
                .FirstOrDefault(m => m.Id == id);

            if (Specificmovie == null)
            {
                return NotFound();
            }

            var relatedMovies = _context.Movies
               .Where(p => p.GenreId == Specificmovie.GenreId && p.Id != Specificmovie.Id)
               .Take(12) // Assuming  want to display 12 related products
               .ToList();

            var viewModel = new DetaislViewModel
            {
                SpecificMovies = Specificmovie,
                RelatedMovies = relatedMovies
            };


            return View(viewModel);
        }

    }
}
