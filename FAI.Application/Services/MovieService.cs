using FAI.Core.Application.DTOs.Movie;
using FAI.Core.Application.Services;
using FAI.Core.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Application.Services
{
    public class MovieService : IMovieService
    {
        public Task<MovieDto> CreateMovie(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMovie(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Genre>> GetGenres(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MediumType>> GetMediumTypes(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<MovieDto> GetMovieDtoById(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MovieDto>> GetMovieDtos(string? searchText, int? genreId, string? mediumTypeCd, int take = 10, int skip = 0, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<MovieDto> UpdateMovie(MovieDto movieDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
