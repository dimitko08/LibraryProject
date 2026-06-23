using LibraryProject.Business.Interfaces;
using LibraryProject.Business.Services;
using LibraryProject.Data;
using LibraryProject.Data.Interfaces;
using LibraryProject.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryProject.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            var connectionString = "Server=localhost;Port=3306;Database=LibraryDb;User=root;Password=200878;";

            services.AddDbContext<LibraryDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                ));

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorService, AuthorService>();

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
                db.Database.Migrate();
            }

            var menuHandler = new MenuHandler(serviceProvider);
            await menuHandler.RunAsync();
        }
    }
}