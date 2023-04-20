using Microsoft.Xna.Framework;

namespace WebApi.Models;

public struct AreaPoint : IEquatable<AreaPoint>
{
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }

    public AreaPoint(double longitude, double latitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }

    public AreaPoint(Vector2 vector2)
    {
        Longitude = vector2.X;
        Latitude = vector2.Y;
    }

    public override int GetHashCode()
    {
        return Longitude.GetHashCode() + Latitude.GetHashCode();
    }
    public bool Equals(AreaPoint other)
    {
        return other.Longitude == Longitude
               && other.Latitude == Latitude;
    }
    public override bool Equals(object? obj)
    {
        if (obj is AreaPoint areaPoint)
            return Equals(areaPoint);
        return false;
    }

    public static bool operator ==(AreaPoint ap1, AreaPoint ap2) => ap1.Equals(ap2);
    public static bool operator !=(AreaPoint ap1, AreaPoint ap2) => !ap1.Equals(ap2);
    

    public static implicit operator Vector2(AreaPoint areaPoint)
        => new Vector2(Convert.ToSingle(areaPoint.Longitude), Convert.ToSingle(areaPoint.Latitude));
}