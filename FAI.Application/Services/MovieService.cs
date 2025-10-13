using FAI.Core.Application.DTOs.Movies;
using FAI.Core.Application.Services;
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
                                            .Include(i => i.MediumType)
                                            .Select(s => MovieDto.MapFrom(s));

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
      
        public Task<IEnumerable<Genre>> GetGenres(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MediumType>> GetMediumTypes(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region COMMAND methods (Create, Update, Delete)

        public Task<MovieDto> CreateMovie(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMovie(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        public Task<MovieDto> UpdateMovie(MovieDto movieDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
