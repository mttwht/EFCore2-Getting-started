using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SamuraiApp.Domain;

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

            //// mapping shadow property; mullable foreign key
            //modelBuilder.Entity<Samurai>()
            //            .HasOne(s => s.SecretIdentity)
            //            .WithOne(i => i.Samurai)
            //            //.IsRequired() // SecretIdentity must always have a Samurai
            //            .HasForeignKey<SecretIdentity>("SamuraiId");
        }

    }
}
