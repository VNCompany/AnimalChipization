using System.Net.Mail;

using WebApi.Models;
using WebApi.Services;
using DataLayer.Entities;

namespace WebApi.Controllers;

public class RegistrationController : ApiController
{
    [HttpPost]
    public IActionResult Get([FromServices]IAuthorizationService authorizationService, [FromBody]AccountModel model)
    {
        if (Authorize(HttpContext))
            return StatusCode(403);

        if (model.Validate())
        {
            Account? registeredAccount = authorizationService.Register(model);
            if (registeredAccount != null)
            {
                Response.StatusCode = 201;
                return Json(registeredAccount);
            }
            else return StatusCode(409);
        }
        else return StatusCode(400);
    }
}