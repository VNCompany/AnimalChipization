using DataLayer;
using DataLayer.Entities;

namespace WebApi.ViewModels;

public class VisitedLocationsViewModel
{
    public static VisitedLocationsViewModel? Build(ApplicationContext context, long animalId)
    {
        Animal? animal = context.Animals.FirstOrDefault(a => a.Id == animalId);
        return animal is null ? null : new VisitedLocationsViewModel(context, animal);
    }

    private readonly Animal animal;
    private VisitedLocationsViewModel(ApplicationContext context, Animal animal)
    {
        this.animal = animal;
        VisitedLocations = context.VisitedLocations.Where(vl => vl.AnimalId == animal.Id).OrderBy(vl2 => vl2.DateTimeOfVisitLocationPoint).ToList();
    }

    public List<VisitedLocation> VisitedLocations { get; private set; }
    public Animal Animal => animal;

    public VisitedLocation? LastVisitedLocation => VisitedLocations.LastOrDefault();
}