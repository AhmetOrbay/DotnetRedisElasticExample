using Microsoft.EntityFrameworkCore;
using Turkai.Model.Entity;

namespace Turkai.Data
{
    public class TurkaiDbContext :DbContext
    {
        public TurkaiDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<Product> Products { get; set; }
    }
}