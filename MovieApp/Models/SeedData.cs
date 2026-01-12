using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using System;
using System.Linq;

namespace MovieApp.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcMovieContext(
                serviceProvider.GetRequiredService<DbContextOptions<MvcMovieContext>>()))
            {
                // Look for any movies.
                if (context.Movie.Any())
                {
                    return;   // DB has been seeded
                }

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
                    // Add YOUR 3 favorite movies here:
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
    }
}