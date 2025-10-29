using FAI.Common;
using FAI.Core.Application.DTOs.Movies;
using FAI.Core.Application.Services;
using FAI.Core.Attributes;
using FAI.Core.Entities.Movies;
using FAI.Core.Repositories.Movies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Application.Services
{
    [MapServiceDependency(nameof(MovieService))]
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository movieRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IMediumTypeRepository mediumTypeRepository;
         
        public MovieService(IMovieRepository movieRepository, 
                            IGenreRepository genreRepository, 
                            IMediumTypeRepository mediumTypeRepository)
        {
            this.movieRepository = movieRepository;
            this.genreRepository = genreRepository;
            this.mediumTypeRepository = mediumTypeRepository;
        }


        #region READ methods

        public async Task<IEnumerable<MovieDto>> GetMovieDtos(string? searchText, 
                                                        int? genreId, 
                                                        string? mediumTypeCd, 
                                                        int take = 10,
                                                        int skip = 0, CancellationToken cancellationToken = default)
        {
            var query = this.movieRepository.QueryFrom<Movie>()
                                            .Include(i => i.Genre)
                                            .Include(i => i.MediumType).AsQueryable();
                                            

            if(!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(w => w.Title.Contains(searchText));
            }

            if (genreId.HasValue)
            {
                query = query.Where(w => w.GenreId == genreId.Value);
            }

            if(!string.IsNullOrWhiteSpace(mediumTypeCd))
            {
                query = query.Where(w => w.MediumTypeCode == mediumTypeCd);
            }

            return await query.Skip(skip) /* Pagination Skip / Take, Achtung: immer zuerst Skip, dann Take */ 
                              .Take(take)
                              .Select(s => MovieDto.MapFrom(s))
                              .ToListAsync(cancellationToken);
        }


        public async Task<MovieDto> GetMovieDtoById(Guid id, CancellationToken cancellationToken = default)
        {
            var query = this.movieRepository.QueryFrom<Movie>(m => m.Id == id)
                                            .Include(i => i.Genre)
                                            .Include(i => i.MediumType)
                                            .Select(s => MovieDto.MapFrom(s));

            return await query.SingleOrDefaultAsync(cancellationToken);
        }
      
        public async Task<IEnumerable<Genre>> GetGenres(CancellationToken cancellationToken = default)
        {
            return await this.genreRepository.QueryFrom<Genre>()
                                             .AsNoTracking()
                                             .OrderBy(o => o.Name)
                                             .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MediumType>> GetMediumTypes(CancellationToken cancellationToken = default)
        {
            return await this.mediumTypeRepository.QueryFrom<MediumType>()
                                                  .AsNoTracking()
                                                  .OrderBy(o => o.Name)
                                                  .ToListAsync(cancellationToken);
        }


        #endregion

        #region COMMAND methods (Create, Update, Delete)

        public async Task<MovieDto> CreateMovieDto(CancellationToken cancellationToken)
        {
            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "n/a",
                Price = 0M,
                ReleaseDate = DateTime.Now.Date,
                GenreId = 1, // Default Genre
                MediumTypeCode = "BD" // Default MediumType
            };

            /* Variante 1: Neues Movie Dummy Object wird in Db gespeichert */
            await this.movieRepository.AddAsync(movie, saveImmediately: false, cancellationToken);

            await this.movieRepository.SaveAsync(cancellationToken);
            /* Gespeicherte Dummy in DTO mappen und zurückgeben */
            return MovieDto.MapFrom(movie)!;
        }

        public async Task<MovieDto> UpdateMovieDto(MovieDto movieDto, CancellationToken cancellationToken)
        {
            /* Movie Entität erzeugen */
            var movie = new Movie();

            /* Werte vom DTO in die Entität mappen */
            Helpers.MapEntityProperties(movieDto, movie, null);

            /* Entität mit eingefügten Werten speichern. Die Id kann aus movie oder movieDto gelesen werden. */
            var updatedMovie = await this.movieRepository.UpdateAsync(movie, movieDto.Id, saveImmediately: true, cancellationToken);

            // await this.movieRepository.SaveAsync(cancellationToken); <= wenn saveImmediately: true ist, dann nicht mehr nötig

            /* Aktualisiertes DTO zurück geben */
            return MovieDto.MapFrom(updatedMovie)!;
        }

        public async Task DeleteMovie(Guid id, CancellationToken cancellationToken)
        {
            await this.movieRepository.RemoveByKeyAsync<Movie>(id, saveImmediately: true, cancellationToken);
        }



        #endregion
    }
}
