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
    public IActionResult TypesPost([FromBody] Dictionary<string, string?> body)
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
    public IActionResult TypesPut(long? id, [FromBody] Dictionary<string, string?> body)
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

    [HttpPost("{animalId}/types")]
    [Authorize]
    public StatusCodeResult AnimalsTypesPost__condNullableTypeId() => StatusCode(400);  // typeId is null
    
    [HttpPost("types/{typeId}")]
    [Authorize]
    public StatusCodeResult AnimalsTypesPost__condNullableAnimalId() => StatusCode(400); // animalId is null

    [HttpPost("{animalId}/types/{typeId}")]
    [Authorize]
    public IActionResult AnimalTypesPost(long animalId, long typeId)
    {
        if (animalId <= 0 || typeId <= 0) return StatusCode(400);

        Animal? animal; AnimalType? animalType;
        if ((animal = context.Animals.FirstOrDefault(a => a.Id == animalId)) != null
            && (animalType = context.AnimalTypes.FirstOrDefault(t => t.Id == typeId)) != null)
        {
            if (context.AnimalsTypesLinks.Count(
                atl => atl.AnimalId == animalId && atl.AnimalTypeId == typeId) == 0)
            {
                context.AnimalsTypesLinks.Add(new AnimalsTypesLink { Animal = animal, AnimalType = animalType });
                context.SaveChanges();
                context.LoadAnimalDependecies(animal.Id);
                Response.StatusCode = 201;
                return Json(animal);
            }
            else return StatusCode(409);
        }
        else return StatusCode(404);
    }

    [HttpPut("{animalId?}/types")]
    [Authorize]
    public IActionResult AnimalTypesPut(long? animalId, [FromBody] Dictionary<string, long?> body)
    {
        if (animalId == null || animalId <= 0 || body.Count < 2
            || !body.TryGetValue("oldTypeId", out long? oldTypeId)
            || !body.TryGetValue("newTypeId", out long? newTypeId)
            || oldTypeId == null || newTypeId == null
            || oldTypeId <= 0 || newTypeId <= 0)
            return StatusCode(400);

        Animal? animal = context.Animals.FirstOrDefault(a => a.Id == animalId);
        if (animal != null)
        {
            List<AnimalsTypesLink> animalTypesLinks = context.AnimalsTypesLinks.Where(atl => atl.AnimalId == animal.Id).ToList();
            AnimalsTypesLink? oldAnimalTypeLink;
            AnimalType? newAnimalType;
            if ((oldAnimalTypeLink = animalTypesLinks.Find(atl => atl.AnimalTypeId == oldTypeId)) != null
                && (newAnimalType = context.AnimalTypes.FirstOrDefault(t => t.Id == newTypeId)) != null)
            {
                if (animalTypesLinks.Find(atl => atl.AnimalTypeId == newTypeId) != null)
                    return StatusCode(409);

                context.AnimalsTypesLinks.Remove(oldAnimalTypeLink);
                context.AnimalsTypesLinks.Add(new AnimalsTypesLink { Animal = animal, AnimalType = newAnimalType });
                context.SaveChanges();
                return Json(animal);
            }
        }
        return StatusCode(404);
    }

    [HttpDelete("{animalId}/types")]
    [Authorize]
    public StatusCodeResult AnimalTypesDelete__condNullableTypeId() => StatusCode(400);

    [HttpDelete("{animalId}/types/{typeId}")]
    [Authorize]
    public IActionResult AnimalTypesDelete(long animalId, long typeId)
    {
        if (animalId <= 0 || typeId <= 0)
            return StatusCode(400);

        Animal? animal = context.Animals.FirstOrDefault(a => a.Id == animalId);
        if (animal != null)
        {
            List<AnimalsTypesLink> animalsTypesLinks = context.AnimalsTypesLinks.Where(atl => atl.AnimalId == animal.Id).ToList();
            AnimalsTypesLink? animalsTypesLink = animalsTypesLinks.Find(atl => atl.AnimalTypeId == typeId);
            if (animalsTypesLink != null)
            {
                if (animalsTypesLinks.Count == 1)
                    return StatusCode(400);  // У животного только один тип и это тип с typeId

                context.AnimalsTypesLinks.Remove(animalsTypesLink);
                context.SaveChanges();
                return Json(animal);
            }
        }
        return StatusCode(404);
    }
}