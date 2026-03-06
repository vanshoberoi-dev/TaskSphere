using Microsoft.EntityFrameworkCore;
using TS.Model.Domain;

namespace TS.Model.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CommentEntity>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}