using Learning_platform.Models;
using Microsoft.EntityFrameworkCore;

namespace Learning_platform.DBContexts
{
    public class TutoringPlatformContext : DbContext
    {
        public DbSet<UserDto> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Message> Messages { get; set; }

   
        public TutoringPlatformContext(DbContextOptions<TutoringPlatformContext> options) : base(options)
        {
        }

  


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("your_connection_string_here");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasOne(m => m.ContentType)
                .WithMany()
                .IsRequired();

            modelBuilder.Entity<UserDto>()
                .HasOne(u => u.UserType)
                .WithMany()
                .IsRequired();

            modelBuilder.Entity<Tutor>()
                .HasBaseType<UserDto>();

            modelBuilder.Entity<Student>()
                .HasBaseType<UserDto>();

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
