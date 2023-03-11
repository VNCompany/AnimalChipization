using DataLayer.Entities;

namespace WebApi.Controllers;

public partial class AnimalsController  /* Locations */
{
    [HttpGet("{animalId?}/locations")]
    public IActionResult LocationsGet(long? animalId)
    {
        int from = GetQueryParameterInt32("from") ?? 0;
        int size = GetQueryParameterInt32("size") ?? 10;

        var startDateTimeParams = DateTimeMethods.Parse(GetQueryParameterString("startDateTime"));
        var endDateTimeParams = DateTimeMethods.Parse(GetQueryParameterString("endDateTime"));

        if (animalId <= 0
            || from < 0 || size <= 0
            || startDateTimeParams.Status == DateTimeValidationStatus.InvalidDateTime
            || endDateTimeParams.Status == DateTimeValidationStatus.InvalidDateTime)
            return StatusCode(400);

        if (context.Animals.Where(a => a.Id == animalId).LongCount() == 0)
            return StatusCode(404);

        IEnumerable<VisitedLocation> visitedLocations = context.VisitedLocations.Where(vlo => vlo.AnimalId == animalId);

        if (startDateTimeParams.Status == DateTimeValidationStatus.Success)
            visitedLocations = visitedLocations.Where(vlo => vlo.DateTimeOfVisitLocationPoint >= startDateTimeParams.DateTime);
        if (endDateTimeParams.Status == DateTimeValidationStatus.Success)
            visitedLocations = visitedLocations.Where(vlo => vlo.DateTimeOfVisitLocationPoint <= endDateTimeParams.DateTime);

        return Json(visitedLocations.OrderBy(vlo => vlo.DateTimeOfVisitLocationPoint).Skip(from).Take(size));
    }
}