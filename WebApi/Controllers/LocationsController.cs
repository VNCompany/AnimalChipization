using WebApi.Models;
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

    [HttpPost]
    [Authorize]
    public IActionResult Post([FromBody] LocationModel locationModel)
    {
        if (locationModel.Validate())
        {
            if (context.LocationPoints.Count(lp => lp.Latitude == locationModel.Latitude && lp.Longitude == locationModel.Longitude) == 0)
            {
                LocationPoint entity = locationModel.ToEntity(new LocationPoint());
                context.LocationPoints.Add(entity);
                context.SaveChanges();
                Response.StatusCode = 201;
                return Json(entity);
            }
            else return StatusCode(409);
        }
        else return StatusCode(400);
    }

    [HttpPut("{id?}")]
    [Authorize]
    public IActionResult Put(long? id, [FromBody] LocationModel locationModel)
    {
        if (id != null && id > 0 && locationModel.Validate())
        {
            LocationPoint? entity = context.LocationPoints.FirstOrDefault(lp => lp.Id == id);
            if (entity != null)
            {
                if (context.LocationPoints.Count(lp => lp.Latitude == locationModel.Latitude && lp.Longitude == locationModel.Longitude) == 0)
                {
                    locationModel.ToEntity(entity);
                    context.SaveChanges();
                    return Json(entity);
                }
                else return StatusCode(409);
            }
            else return StatusCode(404);
        }
        else return StatusCode(400);
    }

    [HttpDelete("{id?}")]
    [Authorize]
    public IActionResult Delete(long? id)
    {
        if (id != null && id > 0)
        {
            LocationPoint? entity = context.LocationPoints.FirstOrDefault(lp => lp.Id == id);
            if (entity != null)
            {
                if (context.VisitedLocations.Count(vl => vl.LocationPointId == entity.Id) == 0
                    && context.Animals.Count(a => a.ChippingLocationId == entity.Id) == 0)
                {
                    context.LocationPoints.Remove(entity);
                    context.SaveChanges();
                    return StatusCode(200);
                }
            }
            else return StatusCode(404);
        }
        return StatusCode(400);
    }
}