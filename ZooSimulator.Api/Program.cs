using ZooSimulator.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<IRandomValue, RandomValue>();
builder.Services.AddSingleton<Zoo>();
builder.Services.AddHostedService<SimulationService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
