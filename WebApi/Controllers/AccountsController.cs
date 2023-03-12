using WebApi.Models;
using DataLayer;
using DataLayer.Entities;

namespace WebApi.Controllers;

public class AccountsController : ApiController
{
    private readonly ApplicationContext context;
    public AccountsController(ApplicationContext context)
    {
        this.context = context;
    }

    // Получение информации об аккаунте пользователя
    [HttpGet("{id?}")]
    public IActionResult Get(int? id)
    {
        if (id == null || id <= 0)
            return StatusCode(400);

        Account? account = context.Accounts.FirstOrDefault(account => account.Id == id);
        if (account != null)
            return Json(account);
        else
            return StatusCode(404);
    }

    // Поиск аккаунтов пользователей по параметрам
    [HttpGet("search")]
    public IActionResult SearchGet()
    {
        int from = GetQueryParameterInt32("from") ?? 0;
        int size = GetQueryParameterInt32("size") ?? 10;

        if (from < 0 || size <= 0) 
            return StatusCode(400);

        IEnumerable<Account> accounts = context.Accounts;

        string? firstName = GetQueryParameterString("firstName");
        if (firstName is not null)
            accounts = accounts.Where(account => account.FirstName.ToLower().Contains(firstName.ToLower()));

        string? lastName = GetQueryParameterString("lastName");
        if (lastName is not null)
            accounts = accounts.Where(account => account.LastName.ToLower().Contains(lastName.ToLower()));

        string? email = GetQueryParameterString("email");
        if (email is not null)
            accounts = accounts.Where(account => account.Email.ToLower().Contains(email.ToLower()));

        return Json(accounts.Skip(from).Take(size));
    }

    [HttpPut("{id?}")]
    [Authorize]
    public IActionResult Put(int? id, [FromBody]AccountModel model)
    {
        if (id != null && id > 0 && model.Validate())
        {
            if (id == Account!.Id)
            {
                if (context.Accounts.Count(a => a.Email == model.Email) == 0)
                {
                    Account account = context.Accounts.First(a => a.Id == id);
                    account.FirstName = model.FirstName!;
                    account.LastName = model.LastName!;
                    account.Email = model.Email!;
                    account.Password = Services.AuthorizationService.SHA256Hash(model.Password!);
                    context.SaveChanges();
                    return Json(account);
                }
                else return StatusCode(409);
            }
            else return StatusCode(403);
        }
        else return StatusCode(400);
    }

    [HttpDelete("{id?}")]
    [Authorize]
    public IActionResult Delete(int? id)
    {
        if (id != null && id > 0
            && context.Animals.Count(a => a.ChipperId == id) == 0)
        {
            if (Account!.Id == id)
            {
                context.Accounts.Remove(Account);
                context.SaveChanges();
                return StatusCode(200);
            }
            else return StatusCode(403);
        }
        else return StatusCode(400);
    }
}