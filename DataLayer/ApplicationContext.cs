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

    public void LoadAnimalDependecies(IEnumerable<long> animalIds)
    {
        AnimalsTypesLinks.Where(atl => animalIds.Contains(atl.AnimalId)).Load();
        VisitedLocations.Where(vl => animalIds.Contains(vl.AnimalId)).Load();
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

        base.OnModelCreating(modelBuilder);
    }
}
