using System.Text.Json.Serialization;
using DataLayer;
using WebApi.Services;

DbConnectionParams dbConnectionParams;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
    dbConnectionParams = new DbConnectionParams(
        "localhost", 
        "postgres", 
        "admin", 
        "AnimalChipization");
else
    dbConnectionParams = new DbConnectionParams(
        host: Environment.GetEnvironmentVariable("DB_SERVER")
              ?? throw new ArgumentNullException("DB_SERVER"),
        user: Environment.GetEnvironmentVariable("DB_USER")
              ?? throw new ArgumentNullException("DB_USER"),
        password: Environment.GetEnvironmentVariable("DB_PASSWORD")
                  ?? throw new ArgumentNullException("DB_PASSWORD"),
        database: Environment.GetEnvironmentVariable("DB_DATABASE")
                  ?? throw new ArgumentNullException("DB_DATABASE"));


builder.Services.AddControllers().AddJsonOptions(
    options =>
    {
        options.JsonSerializerOptions.Converters.Add(new WebApi.Converters.DateTimeConverter());
        options.JsonSerializerOptions.Converters.Add(new WebApi.Converters.NullableDateTimeConverter());
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddScoped(provider => new ApplicationContext(
    dbConnectionParams.ToString()));

builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();

var app = builder.Build();

using (IServiceScope provider = app.Services.CreateScope())
{
    ApplicationContext context = provider.ServiceProvider.GetRequiredService<ApplicationContext>();
    context.Migrate();
}

if (app.Environment.IsProduction())
{
    using (IServiceScope provider = app.Services.CreateScope())
    {
        ApplicationContext context = provider.ServiceProvider.GetRequiredService<ApplicationContext>();
        context.Migrate();
    }
}

app.MapControllers();
app.Run();
