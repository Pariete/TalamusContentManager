using Microsoft.EntityFrameworkCore;
using Talamus_ContentManager.Models;

namespace Talamus_ContentManager
{
    public class TalamusContext : DbContext
    {
        public TalamusContext(DbContextOptions<TalamusContext> options)
         : base(options)
        {
            // Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity(typeof(Subsequent))
            .HasOne(typeof(Part), "Part")
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<Part> Parts { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Subsequent> Subsequents { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
