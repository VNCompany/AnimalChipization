namespace WebApi;

public class AuthorizeAttribute : Attribute
{
    public string[]? RequiredRoles { get; set; }

    public AuthorizeAttribute() {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requiredRoles">Роли, которым доступен метод</param>
    public AuthorizeAttribute(params string[] requiredRoles)
    {
        RequiredRoles = requiredRoles;
    }
}