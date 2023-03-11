using DataLayer;
using DataLayer.Entities;

namespace WebApi.Controllers;

public class LocationsController : ApiController
{
    private readonly ApplicationContext context;
    public LocationsController(ApplicationContext context)
    {
        this.context = context;
    }

    [HttpGet("{id?}")]
    public IActionResult Get(long? id)
    {
        if (id == null || id <= 0)
            return StatusCode(400);

        LocationPoint? lp = context.LocationPoints.FirstOrDefault(p => p.Id == id);
        if (lp != null)
            return Json(lp);
        else
            return StatusCode(404);
    }
}