using System.Net.Mail;

using WebApi.Models;
using WebApi.Services;
using DataLayer.Entities;

namespace WebApi.Controllers;

public class RegistrationController : ApiController
{
    [HttpGet]
    public IActionResult Get([FromServices]IAuthorizationService authorizationService, [FromBody]AccountModel model)
    {
        if (Authorize(HttpContext))
            return StatusCode(403);

        if (string.IsNullOrWhiteSpace(model.FirstName)
            || string.IsNullOrWhiteSpace(model.LastName)
            || string.IsNullOrWhiteSpace(model.Email)
            || string.IsNullOrWhiteSpace(model.Password))
            return StatusCode(400);

        if (MailAddress.TryCreate(model.Email, out var _))
        {
            Account? registeredAccount = authorizationService.Register(model);
            if (registeredAccount != null)
            {
                Response.StatusCode = 201;
                return Json(registeredAccount);
            }
            else return StatusCode(409);
        } 
        else  // Email не соответствует стандарту RFC 822
        {
            return StatusCode(400);
        }
    }
}