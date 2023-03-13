using DataLayer.Entities;

namespace WebApi.Models
{
    public class AnimalPutModel : AnimalModel
    {
        public string? LifeStatus { get; set; }

        public override Animal ToEntity(Animal entity)
        {
            entity.LifeStatus = LifeStatus ?? throw new ArgumentNullException(nameof(LifeStatus));
            return base.ToEntity(entity);
        }

        static readonly string[] __lifeStatuses = { "ALIVE", "DEAD" };
        public override bool Validate()
        {
            return base.Validate() && LifeStatus != null && __lifeStatuses.Contains(LifeStatus);
        }
    }
}
