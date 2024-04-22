using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesFair.Controllers;
using MoviesFair.Data;
using MoviesFair.Models;

namespace MoviesFair.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GenreController : Controller
    {
        private ApplicationDbContext _db;
        private readonly ILogger<GenreController> _logger;
        public GenreController(ApplicationDbContext context, ILogger<GenreController> logger)
        {
            _db = context;
            _logger = logger;
        }

        public IActionResult Test()
        {
            return View();
        }

        public IActionResult Index()
        {
            var data = _db.Genres.ToList();
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Genre GenreTypes)
        {
            if (ModelState.IsValid)
            {
                _db.Genres.Add(GenreTypes);
                await _db.SaveChangesAsync();
                TempData["save"] = "Genre has been Added";
                return RedirectToAction(nameof(Index));
            }

            return View(GenreTypes);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.Genres == null)
            {
                return NotFound();
            }

            var genre = await _db.Genres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.Genres == null)
            {
                return NotFound();
            }

            var genre = await _db.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GenreName")] Genre genre)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(genre);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genre.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _db.Genres == null)
            {
                return NotFound();
            }

            var genre = await _db.Genres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.Genres == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Genres'  is null.");
            }
            var genre = await _db.Genres.FindAsync(id);
            if (genre != null)
            {
                _db.Genres.Remove(genre);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenreExists(int id)
        {
            return (_db.Genres?.Any(e => e.Id == id)).GetValueOrDefault();
        }


    }
}
