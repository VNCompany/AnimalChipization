using System.Security.Cryptography;
using System.Text;

using DataLayer;
using DataLayer.Entities;
using WebApi.Models;

namespace WebApi.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly ApplicationContext context;
    public AuthorizationService(ApplicationContext context)
    {
        this.context = context;
    }

    public static string SHA256Hash(string input)
    {
        using SHA256 sha = SHA256.Create();
        return string.Concat(
            sha.ComputeHash(
                Encoding.UTF8.GetBytes(input))
            .Select(b => b.ToString("x2")));
    }

    public bool Authorize(string token, out Account? account)
    {
        string[] tokenParts;
        try
        {
            tokenParts = Encoding.UTF8.GetString(
            Convert.FromBase64String(token))
            .Split(':');
        }
        catch (FormatException)
        {
            account = null;
            return false;
        }
        (string login, string password) = (tokenParts[0], tokenParts[1]);

        Account? _account = context.Accounts.FirstOrDefault(a => a.Email == login);

        if (_account != null &&
            _account.Password.Equals(SHA256Hash(password)))
        {
            account = _account;
            return true;
        }
        else
        {
            account = null;
            return false;
        }
    }

    public Account? Register(AccountModel accountModel)
    {
        if (context.Accounts.Count(a => a.Email == accountModel.Email) == 0)
        {
            Account account = accountModel.ToEntity(new Account());
            account.Password = SHA256Hash(account.Password);
            context.Accounts.Add(account);
            context.SaveChanges();
            return account;
        }
        return null;
    }
}