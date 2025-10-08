using FAI.Core.Entities.Movies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
                      .HasDatabaseName("IX_" + nameof(Movie) + "_" + nameof(Movie.Title));

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


            /* Seed Data */
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Comedy" },
                new Genre { Id = 3, Name = "Drama" },
                new Genre { Id = 4, Name = "Horror" },
                new Genre { Id = 5, Name = "Science Fiction" }
            );

            modelBuilder.Entity<MediumType>().HasData(
                new MediumType { Code = "DVD", Name = "Digital Versatile Disc" },
                new MediumType { Code = "BD", Name = "Blu-ray Disc" },
                new MediumType { Code = "4K", Name = "4K Ultra HD Blu Ray" },
                new MediumType { Code = "DIGI", Name = "Digital Copy" },
                new MediumType { Code = "VHS", Name = "Video Home System" },
                new MediumType { Code = "STR", Name = "Streaming" }
            );

            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    Id = new Guid("11111111-1111-1111-1111-111111111111"),
                    Title = "Inception",
                    ReleaseDate = new DateTime(2010, 7, 16),
                    GenreId = 5, // Science Fiction
                    MediumTypeCode = "BD",
                    Price = 14.99M
                },
                new Movie
                {
                    Id = new Guid("22222222-2222-2222-2222-222222222222"),
                    Title = "The Dark Knight",
                    ReleaseDate = new DateTime(2008, 7, 18),
                    GenreId = 1, // Action
                    MediumTypeCode = "4K",
                    Price = 19.99M
                },
                // Horror
                new Movie
                {
                    Id = new Guid("44444444-4444-4444-4444-444444444444"),
                    Title = "The Shining",
                    ReleaseDate = new DateTime(1980, 5, 23),
                    GenreId = 4, // Horror
                    MediumTypeCode = "BD",
                    Price = 12.99M
                },
                new Movie
                {
                    Id = new Guid("55555555-5555-5555-5555-555555555555"),
                    Title = "Get Out",
                    ReleaseDate = new DateTime(2017, 2, 24),
                    GenreId = 4, // Horror
                    MediumTypeCode = "STR",
                    Price = 9.99M
                },
                // Drama
                new Movie
                {
                    Id = new Guid("66666666-6666-6666-6666-666666666666"),
                    Title = "The Shawshank Redemption",
                    ReleaseDate = new DateTime(1994, 9, 23),
                    GenreId = 3, // Drama
                    MediumTypeCode = "DVD",
                    Price = 11.99M
                },
                new Movie
                {
                    Id = new Guid("77777777-7777-7777-7777-777777777777"),
                    Title = "Forrest Gump",
                    ReleaseDate = new DateTime(1994, 7, 6),
                    GenreId = 3, // Drama
                    MediumTypeCode = "BD",
                    Price = 13.99M
                },
                // Comedy
                new Movie
                {
                    Id = new Guid("88888888-8888-8888-8888-888888888888"),
                    Title = "Superbad",
                    ReleaseDate = new DateTime(2007, 8, 17),
                    GenreId = 2, // Comedy
                    MediumTypeCode = "DVD",
                    Price = 8.99M
                },
                new Movie
                {
                    Id = new Guid("99999999-9999-9999-9999-999999999999"),
                    Title = "The Hangover",
                    ReleaseDate = new DateTime(2009, 6, 5),
                    GenreId = 2, // Comedy
                    MediumTypeCode = "DIGI",
                    Price = 10.99M
                }

            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

#if DEBUG
            if(currentDirectory.IndexOf("bin") > -1)
            {
                currentDirectory = currentDirectory.Substring(0, currentDirectory.IndexOf("bin"));
            }
#endif

            var configurationBuilder = new ConfigurationBuilder().SetBasePath(currentDirectory)
                                                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = configurationBuilder.Build();
            var connectionString = configuration.GetConnectionString("MovieDbContext");
            optionsBuilder.UseSqlServer(connectionString, opt => opt.CommandTimeout(60));

            /* Command für Initialisierung der EF-Migration 
             * 1. PM Console öffnen
             * 2. add-migration Initial -startupProject FAI.Persistence
             * 3. update-database -startupProject FAI.Persistence
             */

        }
    }
}
