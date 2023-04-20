using System.Text.Json.Serialization;

namespace DataLayer.Entities;

public class Animal
{
    public long Id { get; set; }
    public double Weight { get; set; }
    public double Length { get; set; }
    public double Height { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string LifeStatus { get; set; } = "ALIVE";
    public DateTime? ChippingDateTime { get; set; }
    public DateTime? DeathDateTime { get; set; }

    [JsonIgnore]
    public Account Chipper { get; set; } = null!;
    public int ChipperId { get; set; }

    [JsonIgnore]
    public LocationPoint ChippingLocation { get; set; } = null!;
    public long ChippingLocationId { get; set; }

    [JsonIgnore]
    public List<AnimalsTypesLink> AnimalsTypesLinks { get; set; } = new();
    [JsonIgnore]
    public List<VisitedLocation> VisitedLocations { get; set; } = new();

    [JsonPropertyName("animalTypes")]
    public IEnumerable<long> AnimalTypesIds => AnimalsTypesLinks.Select(atl => atl.AnimalTypeId);
    [JsonPropertyName("visitedLocations")]
    public IEnumerable<long> VisitedLocationsIds => VisitedLocations.Select(vl => vl.Id);
}