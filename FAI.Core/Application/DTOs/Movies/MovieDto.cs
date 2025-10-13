using FAI.Core.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Core.Application.DTOs.Movies
{
    public class MovieDto : MovieBase
    {
        public virtual string GenreName { get; set; } = string.Empty;
        public virtual string? MediumTypeName { get; set; }

        public static MovieDto? MapFrom(Movie movie)
        {
            if(movie != null)
            {
                return new MovieDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Price = movie.Price,
                    ReleaseDate = movie.ReleaseDate,
                    GenreId = movie.GenreId,
                    MediumTypeCode = movie.MediumTypeCode,
                    GenreName = movie.Genre?.Name ?? string.Empty,
                    MediumTypeName = movie.MediumType?.Name
                };
            }

            return null;
        }
    }
}
