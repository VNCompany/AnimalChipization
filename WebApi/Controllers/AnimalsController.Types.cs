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
        if (animalType != null)
            return Json(animalType);
        else
            return StatusCode(404);
    }

    [HttpPost("types")]
    [Authorize]
    public IActionResult TypesPost([FromBody]Dictionary<string, string?> body)
    {
        string? type;
        if (!body.TryGetValue("type", out type)
            || string.IsNullOrWhiteSpace(type))
            return StatusCode(400);

        if (context.AnimalTypes.Count(t => t.Type == type) == 0)
        {
            AnimalType entity = new AnimalType { Type = type };
            context.AnimalTypes.Add(entity);
            context.SaveChanges();

            Response.StatusCode = 201;
            return Json(entity);
        }
        else return StatusCode(409);
    }

    [HttpPut("types/{id?}")]
    [Authorize]
    public IActionResult TypesPut(long? id, [FromBody]Dictionary<string, string?> body)
    {
        string? type;
        if (id == null || id <= 0
            || !body.TryGetValue("type", out type)
            || string.IsNullOrWhiteSpace(type))
            return StatusCode(400);

        AnimalType? entity = context.AnimalTypes.FirstOrDefault(t => t.Id == id);
        if (entity != null)
        {
            if (context.AnimalTypes.Count(t => t.Type == type) == 0)
            {
                entity.Type = type;
                context.SaveChanges();
                return Json(entity);
            }
            else return StatusCode(409);
        }
        else return StatusCode(404);
    }

    [HttpDelete("types/{id?}")]
    [Authorize]
    public IActionResult TypesDelete(long? id)
    {
        if (id != null && id > 0)
        {
            AnimalType? entity = context.AnimalTypes.FirstOrDefault(t => t.Id == id);
            if (entity != null)
            {
                if (context.AnimalsTypesLinks.LongCount(atl => atl.AnimalTypeId == entity.Id) == 0)
                {
                    context.AnimalTypes.Remove(entity);
                    context.SaveChanges();
                    return StatusCode(200);
                }
            }
            else return StatusCode(404);
        }
        return StatusCode(400);
    }
}