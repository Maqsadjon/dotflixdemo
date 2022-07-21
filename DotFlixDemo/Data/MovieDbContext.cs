using DotFlixDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DotFlixDemo.Data
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options)
            :base(options)
        {
        }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Author> Authors { get; set; }

    }
}
