using Microsoft.EntityFrameworkCore;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.Server.Core.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication MigrateEFDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();

            var pendingMigrations = context.Database.GetPendingMigrations();

            if (pendingMigrations.Any())
            {
                Console.WriteLine($"\n{pendingMigrations.Count()} pending migration(s) found.");
                Console.WriteLine($"Applying migrations...\n");

                try
                {
                    context.Database.Migrate();

                    var lastAppliedMigration = context.Database.GetAppliedMigrations().LastOrDefault();
                    Console.WriteLine($"\nCurrent schema version: {lastAppliedMigration}\n");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error has occurred during migration.");
                    throw;
                }
            }

            return app;
        }

        public static WebApplication SeedTestData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            if (context.Activities.Any()) return app;

            var activities = new List<Activity>
            {
                new Activity
                {
                    Title = "Past Activity 1",
                    Date = DateTime.UtcNow.AddMonths(-2),
                    Description = "Activity 2 months ago",
                    Category = "drinks",
                    City = "London",
                    Venue = "Pub",
                },
                new Activity
                {
                    Title = "Past Activity 2",
                    Date = DateTime.UtcNow.AddMonths(-1),
                    Description = "Activity 1 month ago",
                    Category = "culture",
                    City = "Paris",
                    Venue = "Louvre",
                },
                new Activity
                {
                    Title = "Future Activity 1",
                    Date = DateTime.UtcNow.AddMonths(1),
                    Description = "Activity 1 month in future",
                    Category = "culture",
                    City = "London",
                    Venue = "Natural History Museum",
                },
                new Activity
                {
                    Title = "Future Activity 2",
                    Date = DateTime.UtcNow.AddMonths(2),
                    Description = "Activity 2 months in future",
                    Category = "music",
                    City = "London",
                    Venue = "O2 Arena",
                },
                new Activity
                {
                    Title = "Future Activity 3",
                    Date = DateTime.UtcNow.AddMonths(3),
                    Description = "Activity 3 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Another pub",
                },
                new Activity
                {
                    Title = "Future Activity 4",
                    Date = DateTime.UtcNow.AddMonths(4),
                    Description = "Activity 4 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Yet another pub",
                },
                new Activity
                {
                    Title = "Future Activity 5",
                    Date = DateTime.UtcNow.AddMonths(5),
                    Description = "Activity 5 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Just another pub",
                },
                new Activity
                {
                    Title = "Future Activity 6",
                    Date = DateTime.UtcNow.AddMonths(6),
                    Description = "Activity 6 months in future",
                    Category = "music",
                    City = "London",
                    Venue = "Roundhouse Camden",
                },
                new Activity
                {
                    Title = "Future Activity 7",
                    Date = DateTime.UtcNow.AddMonths(7),
                    Description = "Activity 2 months ago",
                    Category = "travel",
                    City = "London",
                    Venue = "Somewhere on the Thames",
                },
                new Activity
                {
                    Title = "Future Activity 8",
                    Date = DateTime.UtcNow.AddMonths(8),
                    Description = "Activity 8 months in future",
                    Category = "film",
                    City = "London",
                    Venue = "Cinema",
                }
            };

            context.Activities.AddRange(activities);
            context.SaveChanges();

            return app;
        }
    }
}
