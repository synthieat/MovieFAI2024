using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Core.Entities.Movies
{
    // [Table(name: "XY")] Table Attribute, um den SQL-Tabellen Namen zu übersteuern
    public class MediumType
    {
        public MediumType() 
        { 
            this.Movies = new HashSet<Movie>();
        }

        [MaxLength(8), MinLength(2)]
        [Key]
        public virtual string Code { get; set; }

        [MaxLength(32),MinLength(2)]
        [Required]
        public virtual string Name { get; set; }

        public virtual ICollection<Movie> Movies { get; } //= new HashSet<Movie>();
    }
}
