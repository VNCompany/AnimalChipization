namespace DataLayer.Entities;

public class Area
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Geometry { get; set; } = null!;
}