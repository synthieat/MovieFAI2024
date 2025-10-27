﻿using FAI.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Core.Entities.Movies
{
    public class MovieBase
    {
        public virtual Guid Id { get; set; }

        [MaxLength(128), MinLength(1)]
        [Required]
        [Display(Name = nameof(Title), ResourceType = typeof(BasicRes))]

        public virtual string Title { get; set; } = string.Empty;

        // [Required]
        [Display(Name = nameof(Price), ResourceType = typeof(BasicRes))]
        public virtual decimal Price { get; set; }

        [Display(Name = nameof(ReleaseDate), ResourceType = typeof(BasicRes))]
        public virtual DateTime ReleaseDate { get; set; }

        [Display(Name = "Genre", ResourceType = typeof(BasicRes))]
        public virtual int GenreId { get; set; }

        [Display(Name = "MediumType", ResourceType = typeof(BasicRes))]
        public virtual string? MediumTypeCode { get; set; }

    }
}
