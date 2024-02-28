using Learning_platform.Entities;
using Learning_platform.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Learning_platform.DBContexts
{
    public class TutoringPlatformContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<User> User { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Tutor> Tutor { get; set; }
        //public DbSet<Message> Messages { get; set; }
        public DbSet<ApplicationUser> AspNetUsers { get; set; }
        public DbSet<ApplicationUser> AspNetRoles { get; set; }




        public TutoringPlatformContext(DbContextOptions<TutoringPlatformContext> options) : base(options)
        {
        }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Tutor>().ToTable("Tutor");

            // Configure common properties in base User entity
            modelBuilder.Entity<User>().HasKey(u => u.Id);


            base.OnModelCreating(modelBuilder);
        }
    }
}
