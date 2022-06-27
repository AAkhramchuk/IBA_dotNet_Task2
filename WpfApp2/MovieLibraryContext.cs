using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Design;

namespace WpfApp2
{
    public class MovieLibraryContext : DbContext
    {
/*
        public MovieLibraryContext(DbContextOptions<MovieLibraryContext> options)
            : base(options)
        {
            //    ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
            //    ChangeTracker.DeleteOrphansTiming = CascadeTiming.OnSaveChanges;
        }
*/
        private readonly string _connectionString;

        public MovieLibraryContext(string connectionString)
        {
            _connectionString = connectionString;
        }
/*
        public MovieLibraryContext()
        {
        }
*/
        public DbSet<Producer> Producers { get; set; } = null!;
        public DbSet<Movie> Movies { get; set; } = null!;

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder)
        {
            //string connectionDB = @ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //optionsBuilder.UseSqlServer(connectionDB);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .EnableSensitiveDataLogging()
                    .UseSqlServer(_connectionString
                        , providerOptions => { providerOptions.EnableRetryOnFailure(); });
            }
        }
/*
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
*/
    }
}
