using Microsoft.EntityFrameworkCore;
using OnboardingPlatform.Core.Models;
using OnboardingPlatform.Core.Enums;



namespace OnboardingPlatform.Data.Implementations
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Anu's tables
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<CustomerIdentifier> CustomerIdentifiers => Set<CustomerIdentifier>();
        public DbSet<CustomerAddress> CustomerAddresses => Set<CustomerAddress>();
        public DbSet<CustomerProduct> CustomerProducts => Set<CustomerProduct>();

        // Emmanuel's tables — declared here so EF knows the full graph, but Emmanuel owns the logic
        public DbSet<DraftApplication> DraftApplications => Set<DraftApplication>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ── Products ──────────────────────────────────────────────
            modelBuilder.Entity<Product>(e =>
            {
                e.HasKey(p => p.ProductId);
                e.HasIndex(p => p.ProductCode).IsUnique();
                e.Property(p => p.ProductCode).HasConversion<string>();
            });

            // ── Customers ─────────────────────────────────────────────
            modelBuilder.Entity<Customer>(e =>
            {
                e.HasKey(c => c.CustomerId);
                e.Property(c => c.Status).HasConversion<string>();
                e.HasMany(c => c.Identifiers)
                 .WithOne(i => i.Customer)
                 .HasForeignKey(i => i.CustomerId);
                e.HasMany(c => c.Addresses)
                 .WithOne(a => a.Customer)
                 .HasForeignKey(a => a.CustomerId);
            });

            // ── CustomerIdentifiers ───────────────────────────────────
            modelBuilder.Entity<CustomerIdentifier>(e =>
            {
                e.HasKey(i => i.IdentifierId);
                // THIS is the dedup constraint — same BVN cannot register twice
                e.HasIndex(i => new { i.IdentifierType, i.IdentifierValueHash }).IsUnique();
                e.Property(i => i.IdentifierType).HasConversion<string>();
            });

            // ── CustomerAddress ───────────────────────────────────────
            modelBuilder.Entity<CustomerAddress>(e =>
            {
                e.HasKey(a => a.AddressId);
            });

            // ── CustomerProducts ──────────────────────────────────────
            modelBuilder.Entity<CustomerProduct>(e =>
            {
                e.HasKey(cp => cp.CustomerProductId);
                // One product per customer (for MVP)
                e.HasIndex(cp => new { cp.CustomerId, cp.ProductId }).IsUnique();
                e.Property(cp => cp.Status).HasConversion<string>();
                e.HasOne(cp => cp.Customer)
                 .WithMany(c => c.CustomerProducts)
                 .HasForeignKey(cp => cp.CustomerId);
                e.HasOne(cp => cp.Product)
                 .WithMany(p => p.CustomerProducts)
                 .HasForeignKey(cp => cp.ProductId);
            });

            modelBuilder.Entity<DraftApplication>(e =>
            {
                e.HasKey(d => d.DraftId);

                e.Property(d => d.PrimaryIdentifierType)
                    .HasConversion<string>();

                e.Property(d => d.SecondaryIdentifierType)
                    .HasConversion<string>();

                e.Property(d => d.CurrentStep)
                    .HasConversion<string>();

                e.Property(d => d.Channel)
                    .HasConversion<string>();

                e.Property(d => d.Status)
                    .HasConversion<string>();

                e.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId);

                e.HasOne(d => d.Customer)
                    .WithMany()
                    .HasForeignKey(d => d.CustomerId)
                    .IsRequired(false);
            });

            // ── Seed Products ─────────────────────────────────────────
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = Guid.Parse("aaaaaaaa-0001-0001-0001-aaaaaaaaaaaa"), ProductCode = ProductCode.SAVINGS, ProductName = "Savings Account", RequiredIdentifiers = "[\"BVN\"]", IsActive = true, CreatedAt = DateTime.Now },
                new Product { ProductId = Guid.Parse("aaaaaaaa-0002-0002-0002-aaaaaaaaaaaa"), ProductCode = ProductCode.CURRENT, ProductName = "Current Account", RequiredIdentifiers = "[\"BVN\"]", IsActive = true, CreatedAt = DateTime.Now },
                new Product { ProductId = Guid.Parse("aaaaaaaa-0003-0003-0003-aaaaaaaaaaaa"), ProductCode = ProductCode.PENSION_RSA, ProductName = "Pension (RSA)", RequiredIdentifiers = "[\"NIN\",\"PHONE\"]", IsActive = true, CreatedAt = DateTime.Now },
                new Product { ProductId = Guid.Parse("aaaaaaaa-0004-0004-0004-aaaaaaaaaaaa"), ProductCode = ProductCode.STOCKBROKING, ProductName = "Stockbroking", RequiredIdentifiers = "[\"EMAIL\"]", IsActive = true, CreatedAt = DateTime.Now },
                new Product { ProductId = Guid.Parse("aaaaaaaa-0005-0005-0005-aaaaaaaaaaaa"), ProductCode = ProductCode.INSURANCE, ProductName = "Insurance", RequiredIdentifiers = "[\"EMAIL\",\"PHONE\"]", IsActive = true, CreatedAt = DateTime.Now }
            );
        }
    }
}
