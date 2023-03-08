using Microsoft.EntityFrameworkCore;
using DataLayer.Entities;

namespace DataLayer;

public class ApplicationContext : DbContext
{
    private readonly string connectionString;

    public DbSet<Account> Accounts { get; set; }
    public DbSet<LocationPoint> LocationPoints { get; set; }
    public DbSet<Animal> Animals { get; set; }
    public DbSet<AnimalType> AnimalTypes { get; set; }
    public DbSet<AnimalsTypesLink> AnimalsTypesLinks { get; set; }

    public ApplicationContext(string connectionString)
    {
        this.connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
        optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 3, 22)));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройки для модели Animal
        modelBuilder.Entity<Animal>().Ignore(prop => prop.AnimalTypes);
        modelBuilder.Entity<Animal>().Ignore(prop => prop.VisitedLocations);

        // Настройки для модели AnimalsTypesLinks
        modelBuilder.Entity<AnimalsTypesLink>().HasKey(key => new { key.AnimalId, key.AnimalTypeId });

        modelBuilder.Entity<Account>().HasData(
            new { Id = 1, FirstName = "Victor", LastName = "Neznanov", Email = "i@vneznanov.ru", Password = "99bde068af2d49ed7fc8b8fa79abe13a6059e0db320bb73459fd96624bb4b33f" },
            new { Id = 2, FirstName = "Anton", LastName = "Belousov", Email = "i@vneznanov.ru", Password = "1f29f2d29f02f2608eb72d45625ba3a851eda1ee2be1bda22427a584b787c722" });

        modelBuilder.Entity<LocationPoint>().HasData(
            new { Id = 1L, Latitude = 56.195, Longitude = 23.1212 });

        modelBuilder.Entity<Animal>().HasData(
            new Animal()
            {
                Id = 1L,
                Weight = 200.2f,
                Length = 185.5f,
                Height = 95.7f,
                Gender = "MALE",
                LifeStatis = "ALIVE",
                ChippingDateTime = DateTimeExtensions.ClearNow(),
                ChipperId = 1,
                ChippingLocationId = 1
            });

        modelBuilder.Entity<AnimalType>().HasData(
            new AnimalType { Id = 1, Type = "elephant" },
            new AnimalType { Id = 2, Type = "monkey" });

        modelBuilder.Entity<AnimalsTypesLink>().HasData(
            new AnimalsTypesLink() { AnimalId = 1, AnimalTypeId = 2L });


        base.OnModelCreating(modelBuilder);
    }
}