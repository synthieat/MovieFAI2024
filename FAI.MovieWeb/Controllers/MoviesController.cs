﻿using FAI.Core.Application.DTOs.Movies;
using FAI.Core.Application.Services;
using FAI.Core.Entities.Movies;
using FAI.MovieWeb.Models;
using FAI.Persistence.Repositories.DBContext;
using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FAI.MovieWeb.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieDbContext _context;
        private readonly IMovieService movieService;

        public MoviesController(MovieDbContext context, IMovieService movieService)
        {
            _context = context;
            this.movieService = movieService;
        }

        // GET: Movies
        public async Task<IActionResult> Index([FromQuery] string? searchText,
                                               int? genreId,
                                               string? mediumTypeCode,
                                               int take = 10,
                                               int skip = 0,
                                               CancellationToken cancellationToken = default)
        {
            var movieDtos = await this.movieService.GetMovieDtos(searchText: searchText,
                                                                 genreId: genreId,                                                                    
                                                                 mediumTypeCd: mediumTypeCode,
                                                                 take: take,
                                                                 skip: skip, cancellationToken);
           
            return View(movieDtos);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details([FromRoute]Guid id, CancellationToken cancellation)
        {
            var movieDto = await this.movieService.GetMovieDtoById(id, cancellation);

            if (movieDto == null)
            {
                return NotFound();
            }

            ViewBag.Title = nameof(MoviesController.Details);

            return View(movieDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var movieModel = new MovieEditModel();

            var movieDto = await this.movieService.CreateMovieDto(cancellationToken);
            movieModel.MovieDto = movieDto;

            await this.InitMovieEditModel(movieModel, cancellationToken);

            /* Version ohne Model
            var genres = await this.movieService.GetGenres(cancellationToken);
            var mediumTypes = await this.movieService.GetMediumTypes(cancellationToken);

             ViewData["Genres"] = new SelectList(genres, "Id", "Name");
             ViewData["MediumTypes"] = new SelectList(mediumTypes, "Code", "Name");

            return View(movieDto);
            */

            return View(movieModel);
        }

        private async Task InitMovieEditModel(MovieEditModel movieModel, CancellationToken cancellationToken)
        {
            var genres = await this.movieService.GetGenres(cancellationToken);
            var mediumTypes = await this.movieService.GetMediumTypes(cancellationToken);

            movieModel.Genres = new SelectList(genres, "Id", "Name");
            movieModel.MediumTypes = new SelectList(mediumTypes, "Code", "Name");
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Title,Price,ReleaseDate,GenreId,MediumTypeCode")] Movie movie)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        movie.Id = Guid.NewGuid();
        //        _context.Add(movie);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
        //    ViewData["MediumTypeCode"] = new SelectList(_context.MediumTypes, "Code", "Code", movie.MediumTypeCode);
        //    return View(movie);
        //}

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
            ViewData["MediumTypeCode"] = new SelectList(_context.MediumTypes, "Code", "Code", movie.MediumTypeCode);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]Guid id, MovieDto movieDto, CancellationToken cancellationToken)
        {
            movieDto.Id = id;

            if(movieDto == null)
            {
                return NotFound();
            }
           
          
            if (ModelState.IsValid)
            {
                try
                {
                    var updMovieDto = await this.movieService.UpdateMovieDto(movieDto, CancellationToken.None);
                  
                }
                catch 
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var movieModel = new MovieEditModel();
            movieModel.MovieDto = movieDto;

            await this.InitMovieEditModel(movieModel, cancellationToken);

            return View(movieModel);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Genre)
                .Include(m => m.MediumType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(Guid id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
