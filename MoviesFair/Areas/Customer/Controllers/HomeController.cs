using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MoviesFair.Data;
using MoviesFair.Models;
using MoviesFair.Models.View_Model;
using System.Security.Claims;
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

          

            var movieReviews = _context.Reviews
                .Where(r => r.MovieId == id)
                .Select(r => new MovieReviewViewModel
                {

                    UserId = r.UserId,

                    UserName = _context.Users.FirstOrDefault(u => u.Id == r.UserId).UserName,
                     Comment = r.Comment,
                    Rating = r.Rating
                })
                .ToList();

            var viewModel = new DetaislViewModel
            {
                SpecificMovies = Specificmovie,
                RelatedMovies = relatedMovies,
                MovieReviews = movieReviews
            };


            return View(viewModel);
        }


        private string GetUserNameById(string userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            return user != null ? user.UserName : string.Empty;
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

        [Authorize]
        [HttpPost]
        public IActionResult AddToFavorites(int movieId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to login page or show an error message
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favorite = new Favorite { MovieId = movieId, UserId = userId };

            _context.Favorites.Add(favorite);
            _context.SaveChanges();

            // Redirect to the same page or a confirmation page
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult FavoriteMovies()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName");
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName");
          
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favoriteMovies = _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.Movie)
                .ToList();

            return View(favoriteMovies);
        }

        [Authorize]
        [HttpPost]
        public IActionResult RemoveFromFavorites(int movieId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favoriteToRemove = _context.Favorites.FirstOrDefault(f => f.UserId == userId && f.MovieId == movieId);
            if (favoriteToRemove != null)
            {
                _context.Favorites.Remove(favoriteToRemove);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [Authorize]
        public IActionResult CreateReview(int movieId)
        {
            ViewBag.MovieId = movieId; // Pass the movie ID to the view
            return View();
        }


        [HttpPost]
        [Authorize]
        public IActionResult CreateReview(Review review)
        {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                review.UserId = userId;
                review.ReviewDate = DateTime.Now;



                _context.Reviews.Add(review);
                _context.SaveChanges();

                // Update movie's total rating and rating count
                var movie = _context.Movies.Find(review.MovieId);
                if (movie != null)
                {
                    // Calculate new overall rating and update total reviews
                    var newTotalReviews = movie.TotalReviews + 1;
                    var newOverallRating = (movie.OverallRating * movie.TotalReviews + review.Rating) / newTotalReviews;

                    movie.TotalReviews = newTotalReviews;
                    movie.OverallRating = newOverallRating;

                    _context.SaveChanges();
                }

                return RedirectToAction("Details", new { id = review.MovieId });
          
        }



    }
}
