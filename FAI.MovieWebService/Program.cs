using FAI.Application.Authentication;
using FAI.Application.Extensions;
using FAI.Core.Repositories.Movies;
using FAI.Persistence.Extensions;
using FAI.Persistence.Repositories.DBContext;
using FAI.Persistence.Repositories.Movies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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

                g.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authentictation header using basic scheme"

                });

                g.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string []{}
                    }

                });

            });

            /* MovieDbContext konfigurieren */
            var connectionString = builder.Configuration.GetConnectionString("MovieDbContext");
            builder.Services.AddDbContext<MovieDbContext>(options => options.UseSqlServer(connectionString));

            // Add services to the container.

            builder.Services.AddControllers();

            /* BasicAuthentication Handler registrieren */
            builder.Services.AddAuthentication(nameof(BasicAuthenticationHandler))
                            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(nameof(BasicAuthenticationHandler), null);

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
