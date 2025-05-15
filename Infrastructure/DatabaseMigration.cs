using Microsoft.EntityFrameworkCore;
using Todo.AppDbContext;


namespace Todo.Infrastructure
{
    public static class DatabaseMigrationManager
    {
        public static void MigrateDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<TodoDbContext>();
                    
                    // Apply migrations if any, or create database if it doesn't exist
                    context.Database.Migrate();
                    
                    // Seed the database if needed
                    // SeedDatabase(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
        }
        
        // private static void SeedDatabase(TodoDbContext context)
        // {
        //     // Add seed data if needed
        // }
    }
}