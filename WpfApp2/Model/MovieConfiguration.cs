using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WpfApp2.Model
{
    /// <summary>
    /// Movie model configuration
    /// </summary>
    class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            // Primary key definition for database
            builder.HasKey(m => m.ID);
        }
    }
}
