using Microsoft.EntityFrameworkCore;
using DataLayer.Entities;

namespace DataLayer;

public class ApplicationContext : DbContext
{
    private readonly string connectionString;

    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<LocationPoint> LocationPoints { get; set; } = null!;
    public DbSet<Animal> Animals { get; set; } = null!;
    public DbSet<AnimalType> AnimalTypes { get; set; } = null!;
    public DbSet<AnimalsTypesLink> AnimalsTypesLinks { get; set; } = null!;
    public DbSet<VisitedLocation> VisitedLocations { get; set; } = null!;

    public ApplicationContext(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void Migrate()
    {
        if (Database.GetPendingMigrations().Any())
            Database.Migrate();
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
        optionsBuilder.UseNpgsql(connectionString);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройки для сущности Account
        new EntityConfigurations.AccountConfiguration().Configure(modelBuilder.Entity<Account>());

        // Настройки для сущности Animal
        modelBuilder.Entity<Animal>().Property(p => p.LifeStatus).HasDefaultValue("ALIVE");
        modelBuilder.Entity<Animal>().Property(p => p.Weight).HasColumnType("NUMERIC(13,8)");
        modelBuilder.Entity<Animal>().Property(p => p.Length).HasColumnType("NUMERIC(13,8)");
        modelBuilder.Entity<Animal>().Property(p => p.Height).HasColumnType("NUMERIC(13,8)");

        // Настройки для сущности AnimalsTypesLinks
        modelBuilder.Entity<AnimalsTypesLink>().HasKey(key => new { key.AnimalId, key.AnimalTypeId });

        base.OnModelCreating(modelBuilder);
    }
}
