using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Reflection;

namespace WpfApp2.Model
{
    /// <summary>
    /// Movie data context definition
    /// </summary>
    public class MovieContext : DbContext
    {
        /// <summary>
        /// Connection string
        /// </summary>
        public readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        
        public MovieContext() : base() { }

        public MovieContext(DbContextOptions<MovieContext> options) : base(options) { }

        /// <summary>
        /// Used to query and save Movie instances
        /// </summary>
        public DbSet<Movie> Movies { get { return Set<Movie>(); } set { } }

        /// <summary>
        /// Configuring a database connection
        /// </summary>
        /// <param name="optionsBuilder"></param>
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

        /// <summary>
        /// Configuring construction of a model
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
