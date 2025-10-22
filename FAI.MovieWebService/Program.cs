using FAI.Core.Repositories.Movies;
using FAI.Persistence.Repositories.DBContext;
using FAI.Persistence.Repositories.Movies;
using Microsoft.EntityFrameworkCore;
using FAI.Persistence.Extensions;
using FAI.Application.Extensions;

namespace FAI.MovieWebService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();

            // Swagger configuration
            builder.Services.AddSwaggerGen(g =>
            {
                g.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "FAI Movie Web Service",
                    Version = "v1",
                    Description = "Web Service for FAI Movie Application",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "FAI Support",
                        Email = "horst.schneider@hotmail.com"
                    }
                });
            });

            /* MovieDbContext konfigurieren */
            var connectionString = builder.Configuration.GetConnectionString("MovieDbContext");
            builder.Services.AddDbContext<MovieDbContext>(options => options.UseSqlServer(connectionString));

            // Add services to the container.

            builder.Services.AddControllers();

            /* Beispiel für Registrierung einer Klasse */
            // builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.RegisterRepositories();
          
            builder.Services.RegisterServices();

            var app = builder.Build();

            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FAI Movie Web Service v1");
                });
            }

            // Configure the HTTP request pipeline

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
