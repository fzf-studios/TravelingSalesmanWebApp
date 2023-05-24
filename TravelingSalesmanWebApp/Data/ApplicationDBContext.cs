using BlazorApp2.Data.Models;
using Microsoft.EntityFrameworkCore;
using Path = BlazorApp2.Data.Models.Path;

namespace BlazorApp2.Data;

public class ApplicationDBContext:DbContext
{

    public DbSet<City> Cities { get; set; }
    public DbSet<Path> Paths { get; set; }

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
            .Property(city => city.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Path>()
            .Property(path => path.Id)
            .ValueGeneratedOnAdd();
    }

}