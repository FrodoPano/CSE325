using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Use SQLite
builder.Services.AddDbContext<MvcMovieContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MvcMovieContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movies}/{action=Index}/{id?}");

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    try
    {
        var context = services.GetRequiredService<MvcMovieContext>();
        context.Database.EnsureCreated(); // This creates the database if it doesn't exist
        
        // Check if we need to seed data
        if (!context.Movie.Any())
        {
            context.Movie.AddRange(
                new Movie
                {
                    Title = "When Harry Met Sally",
                    ReleaseDate = DateTime.Parse("1989-2-12"),
                    Genre = "Romantic Comedy",
                    Price = 7.99M,
                    Rating = "R"
                },
                new Movie
                {
                    Title = "Ghostbusters",
                    ReleaseDate = DateTime.Parse("1984-3-13"),
                    Genre = "Comedy",
                    Price = 8.99M,
                    Rating = "PG"
                },
                new Movie
                {
                    Title = "Ghostbusters 2",
                    ReleaseDate = DateTime.Parse("1986-2-23"),
                    Genre = "Comedy",
                    Price = 9.99M,
                    Rating = "PG"
                },
                new Movie
                {
                    Title = "Rio Bravo",
                    ReleaseDate = DateTime.Parse("1959-4-15"),
                    Genre = "Western",
                    Price = 3.99M,
                    Rating = "PG-13"
                },
                // Your 3 favorite movies
                new Movie
                {
                    Title = "The Shawshank Redemption",
                    ReleaseDate = DateTime.Parse("1994-9-23"),
                    Genre = "Drama",
                    Price = 12.99M,
                    Rating = "R"
                },
                new Movie
                {
                    Title = "Inception",
                    ReleaseDate = DateTime.Parse("2010-7-16"),
                    Genre = "Sci-Fi",
                    Price = 14.99M,
                    Rating = "PG-13"
                },
                new Movie
                {
                    Title = "The Dark Knight",
                    ReleaseDate = DateTime.Parse("2008-7-18"),
                    Genre = "Action",
                    Price = 13.99M,
                    Rating = "PG-13"
                }
            );
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Run();