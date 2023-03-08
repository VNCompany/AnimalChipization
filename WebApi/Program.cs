using DataLayer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(
    options =>
    {
        options.JsonSerializerOptions.Converters.Add(new WebApi.Converters.DateTimeConverter());
        options.JsonSerializerOptions.Converters.Add(new WebApi.Converters.NullableDateTimeConverter());
    });
builder.Services.AddScoped(
    provider => new ApplicationContext(
        "Server=127.0.0.1;User=root;Password=root;Database=AnimalChipization"));

var app = builder.Build();

app.MapControllers();

app.Run();