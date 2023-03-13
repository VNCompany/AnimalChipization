using DataLayer.Entities;

namespace WebApi.Models
{
    public class AnimalPostModel : AnimalModel
    {
        public long?[]? AnimalTypes { get; set; }

        public override bool Validate()
            => AnimalTypes != null && AnimalTypes.Length > 0 && AnimalTypes.All(t => t != null && t > 0)
            && base.Validate();
    }
}
