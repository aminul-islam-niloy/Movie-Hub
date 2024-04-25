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

            ViewData["GenreTypeSearchId"] = new SelectList(_context.Movies.ToList(), "Id", "Genre");

            return View(_context.Movies.Include(c => c.Genre).Include(c => c.Category).ToList());
        }



        [HttpPost]
        public IActionResult Index(string searchString)
        {
            var movies = _context.Movies.Include(c => c.Genre).Include(c => c.Category).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                // Filter products based on search string
                movies = movies.Where(p => p.Name.Contains(searchString)).ToList();
            }
            return View(movies);
        }
    }
    }
