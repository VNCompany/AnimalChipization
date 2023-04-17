namespace WebApi.Internal;

internal class DbConnectionParams
{
    public string Host { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string Database { get; set; }

    public DbConnectionParams(string host, string user, string password, string database)
    {
        Host = host;
        User = user;
        Password = password;
        Database = database;
    }

    public override string ToString()
    {
        return $"Host={Host};Username={User};Password={Password};Database={Database}";
    }
}