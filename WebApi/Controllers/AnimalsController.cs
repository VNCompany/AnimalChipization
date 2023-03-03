using DataLayer;
using DataLayer.Entities;

namespace WebApi.Controllers;

public class AnimalsController : ApiController
{
    private readonly ApplicationContext context;
    public AnimalsController(ApplicationContext context)
    {
        this.context = context;
    }

    [HttpGet("{id?}")]
    public IActionResult Get(int? id)
    {
        if (id is null || id <= 0)
            return StatusCode(400);

        Animal? animal = context.Animals.FirstOrDefault(animal => animal.Id == id);
        if (animal is null)
            return StatusCode(404);

        animal.AnimalTypes = new long[0];
        animal.VisitedLocations = new long[0];
        return Json(animal);
    }

    [HttpGet("search")]
    public DateTime SearchGet()
    {
        return DateTime.Now;
    }
}