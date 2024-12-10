using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformService", Version = "v1" });
});

// Add Controllers
builder.Services.AddControllers();

// Register HTTP client and messaging services
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

// Add gRPC support
builder.Services.AddGrpc();

// Configure database context based on environment
if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using SQL Server database");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
}
else
{
    Console.WriteLine("--> Using InMemory database");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseInMemoryDatabase("InMem"));
}

// Dependency Injection for Repository
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Log CommandService endpoint
Console.WriteLine($"--> CommandService Endpoint: {builder.Configuration["CommandService"]}");

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService v1"));
}

// app.UseHttpsRedirection();

// Map routes directly
app.MapControllers(); 
app.MapGrpcService<GrpcPlatformService>(); 

// Serve the proto file directly as a route
app.MapGet("/protos/platforms.proto", async context =>
{
    await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
});

// Seed the database
PrepDb.PrepPopulation(app, app.Environment.IsProduction());

app.Run();
