namespace dotnet_api_test.Persistence
{
    public static class Seed
    {
        public static void PopulateDb(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
            if (context != null) SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>() ?? throw new InvalidOperationException());
        }

        private static void SeedData(AppDbContext context)
        {
            if (!context.Dishes.Any())
            {
                Console.WriteLine("Seeding data...");

                context.Dishes.AddRange(
                    new Dish {Name = "Pancakes", MadeBy = "John Doe", Cost = 12},
                    new Dish {Name = "Chicken with curry", MadeBy = "Emil Johansson", Cost = 15},
                    new Dish {Name = "Hamburger with fries", MadeBy = "Emilia Nilsson", Cost = 17},
                    new Dish {Name = "Spaghetti Bolognese", MadeBy = "Johan Jonsson", Cost = 11}
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Data already exist");
            }
        }
    }
}