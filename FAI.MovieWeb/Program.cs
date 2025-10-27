using FAI.MovieWeb.Data;
using FAI.Persistence.Repositories.DBContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FAI.Application.Extensions;
using FAI.Persistence.Extensions;
using System.Globalization;
using Microsoft.Extensions.Options;

namespace FAI.MovieWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

             // Add services to the container.
            var connectionStringIdentity = builder.Configuration.GetConnectionString("IdentityDbContext") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionStringIdentity));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            var connectionStringMovie = builder.Configuration.GetConnectionString("MovieDbContext") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<MovieDbContext>(options =>
                options.UseSqlServer(connectionStringMovie));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.RegisterRepositories();
            builder.Services.RegisterServices();


            /* Defintion der Sprachermittlung aus dem HTTP-Request */
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "en", "de", "fr" };

                options.SetDefaultCulture(supportedCultures[1])
                       .AddSupportedCultures(supportedCultures)
                       .AddSupportedUICultures(supportedCultures)
                       .DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("de");
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

       
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Movies}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            /* Sprachermittlung aus dem HTTP-Request aktivieren`*/
            var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.Run();
        }
    }
}
