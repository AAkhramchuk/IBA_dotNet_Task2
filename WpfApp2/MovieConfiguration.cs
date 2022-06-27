using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WpfApp2
{
    /// <summary>
    /// Configuration Class for Entity Class Movie
    /// EFCore >= 2.0
    /// </summary>
    class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(m => m.ID);
            builder.Property(m => m.ID).ValueGeneratedNever();
            builder.Property(m => m.ProducerID)
                   .IsRequired()
                   .HasColumnType("Int");
        }
    }
}
