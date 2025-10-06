using FAI.Core.Entities.Movies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Persistence.Repositories.DBContext
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext() { }

        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) 
        { 
            Database.SetCommandTimeout(90);
        }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<MediumType> MediumTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable(nameof(Movie)); /* Aktuell keinen Sinn, da der Tabellenname 
                                                * dem Klassennamen entspricht */
                entity.Property(p => p.Title)
                      .IsRequired()
                      .HasMaxLength(128);
                
                /* Index für Title und Fremdschlüsselfelder */
                entity.HasIndex(p => p.Title)
                      .IsUnique(false)
                      .HasDatabaseName("IX_" + nameof(Movie) + "_" +nameof(Movie.Title));

                entity.HasIndex(p => p.GenreId)
                      .IsUnique(false)
                      .HasDatabaseName("IX_" + nameof(Movie) + "_" + nameof(Movie.GenreId));

                entity.HasIndex(p => p.MediumTypeCode)
                      .IsUnique(false)
                      .HasDatabaseName("IX_" + nameof(Movie) + "_" + nameof(Movie.MediumTypeCode));

                entity.Property(p => p.Price)
                      .HasPrecision(19, 2)
                      .HasDefaultValue(0M);

                entity.Property(p => p.ReleaseDate)
                      .HasColumnType("date");
            });

            modelBuilder.Entity<Movie>().HasOne(m => m.MediumType)
                                        .WithMany(m => m.Movies)
                                        .HasForeignKey(m => m.MediumTypeCode)
                                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<MediumType>(entity =>
            {
                entity.HasKey(e => e.Code);

            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasMany(g => g.Movies)
                      .WithOne(m => m.Genre)
                      .HasForeignKey(m => m.GenreId)
                      .OnDelete(DeleteBehavior.Restrict);
            });



        }
    }
}
