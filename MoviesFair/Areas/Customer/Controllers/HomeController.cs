using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MoviesFair.Data;
using MoviesFair.Models;
using MoviesFair.Models.View_Model;
using X.PagedList;

namespace MoviesFair.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IMemoryCache cache)
        {
            _logger = logger;
            _context = context;
            _cache = cache;
        }


        public IActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 12;

            // Check if the page of movies is already cached
            if (!_cache.TryGetValue($"MoviesPage_{pageNumber}_{pageSize}", out IPagedList<Movie> cachedMovies))
            {
                // Movies page is not in cache, so retrieve it from the database
                var moviesQuery = _context.Movies
                    .Include(c => c.Genre)
                    .Include(c => c.Category)
                    .OrderByDescending(m => m.Id) // Sort by descending order of ID
                    .AsQueryable();

                // Paginate the movies
                var movies = moviesQuery.ToPagedList(pageNumber, pageSize);

                // Cache the movies page
                _cache.Set($"MoviesPage_{pageNumber}_{pageSize}", movies, TimeSpan.FromMinutes(5));

                cachedMovies = movies;
            }

            // Pass genre list to the view
            ViewData["GenreTypeSearchId"] = new SelectList(_context.Genres.ToList(), "Id", "GenreName");

            // Initialize view model
            var viewModel = new IndexPageViewModel
            {
                Movies = cachedMovies,
                Action = GetCachedMoviesByCategory("Action"),
                Horror = GetCachedMoviesByCategory("Horror"),
                Romantic = GetCachedMoviesByCategory("Romantic"),
                Animation = GetCachedMoviesByCategory("Animation"),
                Sci_Fi = GetCachedMoviesByCategory("Sci-Fi"),
                War = GetCachedMoviesByCategory("War"),
                Advanture = GetCachedMoviesByCategory("Adventure"),
                History = GetCachedMoviesByCategory("History"),
                Comedy = GetCachedMoviesByCategory("Comedy")
            };

            return View(viewModel);
        }


        private IEnumerable<Movie> GetCachedMoviesByCategory(string category)
        {
            if (!_cache.TryGetValue(category, out IEnumerable<Movie> cachedMovies))
            {
                // Data is not in cache, so retrieve it from the database
                cachedMovies = _context.Movies
                    .Where(m => m.Genre.GenreName == category)
                    .Include(m => m.Genre)
                    .Include(m => m.Category)
                    .OrderByDescending(m => m.Id)
                    .ToList();

                // Cache the data for 5 minutes
                _cache.Set(category, cachedMovies, TimeSpan.FromMinutes(5));
            }

            return cachedMovies;
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

            return RedirectToAction("MoviesPage", new { searchString, genreId });
        }


        public IActionResult MoviesPage(string searchString, int genreId = 0)
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

            return View("MoviesPage", movies);
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



        public IActionResult MoviesByGenre(string genreName)
        {
            var movies = _context.Movies
                                  .Include(m => m.Genre)
                                  .Include(m => m.Category)
                                  .Where(m => m.Genre.GenreName == genreName)
                                  .ToList();
            ViewData["Title"] = $"Movies by Genre: {genreName}";
            return View("MoviesByGenre", movies);
        }


        public IActionResult MoviesByCategory(string categoryName)
        {
            var movies = _context.Movies
                                  .Include(m => m.Genre)
                                  .Include(m => m.Category)
                                  .Where(m => m.Category.CategoryName == categoryName)
                                  .ToList();
            ViewData["Title"] = $"Movies by Category: {categoryName}";
            return View("MoviesByGenre", movies); // Reusing the same view as MoviesByGenre
        }

    }
}
