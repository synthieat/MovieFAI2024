using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Core.Entities.Movies
{
    public class Movie : MovieBase, IEntity
    {
        public virtual Genre Genre { get; set; }

        [ForeignKey(nameof(MediumTypeCode))]
        public virtual MediumType MediumType { get; set; }
              

    }
}
