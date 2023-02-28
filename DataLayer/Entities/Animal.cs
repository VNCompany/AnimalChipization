namespace DataLayer.Entities;

public class Animal
{
    public long Id { get; set; }
    public float Weight { get; set; }
    public float Length { get; set; }
    public float Height { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string LifeStatis { get; set; } = string.Empty;
    public DateTime? ChippingDateTime { get; set; }
    public int ChipperId { get; set; }
    public long ChippingLocationId { get; set; }

    public long[]? AnimalTypes { get; set; }
    public long[]? VisitedLocations { get; set; }
}