using DataLayer.Entities;

namespace WebApi.Models;

public class AccountModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    public Account ToEntity()
    {
        return new Account()
        {
            Email = this.Email ?? throw new ArgumentNullException(nameof(Email)),
            FirstName = this.FirstName ?? throw new ArgumentNullException(nameof(FirstName)),
            LastName = this.LastName ?? throw new ArgumentNullException(nameof(LastName)),
            Password = this.Password ?? throw new ArgumentNullException(nameof(Password)),
        };
    }
}