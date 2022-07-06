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
    /// Configuration Class for Entity Class Producer
    /// EFCore >= 2.0
    /// </summary>
    class ProducerConfiguration : IEntityTypeConfiguration<Producer>
    {
        public void Configure(EntityTypeBuilder<Producer> builder)
        {
            builder.HasKey(p => p.ID);
            builder.Property(p => p.ID);//.ValueGeneratedNever();
            builder.HasMany(p => p.Movies)
                   .WithOne(m => m.Producer)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
        }
    }
}
