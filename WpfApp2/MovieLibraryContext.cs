using System;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace WpfApp2
{
    public class MovieLibraryContext : DbContext
    {
        const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=MovieLibrary;Trusted_Connection=True;";

        public MovieLibraryContext() : base() { }

        public MovieLibraryContext(DbContextOptions<MovieLibraryContext> options) : base(options) { }

        public DbSet<Producer> Producers { get; set; } = null!;
        public DbSet<Movie> Movies { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseSqlServer(connectionString
                    , providerOptions => { providerOptions.EnableRetryOnFailure(); });
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return SaveChanges();
        }

        public override int SaveChanges()
        {
            throw new NotImplementedException("Нужно сохранять асинхронно!");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
