using DataLayer;
using DataLayer.Entities;

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

        return Json(animals.Skip(from).Take(size).Select((a) =>
        {
            context.LoadAnimalDependecies(a.Id);
            return a;
        }));
    }


    /* part <AnimalsController.Types.cs> */
    /* part <AnimalsController.Locations.cs> */
}