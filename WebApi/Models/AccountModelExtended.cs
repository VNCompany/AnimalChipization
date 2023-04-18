using DataLayer.Entities;

namespace WebApi.Models;

public class AccountModelExtended : AccountModel
{
    public static readonly string[] ValidRoleValues = { "ADMIN", "CHIPPER", "USER" };
    
    public string? Role { get; set; }

    public override bool Validate()
    {
        return base.Validate()
            && Role != null
            && ValidRoleValues.Contains(Role);
    }

    public override Account ToEntity(Account entity)
    {
        entity.Role = (Role)Enum.Parse(typeof(Role), Role ?? throw new ArgumentNullException(nameof(Role)));
        return base.ToEntity(entity);
    }
}