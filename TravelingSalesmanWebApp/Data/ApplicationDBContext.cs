using Microsoft.EntityFrameworkCore;
using TravelingSalesmanWebApp.Data.Models;
using Path = TravelingSalesmanWebApp.Data.Models.Path;

namespace TravelingSalesmanWebApp.Data;

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