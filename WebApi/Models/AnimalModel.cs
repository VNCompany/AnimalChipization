using DataLayer.Entities;

namespace WebApi.Models
{

    public class AnimalModel : IModel<Animal>
    {
        public float? Weight { get; set; }
        public float? Length { get; set; }
        public float? Height { get; set; }
        public string? Gender { get; set; }
        public int? ChipperId { get; set; }
        public long? ChippingLocationId { get; set; }

        public virtual Animal ToEntity(Animal entity)
        {
            entity.Weight = Weight ?? throw new ArgumentNullException(nameof(Weight));
            entity.Length = Length ?? throw new ArgumentNullException(nameof(Length));
            entity.Height = Height ?? throw new ArgumentNullException(nameof(Height));
            entity.Gender = Gender ?? throw new ArgumentNullException(nameof(Gender));
            entity.ChipperId = ChipperId ?? throw new ArgumentNullException(nameof(ChipperId));
            entity.ChippingLocationId = ChippingLocationId ?? throw new ArgumentNullException(nameof(ChippingLocationId));
            return entity;
        }

        static readonly string[] __genders = { "MALE", "FEMALE", "OTHER" };
        public virtual bool Validate()
            => Weight != null && Length != null && Height != null 
            && ChipperId != null && ChippingLocationId != null
            && Weight > 0 && Length > 0 && Height > 0
            && ChipperId > 0 && ChippingLocationId > 0
            && Gender != null && __genders.Contains(Gender);
    }
}
