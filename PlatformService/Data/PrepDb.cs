
public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app)
    {
        using(var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
        }
    }

    public static void SeedData(AppDbContext dbContext)
    {
        if(!dbContext.Platforms.Any())
        {
            Console.WriteLine("--> Seeding data...");

            dbContext.Platforms.AddRange(
                new Platform() {Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"},
                new Platform() {Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free"},
                new Platform() {Name = "Kubernetes", Publisher = "Cloud Native Computer Foundation", Cost = "Free"},
                new Platform() {Name = "Docker", Publisher = "Docker Hub", Cost = "Free"}
            );

            dbContext.SaveChanges();
        } else
        {
            Console.WriteLine("--> We already have data");
        }

    }
}