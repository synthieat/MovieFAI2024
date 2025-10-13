using FAI.Core.Application.DTOs.Movie;
using FAI.Core.Entities.Movies;
using FAI.Core.Repositories.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Core.Application.Services
{
    public interface IMovieService
    {

        #region READ methods

        Task<IEnumerable<MovieDto>> GetMovieDtos(string? searchText, 
                                           int? genreId, 
                                           string? mediumTypeCd, 
                                           int take = 10, 
                                           int skip = 0,
                                           CancellationToken cancellationToken = default);

        Task<MovieDto> GetMovieDtoById(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Genre>> GetGenres(CancellationToken cancellationToken = default);

        Task<IEnumerable<MediumType>> GetMediumTypes(CancellationToken cancellationToken = default);


        #endregion

        #region COMMAND methods (Create, Update, Delete)

        Task<MovieDto> CreateMovie(CancellationToken cancellationToken);

        Task<MovieDto> UpdateMovie(MovieDto movieDto, CancellationToken cancellationToken);

        Task DeleteMovie(Guid id, CancellationToken cancellationToken);

        #endregion
    }
}
