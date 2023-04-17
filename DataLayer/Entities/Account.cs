using System.Text.Json.Serialization;

namespace DataLayer.Entities;

public class Account
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Role Role { get; set; } = default;
    [JsonIgnore]
    public string Password { get; set; } = null!;
}