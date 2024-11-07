using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

// Check the environment and use the appropriate database
if (builder.Environment.IsProduction())
{
    System.Console.WriteLine("--> Using SQL database");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
}
else
{
    System.Console.WriteLine("--> Using InMemory database");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseInMemoryDatabase("InMem"));
}

// Register Dependency Injection Services
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

// Add AutoMapper Service
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

System.Console.WriteLine($"--> Command Service Endpoint: {builder.Configuration["CommandService"]}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.MapControllers();

// Seed App Data
PrepDb.PrepPopulation(app, app.Environment.IsProduction());

app.Run();