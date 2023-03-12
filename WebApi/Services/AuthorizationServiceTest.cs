using System.Text;

using DataLayer.Entities;
using WebApi.Models;

namespace WebApi.Services;

public class AuthorizationServiceTest : IAuthorizationService
{
    public bool Authorize(string token, out Account? account)
    {
        string[] tokenParts = Encoding.UTF8.GetString(
            Convert.FromBase64String(token)).Split(':');
        (string login, string password) = (tokenParts[0], tokenParts[1]);

        if (login == "admin@mail.ru" &&  password == "qwerty")
        {
            account = new Account() { Email = login, Password = password, FirstName = "None", LastName = "None", Id = 23421 };
            return true;
        }

        account = null;
        return false;
    }

    public Account? Register(AccountModel accountModel)
    {
        throw new NotImplementedException();
    }
}