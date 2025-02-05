﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesFair.Data;
using MoviesFair.Models;

namespace MoviesFair.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MoviesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Movies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Movies.Include(m => m.Category).Include(m => m.Genre);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Admin/Movies/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName");
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
     

        public async Task<IActionResult> Create(Movie movie, List<IFormFile> MovieImages)
        {
           
                var searchmovie = _context.Movies.FirstOrDefault(c => c.Name == movie.Name);
                if (searchmovie != null)
                {
                    ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName");
                    ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName");

                    return View(movie);
                }

                if (MovieImages != null && MovieImages.Count > 0)
                {
                    movie.MovieImages = new List<MovieImages>(); // Initialize the collection

                    foreach (var image in MovieImages)
                    {
                        try
                        {
                            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath + "/Images", Path.GetFileName(image.FileName));

                            // Ensure the directory exists
                            Directory.CreateDirectory(Path.GetDirectoryName(imagePath));

                            using (var fileStream = new FileStream(imagePath, FileMode.Create))
                            {
                                await image.CopyToAsync(fileStream);
                            }

                            // Add the image path to the movie's images collection
                            movie.MovieImages.Add(new MovieImages { ImagePath = "Images/" + image.FileName });
                        }
                        catch (Exception ex)
                        {
                         
                            Console.WriteLine($"Error copying image: {ex.Message}");
                        }
                    }
                }
                else
                {
                    movie.MovieImages = new List<MovieImages>(); // Ensure collection is initialized
                }

                if (movie.MovieImages != null && movie.MovieImages.Any())
                {
                    // Set the Image property to the path of the first image
                    movie.Image = movie.MovieImages.First().ImagePath;
                }

                // Add the movie to the database
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();

                TempData["save"] = "movie has been added";
                return RedirectToAction(nameof(Index));
            

        }




        // GET: Admin/Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName");
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName");
            return View(movie);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Movie movie, List<IFormFile> MovieImages)
        {

            var searchmovie = _context.Movies.FirstOrDefault(c => c.Name == movie.Name);
           

            if (MovieImages != null && MovieImages.Count > 0)
            {
                movie.MovieImages = new List<MovieImages>(); // Initialize the collection

                foreach (var image in MovieImages)
                {
                    try
                    {
                        var imagePath = Path.Combine(_webHostEnvironment.WebRootPath + "/Images", Path.GetFileName(image.FileName));

                        // Ensure the directory exists
                        Directory.CreateDirectory(Path.GetDirectoryName(imagePath));

                        using (var fileStream = new FileStream(imagePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        // Add the image path to the movie's images collection
                        movie.MovieImages.Add(new MovieImages { ImagePath = "Images/" + image.FileName });
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine($"Error copying image: {ex.Message}");
                    }
                }
            }
            else
            {
                movie.MovieImages = new List<MovieImages>(); // Ensure collection is initialized
            }

            if (movie.MovieImages != null && movie.MovieImages.Any())
            {
                movie.Image = movie.MovieImages.First().ImagePath;
            }

            // Add the movie to the database
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();

            TempData["save"] = "movie has been Edited";
            return RedirectToAction(nameof(Index));


        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _context.Movies.Include(c => c.Genre).Include(c => c.Category).Where(c => c.Id == id).FirstOrDefault();
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }


        // POST: Admin/Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName");
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName");

            if (_context.Movies == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Movies'  is null.");
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
          return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
