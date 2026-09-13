using Microsoft.EntityFrameworkCore;
using OnboardingPlatform.Core.Models;



namespace OnboardingPlatform.Data.Implementations
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");

                entity.HasKey(e => e.CustomerId);
                entity.Property(e => e.CustomerId)
                    .HasColumnName("Id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName)
                    .HasColumnName("FirstName")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.MiddleName)
                    .HasColumnName("MiddleName")
                    .HasMaxLength(100)
                    .IsRequired(false)
                    .HasDefaultValue(string.Empty);

                entity.Property(e => e.LastName)
                    .HasColumnName("LastName")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.DateOfBirth)
                    .HasColumnName("DateOfBirth")
                    .HasColumnType("date")
                    .IsRequired();

                entity.Property(e => e.Gender)
                    .HasColumnName("Gender")
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("PhoneNumber")
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.Email)
                    .HasColumnName("Email")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.Nationality)
                    .HasColumnName("Nationality")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.BVN)
                    .HasColumnName("BVN")
                    .HasMaxLength(11)
                    .IsRequired();

                entity.Property(e => e.Password)
                    .HasColumnName("Password")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasColumnName("Status")
                    .HasMaxLength(50)
                    .IsRequired();

                // Logging / timestamps
                entity.Property(e => e.CreatedAt)
                    .HasColumnName("CreatedAt")
                    .HasColumnType("datetime")
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("UpdatedAt")
                    .HasColumnType("datetime")
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAddOrUpdate();
            });
        }
    }
}
