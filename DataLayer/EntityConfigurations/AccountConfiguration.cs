using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.EntityConfigurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasIndex(p => p.Email).IsUnique();
        builder.Property(p => p.Role)
            .HasConversion(
                v => v.ToString(),
                v => (Role)Enum.Parse(typeof(Role), v));
        builder.Property(p => p.Role).HasDefaultValue(Role.USER);

        builder.HasData(
            new Account()
            {
                Id = 1,
                FirstName = "adminFirstName",
                LastName = "adminLastName",
                Email = "admin@simbirsoft.com",
                Password = "daaad6e5604e8e17bd9f108d91e26afe6281dac8fda0091040a7a6d7bd9b43b5",
                Role = Role.ADMIN
            },
            new Account()
            {
                Id = 2,
                FirstName = "chipperFirstName",
                LastName = "chipperLastName",
                Email = "chipper@simbirsoft.com",
                Password = "daaad6e5604e8e17bd9f108d91e26afe6281dac8fda0091040a7a6d7bd9b43b5",
                Role = Role.CHIPPER
            },
            new Account()
            {
                Id = 3,
                FirstName = "userFirstName",
                LastName = "userLastName",
                Email = "user@simbirsoft.com",
                Password = "daaad6e5604e8e17bd9f108d91e26afe6281dac8fda0091040a7a6d7bd9b43b5",
                Role = Role.USER
            });
    }
}