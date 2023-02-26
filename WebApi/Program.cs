using DataLayer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped(
    provider => new ApplicationContext(
        "Server=127.0.0.1;User=root;Password=root;Database=animal_chipization"));

var app = builder.Build();

app.MapControllers();

app.Run();