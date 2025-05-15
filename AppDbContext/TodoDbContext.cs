


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Todo.Domain;
using Todo.Models;

namespace Todo.AppDbContext;


// TodoDbContext class inherits from DbContext
     public class TodoDbContext : DbContext
     {

        // DbSettings field to store the connection string
         private readonly DbSettings _dbsettings;

            // Constructor to inject the DbSettings model
         public TodoDbContext(IOptions<DbSettings> dbSettings)
         {
             _dbsettings = dbSettings.Value;
         }


        // DbSet property to represent the Todo table
        public DbSet<Domain.User> Users { get; set; }
        public DbSet<Domain.Todo> Todos { get; set; }

         // Configuring the database provider and connection string

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         {
             optionsBuilder.UseSqlServer(_dbsettings.ConnectionString);
         }

            // Configuring the model for the Todo entity
         protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
             
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>()
                .HasMany(u => u.Todos)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure unique constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
         }
     }