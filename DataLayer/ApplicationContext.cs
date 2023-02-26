using Microsoft.EntityFrameworkCore;
using DataLayer.Entities;

namespace DataLayer;

public class ApplicationContext : DbContext
{
    private readonly string connectionString;

    public DbSet<Account> Accounts { get; set; }

    public ApplicationContext(string connectionString)
    {
        this.connectionString = connectionString;
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 3, 22)));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>().HasData(
            new Account() { Id = 1, FirstName = "Victor", LastName = "Neznanov", Email = "i@vneznanov.ru", Password = "test" });
        base.OnModelCreating(modelBuilder);
    }
}