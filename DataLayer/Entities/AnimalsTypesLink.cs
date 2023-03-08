namespace DataLayer.Entities;

public class AnimalsTypesLink
{
    public long AnimalId { get; set; }
    public Animal Animal { get; set; } = null!;
    public long AnimalTypeId { get; set; }
    public AnimalType AnimalType { get; set; } = null!;
}