using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data;
using MovieCatalog.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<MoviesDbContext>(options =>
            options.UseInMemoryDatabase("Movies"));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Add some data to database

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();
    if (!dbContext.Movies.Any())
    {
        dbContext.Movies.AddRange(
            new Movie("movie1", "genre1", 2001),
            new Movie("movie2", "genre2", 2002),
            new Movie("movie3", "genre3", 2003),
            new Movie("movie4", "genre4", 2002),
            new Movie("movie5", "genre3", 2005)
        );
        dbContext.SaveChanges();
    }
}

app.Run();


