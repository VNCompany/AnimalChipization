using DataLayer.Entities;

namespace WebApi.Controllers;

public partial class AnimalsController  /* Types */
{
    [HttpGet("types/{id?}")]
    public IActionResult TypesGet(long? id)
    {
        if (id is null || id <= 0)
            return StatusCode(400);

        AnimalType? animalType = context.AnimalTypes.FirstOrDefault(at => at.Id == id);
        if (animalType is not null)
            return Json(animalType);

        return StatusCode(404);
    }
}