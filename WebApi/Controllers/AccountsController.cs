using WebApi.Models;
using WebApi.Services;
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
    [Authorize]
    public IActionResult Get(int? id)
    {
        if (id == null || id <= 0)
            return StatusCode(400);

        if (Account!.Role == Role.ADMIN)
        {
            Account? account = context.Accounts.FirstOrDefault(account => account.Id == id);
            if (account != null)
                return Json(account);
            else
                return StatusCode(404);
        }
        else if (Account!.Id == id)
            return Json(Account!);
        else
            return StatusCode(403);
    }

    // Поиск аккаунтов пользователей по параметрам
    [HttpGet("search")]
    [Authorize]
    public IActionResult SearchGet()
    {
        int from = GetQueryParameterInt32("from") ?? 0;
        int size = GetQueryParameterInt32("size") ?? 10;

        if (from < 0 || size <= 0) 
            return StatusCode(400);

        if (Account!.Role != Role.ADMIN)
            return StatusCode(403);

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

    [HttpPost]
    [Authorize("ADMIN")]
    public IActionResult Post([FromBody]AccountModelExtended model, [FromServices]IAuthorizationService authorizationService)
    {
        if (!model.Validate())
            return StatusCode(400);

        Account? registeredAccount = authorizationService.Register(model);
        if (registeredAccount is null)
            return StatusCode(409);

        Response.StatusCode = 201;
        return Json(registeredAccount);
    }

    [HttpPut("{id?}")]
    [Authorize]
    public IActionResult Put(int? id, [FromBody]AccountModelExtended model)
    {
        if (id is > 0 && model.Validate())
        {
            Account? account;
            if (Account!.Role == Role.ADMIN)
                account = context.Accounts.FirstOrDefault(a => a.Id == id);
            else if (Account!.Id == id)
                account = Account;
            else return StatusCode(403);  // Не админ профиль, но попытка изменить чужой или несуществующий аккаунт
                
            if (account != null)
            {
                if (context.Accounts.Count(a => a.Email == model.Email) == 0 || account.Email == model.Email)
                {
                    model.ToEntity(account);
                    account.Password = AuthorizationService.SHA256Hash(account.Password);
                    context.SaveChanges();
                    return Json(account);
                }
                else return StatusCode(409);
            }
            else return StatusCode(404);
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
            Account? account;
            if (Account!.Role == Role.ADMIN)
                account = context.Accounts.FirstOrDefault(a => a.Id == id);
            else if (Account!.Id == id)
                account = Account;
            else return StatusCode(403);  // Не админ профиль, но попытка удалить чужой или несуществующий аккаунт
            
            if (account != null)
            {
                context.Accounts.Remove(account);
                context.SaveChanges();
                return StatusCode(200);
            }
            else return StatusCode(404);
        }
        else return StatusCode(400);
    }
}