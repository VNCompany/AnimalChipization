using WebApi.Models;

using DataLayer;
using DataLayer.Entities;

namespace WebApi.Controllers;

public class AreasController : ApiController
{
    private readonly ApplicationContext context;

    public AreasController(ApplicationContext context)
    {
        this.context = context;
    }

    [HttpGet("{areaId?}")]
    public IActionResult Get(long? id)
    {
        if (id == null || id <= 0)
            return StatusCode(400);

        var area = context.Areas.FirstOrDefault(a => a.Id == id);
        if (area != null)
        {
            return Json(new AreaModel(area));
        }
        
        return StatusCode(404);
    }

    [HttpPost]
    [Authorize("ADMIN")]
    public IActionResult Post([FromBody]AreaModel model)
    {
        if (!model.Validate())
            return StatusCode(400);

        foreach (var area in context.Areas.Select(a => new AreaModel(a)))
        {
            // Зона с таким name уже существует
            if (model.Name == area.Name)
                return StatusCode(409);
            // Зона, состоящая из таких точек, уже существует
            if (model.AreaPointsValidated.Length == area.AreaPointsValidated.Length 
                && model.GeometryEquals(area))
                return StatusCode(409);
            // Зона пересекается с другой зоной
            if (model.Intersects(area))
                return StatusCode(409);
        }

        context.Areas.Add(model.ToEntity(new Area()));
        context.SaveChanges();
        
        Response.StatusCode = 201;
        return Json(model);
    }

    [HttpPut("{id?}")]
    [Authorize("ADMIN")]
    public IActionResult Put(long? id, [FromBody]AreaModel model)
    {
        if (id == null || id <= 0 || !model.Validate())
            return StatusCode(400);

        Area? entity = context.Areas.FirstOrDefault(a => a.Id == id);
        if (entity == null)
            return StatusCode(404);

        foreach (var area in context.Areas.Select(a => new AreaModel(a)))
        {
            // Зона с таким name уже существует
            if (model.Name == area.Name)
            {
                if (model.Name != entity.Name)
                    return StatusCode(409);
            }
            // Зона, состоящая из таких точек, уже существует
            if (model.AreaPointsValidated.Length == area.AreaPointsValidated.Length 
                && model.GeometryEquals(area))
                return StatusCode(409);
            // Зона пересекается с другой зоной
            if (model.Intersects(area))
                return StatusCode(409);
        }

        model.ToEntity(entity);
        context.SaveChanges();
        
        return Json(model);
    }

    [HttpDelete("{id?}")]
    [Authorize("ADMIN")]
    public IActionResult Delete(long? id)
    {
        if (id == null || id <= 0)
            return StatusCode(400);
        
        Area? entity = context.Areas.FirstOrDefault(a => a.Id == id);
        if (entity != null)
        {
            context.Areas.Remove(entity);
            context.SaveChanges();
        }

        return StatusCode(404);
    }
}