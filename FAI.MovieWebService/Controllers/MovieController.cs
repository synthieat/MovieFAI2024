using FAI.Core.Application.DTOs.Movies;
using FAI.Core.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace FAI.MovieWebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : Controller
    {
        private IMovieService movieService;
        protected const string ID_PARAMETER_NAME = "/{Id}";

        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        [HttpGet(nameof(MovieDto))]
        public async Task<IEnumerable<MovieDto>> GetMovieDtos([FromQuery] string? SearchText,
                                                                          int? GenreId, 
                                                                          string? MediumTypeCd,
                                                                          int Take = 10,
                                                                          int Skip = 0,
                                                                          CancellationToken cancellationToken = default)
                                                                          
        {
            return await this.movieService.GetMovieDtos(searchText: SearchText, 
                                                        genreId: GenreId, 
                                                        mediumTypeCd: MediumTypeCd, 
                                                        take: Take, 
                                                        skip: Skip, cancellationToken);
        }

        [HttpGet(nameof(MovieDto) + ID_PARAMETER_NAME)]
        public async Task<MovieDto> GetMovieDto([FromRoute] Guid Id, CancellationToken cancellationToken)
        {
            return await this.movieService.GetMovieDtoById(Id, cancellationToken);

        }

        [HttpPost(nameof(MovieDto))]
        [ProducesResponseType(typeof(MovieDto), StatusCodes.Status201Created)]
        public async Task<MovieDto> CreateMovieDto(CancellationToken cancellation)
        {
            var result = await this.movieService.CreateMovieDto(cancellationToken: cancellation);
            return result;
        }
    }
}
