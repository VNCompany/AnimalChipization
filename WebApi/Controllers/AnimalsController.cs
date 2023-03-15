using WebApi.Models;
using DataLayer;
using DataLayer.Entities;

using System.Diagnostics;

namespace WebApi.Controllers;

public partial class AnimalsController : ApiController
{
    private readonly ApplicationContext context;
    public AnimalsController(ApplicationContext context)
    {
        this.context = context;
    }

    [HttpGet("{id?}")]
    public IActionResult Get(long? id)
    {
        if (id is null || id <= 0)
            return StatusCode(400);

        Animal? animal = context.Animals.FirstOrDefault(animal => animal.Id == id);
        if (animal != null)
        {
            context.LoadAnimalDependecies(animal.Id);
            return Json(animal);
        }
        else
            return StatusCode(404);
    }

    [HttpGet("search")]
    public IActionResult SearchGet()
    {
        int from = GetQueryParameterInt32("from") ?? 0;
        int size = GetQueryParameterInt32("size") ?? 10;

        var startDateTimeParams = DateTimeMethods.Parse(GetQueryParameterString("startDateTime"));
        var endDateTimeParams = DateTimeMethods.Parse(GetQueryParameterString("endDateTime"));

        if (from < 0 || size <= 0
            || startDateTimeParams.Status == DateTimeValidationStatus.Failure
            || endDateTimeParams.Status == DateTimeValidationStatus.Failure)
            return StatusCode(400);

        IEnumerable<Animal> animals = context.Animals;

        if (startDateTimeParams.Status == DateTimeValidationStatus.Success)
            animals = animals.Where(animal => animal.ChippingDateTime >= startDateTimeParams.Value);
        if (endDateTimeParams.Status == DateTimeValidationStatus.Success)
            animals = animals.Where(animal => animal.ChippingDateTime <= endDateTimeParams.Value);

        int? chipperId = GetQueryParameterInt32("chipperId");
        if (chipperId is not null)
        {
            if (chipperId <= 0) return StatusCode(400);
            animals = animals.Where(animal => animal.ChipperId == chipperId);
        }

        long? chippingLocationId = GetQueryParameterInt64("chippingLocationId");
        if (chippingLocationId is not null)
        {
            if (chippingLocationId <= 0) return StatusCode(400);
            animals = animals.Where(animal => animal.ChippingLocationId == chippingLocationId);
        }

        string? lifeStatus = GetQueryParameterString("lifeStatus");
        if (lifeStatus is not null)
        {
            if (new[] { "ALIVE", "DEAD" }.Contains(lifeStatus) == false) return StatusCode(400);
            animals = animals.Where(animal => animal.LifeStatus == lifeStatus);
        }

        string? gender = GetQueryParameterString("gender");
        if (gender is not null)
        {
            if (new[] { "MALE", "FEMALE", "OTHER" }.Contains(gender) == false) return StatusCode(400);
            animals = animals.Where(animal => animal.Gender == gender);
        }

        List<Animal> animalList = animals.Skip(from).Take(size).ToList();
        context.LoadAnimalDependecies(animalList.Select(a => a.Id));
        return Json(animalList);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Post([FromBody]AnimalPostModel model)
    {
        if (model.Validate())
        {
            long?[] animalTypes = model.AnimalTypes!;
            if (animalTypes.Length == animalTypes.Distinct().Count())
            {
                if (context.Accounts.Count(a => a.Id == model!.ChipperId) == 0
                    || context.LocationPoints.Count(lp => lp.Id == model!.ChippingLocationId) == 0)
                    return StatusCode(404);

                List<AnimalType> selectedAnimalTypes = new List<AnimalType>(animalTypes.Length);
                foreach (long? animalTypeId in animalTypes)
                {
                    AnimalType? animalType = context.AnimalTypes.FirstOrDefault(t => t.Id == animalTypeId);
                    if (animalType != null)
                        selectedAnimalTypes.Add(animalType);
                    else
                        return StatusCode(404);  // Тип животного с Id == animalTypeId не найден
                }

                Animal newAnimal = model.ToEntity(new Animal() { ChippingDateTime = DateTimeMethods.NowWithoutMilliseconds() });
                context.Animals.Add(newAnimal);
                context.AnimalsTypesLinks.AddRange(
                    selectedAnimalTypes.Select(t => new AnimalsTypesLink
                    {
                        Animal = newAnimal,
                        AnimalType = t
                    }));
                context.SaveChanges();

                Response.StatusCode = 201;
                return Json(newAnimal);
            }
            else return StatusCode(409);
        }
        else return StatusCode(400);
    }

    [HttpPut("{id?}")]
    [Authorize]
    public IActionResult Put(long? id, [FromBody]AnimalPutModel model)
    {
        if (id != null && id > 0 && model.Validate())
        {
            Animal? animal = context.Animals.FirstOrDefault(a => a.Id == id);
            Account? chipper = context.Accounts.FirstOrDefault(a => a.Id == model.ChipperId);
            LocationPoint? chippingLocation = context.LocationPoints.FirstOrDefault(lp => lp.Id == model.ChippingLocationId);
            if (animal != null && chipper != null && chippingLocation != null)
            {
                context.LoadAnimalDependecies(animal.Id);
                if (!(animal.LifeStatus == "DEAD" && model.LifeStatus == "ALIVE")
                    && (animal.VisitedLocations.Count == 0 || animal.VisitedLocations[0].LocationPointId != model.ChippingLocationId))
                {
                    model.ToEntity(animal);
                    if (animal.LifeStatus == "DEAD")
                        animal.DeathDateTime = DateTimeMethods.NowWithoutMilliseconds();
                    context.SaveChanges();
                    return Json(animal);
                }
                else return StatusCode(400);
            }
            else return StatusCode(404);
        }
        return StatusCode(400);
    }

    [HttpDelete("{id?}")]
    [Authorize]
    public IActionResult Delete(long? id)
    {
        if (id != null && id > 0)
        {
            Animal? animal = context.Animals.FirstOrDefault(a => a.Id == id);
            if (animal != null)
            {
                context.LoadAnimalDependecies(animal.Id);
                if (animal.VisitedLocations.Count == 0)
                {
                    context.Animals.Remove(animal);
                    context.SaveChanges();
                    return StatusCode(200);
                }
            }
            else return StatusCode(404);
        }
        return StatusCode(400);
    }

    /* part <AnimalsController.Types.cs> */
    /* part <AnimalsController.Locations.cs> */
}