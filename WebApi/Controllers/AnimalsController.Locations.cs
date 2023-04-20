using WebApi.ViewModels;
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
            || startDateTimeParams.Status == DateTimeValidationStatus.Failure
            || endDateTimeParams.Status == DateTimeValidationStatus.Failure)
            return StatusCode(400);

        if (context.Animals.Where(a => a.Id == animalId).LongCount() == 0)
            return StatusCode(404);

        IEnumerable<VisitedLocation> visitedLocations = context.VisitedLocations.Where(vlo => vlo.AnimalId == animalId);

        if (startDateTimeParams.Status == DateTimeValidationStatus.Success)
            visitedLocations = visitedLocations.Where(vlo => vlo.DateTimeOfVisitLocationPoint >= startDateTimeParams.Value);
        if (endDateTimeParams.Status == DateTimeValidationStatus.Success)
            visitedLocations = visitedLocations.Where(vlo => vlo.DateTimeOfVisitLocationPoint <= endDateTimeParams.Value);

        return Json(visitedLocations.Skip(from).Take(size));
    }


    [HttpPost("locations/{pointId?}")]
    [Authorize("ADMIN", "CHIPPER")]
    public StatusCodeResult LocationsPost__condNullable() => StatusCode(400);

    [HttpPost("{animalId}/locations/{pointId?}")]
    [Authorize("ADMIN", "CHIPPER")]
    public IActionResult LocationsPost(long animalId, long? pointId)
    {
        if (animalId <= 0 || pointId == null || pointId <= 0)
            return StatusCode(400);

        VisitedLocationsViewModel? vm = VisitedLocationsViewModel.Build(context, animalId);
        if (vm != null)
        {
            if (vm.Animal.LifeStatus == "DEAD") return StatusCode(400);  // У животного lifeStatus = "DEAD"
            if (vm.Animal.VisitedLocations.Count == 0 && vm.Animal.ChippingLocationId == pointId) return StatusCode(400);  // Животное находится в точке чипирования и никуда не перемещалось, попытка добавить точку локации, равную точке чипирования
            if (vm.VisitedLocations.Count > 0 && vm.LastVisitedLocation!.LocationPointId == pointId) return StatusCode(400);  // Попытка добавить точку локации, в которой уже находится животное

            LocationPoint? locationPoint = context.LocationPoints.FirstOrDefault(lp => lp.Id == pointId);
            if (locationPoint != null)
            {
                VisitedLocation vl = new VisitedLocation
                {
                    Animal = vm.Animal,
                    LocationPoint = locationPoint,
                    DateTimeOfVisitLocationPoint = DateTimeMethods.NowWithoutMilliseconds()
                };
                context.VisitedLocations.Add(vl);
                context.SaveChanges();

                Response.StatusCode = 201;
                return Json(vl);
            }
        }
        return StatusCode(404);
    }


    [HttpPut("locations")]
    [Authorize("ADMIN", "CHIPPER")]
    public StatusCodeResult LocationsPut__condNullableAnimalId() => StatusCode(400);

    [HttpPut("{animalId}/locations")]
    [Authorize("ADMIN", "CHIPPER")]
    public IActionResult LocationsPut(long animalId, [FromBody] Dictionary<string, long?> body)
    {
        if (animalId <= 0
            || !body.TryGetValue("visitedLocationPointId", out long? visitedLocationPointId)
            || !body.TryGetValue("locationPointId", out long? locationPointId)
            || visitedLocationPointId == null || visitedLocationPointId <= 0
            || locationPointId == null || locationPointId <= 0)
            return StatusCode(400);

        VisitedLocationsViewModel? vm = VisitedLocationsViewModel.Build(context, animalId);
        if (vm != null)
        {
            int visitedLocationIndex = vm.VisitedLocations.FindIndex(vl => vl.Id == visitedLocationPointId);
            if (visitedLocationIndex != -1)
            {
                if (vm.VisitedLocations[visitedLocationIndex].LocationPointId == locationPointId) return StatusCode(400);  // Обновление точки на такую же точку
                
                if (visitedLocationIndex == 0
                    && vm.Animal.ChippingLocationId == locationPointId) return StatusCode(400);  // Обновление первой посещенной точки на точку чипирования

                if ((visitedLocationIndex > 0
                        && vm.VisitedLocations[visitedLocationIndex - 1].LocationPointId == locationPointId)
                    || (vm.VisitedLocations.Count - 1 > visitedLocationIndex
                        && vm.VisitedLocations[visitedLocationIndex + 1].LocationPointId == locationPointId)) return StatusCode(400);  // Обновление точки локации на точку, совпадающую со следующей и/или с предыдущей точками

                LocationPoint? locationPoint = context.LocationPoints.FirstOrDefault(lp => lp.Id == locationPointId);
                if (locationPoint != null)
                {
                    vm.VisitedLocations[visitedLocationIndex].LocationPoint = locationPoint;
                    context.SaveChanges();
                    return Json(vm.VisitedLocations[visitedLocationIndex]);
                }
            }
        }
        return StatusCode(404);
    }


    [HttpDelete("locations/{visitedPointId?}")]
    [Authorize("ADMIN")]
    public StatusCodeResult LocationsDelete__condNullable() => StatusCode(400);

    [HttpDelete("{animalId}/locations/{visitedPointId?}")]
    [Authorize("ADMIN")]
    public IActionResult LocationsDelete(long animalId, long? visitedPointId)
    {
        if (animalId <= 0 || visitedPointId == null || visitedPointId <= 0)
            return StatusCode(400);

        VisitedLocationsViewModel? vm = VisitedLocationsViewModel.Build(context, animalId);
        if (vm != null)
        {
            int visitedLocationIndex = vm.VisitedLocations.FindIndex(vl => vl.Id == visitedPointId);
            if (visitedLocationIndex != -1)
            {
                context.VisitedLocations.Remove(vm.VisitedLocations[visitedLocationIndex]);
                if (vm.VisitedLocations.Count > 1
                    && visitedLocationIndex == 0 
                    && vm.VisitedLocations[1].LocationPointId == vm.Animal.ChippingLocationId)
                    context.VisitedLocations.Remove(vm.VisitedLocations[1]);

                context.SaveChanges();
                return StatusCode(200);
            }
        }
        return StatusCode(404);
    }
}