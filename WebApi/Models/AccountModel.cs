using System.Net.Mail;

using DataLayer.Entities;

namespace WebApi.Models;

public class AccountModel : IModel<Account>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    public virtual Account ToEntity(Account entity)
    {
        entity.Email = Email ?? throw new ArgumentNullException(nameof(Email));
        entity.FirstName = FirstName ?? throw new ArgumentNullException(nameof(FirstName));
        entity.LastName = LastName ?? throw new ArgumentNullException(nameof(LastName));
        entity.Password = Password ?? throw new ArgumentNullException(nameof(Password));
        return entity;
    }

    public virtual bool Validate()
        => !string.IsNullOrWhiteSpace(FirstName)
            && !string.IsNullOrWhiteSpace(LastName)
            && !string.IsNullOrWhiteSpace(Email)
            && !string.IsNullOrWhiteSpace(Password)
            && MailAddress.TryCreate(Email!, out var _);  // Проверка email на соответствие стандарту RFC 822
}