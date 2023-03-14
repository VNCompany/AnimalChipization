using System.Text.Json.Serialization;

namespace DataLayer.Entities;

public class VisitedLocation
{
    public long Id { get; set; }
    public DateTime DateTimeOfVisitLocationPoint { get; set; }
    
    [JsonIgnore]
    public LocationPoint LocationPoint { get; set; } = null!;
    public long LocationPointId { get; set; }

    [JsonIgnore]
    public Animal Animal { get; set; } = null!;
    public long AnimalId { get; set; }
}