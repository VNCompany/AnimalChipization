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
        if (account is null)
            return StatusCode(404);

        return Json(account);
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
}