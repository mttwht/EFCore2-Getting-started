using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SamuraiApp.Domain;
using System;
using System.Linq;

namespace SamuraiApp.Data
{
    public class SamuraiContext:DbContext
    {

        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }

        public static readonly LoggerFactory MyConsoleLoggerFactory = new LoggerFactory(new[] {
            new ConsoleLoggerProvider((category, level)
                => category==DbLoggerCategory.Database.Command.Name
                && level==LogLevel.Information, true)
        });

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            optionsBuilder
                .UseLoggerFactory(MyConsoleLoggerFactory) // log sql queries
                .EnableSensitiveDataLogging(true) // show parameters in log
                .UseSqlServer("Server=(localdb)\\mssqllocaldb; Database=SamuraiAppData; Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>().HasKey(s => new { s.SamuraiId, s.BattleId });

            modelBuilder.Entity<Battle>().Property(b => b.StartDate).HasColumnType("Date");
            modelBuilder.Entity<Battle>().Property(b => b.EndDate).HasColumnType("Date");

            //modelBuilder.Entity<Samurai>().Property<DateTime>("CreatedAt");
            //modelBuilder.Entity<Samurai>().Property<DateTime>("UpdatedAt");

            //// mapping shadow property; mullable foreign key
            //modelBuilder.Entity<Samurai>()
            //            .HasOne(s => s.SecretIdentity)
            //            .WithOne(i => i.Samurai)
            //            //.IsRequired() // SecretIdentity must always have a Samurai
            //            .HasForeignKey<SecretIdentity>("SamuraiId");

            //modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName);
            //modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName).ToTable("SamuraiNames"); // Sends new columns to a separate table
            modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName).Property(n => n.GivenName).HasColumnName("GivenName");
            modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName).Property(n => n.Surname).HasColumnName("Surname");

            foreach(var entityType in modelBuilder.Model.GetEntityTypes().Where(e=>!e.IsOwned())) {
                modelBuilder.Entity(entityType.Name).Property<DateTime>("CreatedAt");
                modelBuilder.Entity(entityType.Name).Property<DateTime>("UpdatedAt");
            }

        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            var timestamp = DateTime.Now;

            // For EFCore 2.0/2.1 needing owned types to always be instantiated
            #region Workaround
            var entries = ChangeTracker.Entries().Where(e => e.Entity is Samurai).ToList();
            foreach(var entry in entries) {
                if(entry.Reference("BetterName").CurrentValue == null)
                    entry.Reference("BetterName").CurrentValue = PersonName.Empty();
                entry.Reference("BetterName").TargetEntry.State = entry.State;
            }
            #endregion

            foreach(var entry in ChangeTracker.Entries()
                    .Where(e=>(e.State==EntityState.Added || e.State==EntityState.Modified) && !e.Metadata.IsOwned()) ) {
                entry.Property("UpdatedAt").CurrentValue = timestamp;
                if(entry.State == EntityState.Added)
                    entry.Property("CreatedAt").CurrentValue = timestamp;
            }
            return base.SaveChanges();
        }
    }
}
