using Microsoft.EntityFrameworkCore;

namespace WpfApp2
{
    public class MovieContextOld : DbContext
    {
        public virtual DbSet<Producer> Producers { get; set; } = null!;
        public virtual DbSet<Movie> Movies { get; set; } = null!;
    }
}
