namespace DataLayer.Entities;

public class VisitedLocation
{
    public long Id { get; set; }
    public DateTime DateTimeOfVisitLocationPoint { get; set; }
    
    public long LocationPointId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public LocationPoint LocationPoint { get; set; } = null!;

    public long AnimalId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public Animal Animal { get; set; } = null!;
}