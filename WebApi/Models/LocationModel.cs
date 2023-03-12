using DataLayer.Entities;

namespace WebApi.Models;

public class LocationModel : IModel<LocationPoint>
{
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public bool Validate()
        => Latitude != null && Longitude != null
        && Latitude >= -90 && Latitude <= 90
        && Longitude >= -180 && Longitude <= 180;

    public LocationPoint ToEntity(LocationPoint entity)
    {
        entity.Latitude = Latitude ?? throw new ArgumentNullException(nameof(Latitude));
        entity.Longitude = Longitude ?? throw new ArgumentNullException(nameof(Longitude));
        return entity;
    }
}