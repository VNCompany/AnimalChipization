using DataLayer;
using WebApi.Services;

string mySqlServer = Environment.GetEnvironmentVariable("DB_SERVER") ?? throw new ArgumentNullException("DB_SERVER");
string mySqlUser = Environment.GetEnvironmentVariable("DB_USER") ?? throw new ArgumentNullException("DB_USER");
string mySqlPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new ArgumentNullException("DB_PASSWORD");
string mySqlDatabase = Environment.GetEnvironmentVariable("DB_DATABASE") ?? throw new ArgumentNullException("DB_DATABASE");

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(
    options =>
    {
        options.JsonSerializerOptions.Converters.Add(new WebApi.Converters.DateTimeConverter());
        options.JsonSerializerOptions.Converters.Add(new WebApi.Converters.NullableDateTimeConverter());
    });

builder.Services.AddScoped(
    provider => new ApplicationContext(
        $"Server={mySqlServer};User={mySqlUser};Password={mySqlPassword};Database={mySqlDatabase}"));

builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();

var app = builder.Build();

using (IServiceScope provider = app.Services.CreateScope())
{
    ApplicationContext context = provider.ServiceProvider.GetRequiredService<ApplicationContext>();
    context.Migrate();
}

app.MapControllers();

app.Run();