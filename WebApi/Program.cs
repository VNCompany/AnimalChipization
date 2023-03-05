using DataLayer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(
    options =>
    {
        options.JsonSerializerOptions.Converters.Add(new WebApi.DataConverters.DateTimeConverter());
        options.JsonSerializerOptions.Converters.Add(new WebApi.DataConverters.NullableDateTimeConverter());
    });
builder.Services.AddScoped(
    provider => new ApplicationContext(
        "Server=127.0.0.1;User=root;Password=root;Database=animal_chipization"));

var app = builder.Build();

app.MapControllers();

app.Run();