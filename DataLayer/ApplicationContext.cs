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
    public DbSet<VisitedLocation> VisitedLocations { get; set; }

    public ApplicationContext(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void LoadAnimalDependecies(long animalId)
    {
        AnimalsTypesLinks.Where(atl => atl.AnimalId == animalId).Load();
        VisitedLocations.Where(vl => vl.AnimalId == animalId).Load();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
        optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 3, 22)));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройки для сущности Account
        modelBuilder.Entity<Account>().HasIndex(p => p.Email).IsUnique();

        // Настройки для сущности Animal
        modelBuilder.Entity<Animal>().Property(p => p.LifeStatus).HasDefaultValue("ALIVE");

        // Настройки для сущности AnimalsTypesLinks
        modelBuilder.Entity<AnimalsTypesLink>().HasKey(key => new { key.AnimalId, key.AnimalTypeId });

        modelBuilder.Entity<Account>().HasData(
            new { Id = 1, FirstName = "Victor", LastName = "Neznanov", Email = "victor@mail.ru", Password = "99bde068af2d49ed7fc8b8fa79abe13a6059e0db320bb73459fd96624bb4b33f" },
            new { Id = 2, FirstName = "Anton", LastName = "Belousov", Email = "anton@mail.ru", Password = "1f29f2d29f02f2608eb72d45625ba3a851eda1ee2be1bda22427a584b787c722" });

        modelBuilder.Entity<LocationPoint>().HasData(
            new { Id = 1L, Latitude = 56.195, Longitude = 23.1212 },
            new { Id = 2L, Latitude = 55.1124, Longitude = 134.5673 });

        modelBuilder.Entity<Animal>().HasData(
            new Animal()
            {
                Id = 1L,
                Weight = 200.2f,
                Length = 185.5f,
                Height = 95.7f,
                Gender = "MALE",
                ChippingDateTime = new DateTime(2022, 7, 13, 12, 23, 54),
                ChipperId = 1,
                ChippingLocationId = 1
            });

        modelBuilder.Entity<AnimalType>().HasData(
            new AnimalType { Id = 1, Type = "elephant" },
            new AnimalType { Id = 2, Type = "monkey" });

        modelBuilder.Entity<AnimalsTypesLink>().HasData(
            new AnimalsTypesLink() { AnimalId = 1, AnimalTypeId = 2L });

        modelBuilder.Entity<VisitedLocation>().HasData(
            new VisitedLocation { 
                Id = 1L, 
                DateTimeOfVisitLocationPoint = new DateTime(2023, 2, 14, 13, 59, 33, DateTimeKind.Local),
                AnimalId = 1L,
                LocationPointId = 1L
            });


        base.OnModelCreating(modelBuilder);
    }
}
