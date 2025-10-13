using FAI.Core.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Core.Application.DTOs.Movie
{
    public class MovieDto : MovieBase
    {
        public virtual string GenreName { get; set; } = string.Empty;
        public virtual string? MediumTypeName { get; set; }
    }
}
