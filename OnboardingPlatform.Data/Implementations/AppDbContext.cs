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
        public DbSet<ConsentLog> ConsentLogs => Set<ConsentLog>();
        public DbSet<CurrentAccountDetail> CurrentAccountDetails => Set<CurrentAccountDetail>();
        public DbSet<SavingsAccountDetail> SavingsAccountDetails => Set<SavingsAccountDetail>();
        public DbSet<PensionAccountDetail> PensionAccountDetails => Set<PensionAccountDetail>();
        public DbSet<SecurityCheck> SecurityChecks => Set<SecurityCheck>();
        public DbSet<StockBrokingAccountDetail> StockBrokingAccountDetails => Set<StockBrokingAccountDetail>();

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
                e.Property(c => c.Email)
                   .HasMaxLength(200)
                    .IsRequired(false);
                e.Property(c => c.PhoneNumber)
                    .HasMaxLength(20)
                    .IsRequired(false);
                e.Property(c => c.Status).HasConversion<string>();
                e.HasMany(c => c.Identifiers)
                 .WithOne(i => i.Customer)
                 .HasForeignKey(i => i.CustomerId);
                //e.HasMany(c => c.Addresses)
                 //.WithOne(a => a.Customer)
                 //.HasForeignKey(a => a.CustomerId);
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

            // ── DraftApplications ──────────────────────────────────────
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
            // ── Seed Products ─────────────────────────────────────────
            modelBuilder.Entity<Product>().HasData(
            new Product
            {
                ProductId = Guid.Parse("aaaaaaaa-0001-0001-0001-aaaaaaaaaaaa"),
                ProductCode = ProductCode.SAVINGS,
                ProductName = "Savings Account",
                RequiredIdentifiers = "[\"BVN\"]",
                AdditionalFieldsSchema = """
                [
                  {
                    "field": "branchPreference",
                    "label": "Preferred branch",
                    "type": "text",
                    "required": false
                  }
                ]
                """,
                IsActive = true,
                CreatedAt = DateTime.Now
            },

            new Product
            {
              ProductId = Guid.Parse("aaaaaaaa-0002-0002-0002-aaaaaaaaaaaa"),
              ProductCode = ProductCode.CURRENT,
              ProductName = "Current Account",
              RequiredIdentifiers = "[\"BVN\"]",
              AdditionalFieldsSchema = null,
              IsActive = true,
              CreatedAt = DateTime.Now
            },

            new Product
            {
              ProductId = Guid.Parse("aaaaaaaa-0003-0003-0003-aaaaaaaaaaaa"),
              ProductCode = ProductCode.PENSION_RSA,
              ProductName = "Pension (RSA)",
              RequiredIdentifiers = "[\"BVN\",\"NIN\"]",
              AdditionalFieldsSchema = """
                [
                  {
                    "field": "employerName",
                    "label": "Employer name",
                    "type": "text",
                    "required": false
                  },
                  {
                    "field": "contributionScheme",
                    "label": "Contribution scheme",
                    "type": "select",
                    "options": [
                      "MandatoryCPS",
                      "Voluntary",
                      "MicroPensionPlan"
                    ],
                    "required": true
                  }
                ]
                """,
                IsActive = true,
                CreatedAt = DateTime.Now
            },

            new Product
            {
              ProductId = Guid.Parse("aaaaaaaa-0004-0004-0004-aaaaaaaaaaaa"),
              ProductCode = ProductCode.STOCKBROKING,
              ProductName = "Stockbroking",
              RequiredIdentifiers = "[\"BVN\",\"EMAIL\"]",
              AdditionalFieldsSchema = null,
              IsActive = true,
              CreatedAt = DateTime.Now
            },

            new Product
            {
              ProductId = Guid.Parse("aaaaaaaa-0005-0005-0005-aaaaaaaaaaaa"),
              ProductCode = ProductCode.INSURANCE,
              ProductName = "Insurance",
              RequiredIdentifiers = "[\"BVN\",\"PHONE\"]",
              AdditionalFieldsSchema = null,
              IsActive = true,
              CreatedAt = DateTime.Now
            }
            );

            // ── Security Check ─────────────────────────────────────────
            modelBuilder.Entity<SecurityCheck>(b =>
            {
                b.HasKey(s => s.SecurityCheckId);
                b.Property(s => s.CheckType).HasConversion<string>().HasMaxLength(30);
                b.Property(s => s.Status).HasConversion<string>().HasMaxLength(20);

                b.HasOne(s => s.DraftApplication)
                    .WithMany()
                    .HasForeignKey(s => s.DraftId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Consent Logs ─────────────────────────────────────────
            modelBuilder.Entity<ConsentLog>(e =>
            {
                e.HasKey(c => c.ConsentId);
                e.Property(c => c.Channel).HasConversion<string>();

                e.HasOne(c => c.Customer)
                    .WithMany()
                    .HasForeignKey(c => c.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(c => c.Product)
                    .WithMany()
                    .HasForeignKey(c => c.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Current Account Details ──────────────────────────────
            modelBuilder.Entity<CurrentAccountDetail>(e =>
            {
                e.HasKey(c => c.CurrentAccountId);
                e.Property(c => c.Status).HasConversion<string>();
                e.Property(c => c.AccountNumber).HasMaxLength(10);
                e.Property(c => c.Currency).HasMaxLength(3);
                e.Property(c => c.Balance).HasColumnType("decimal(18,2)");

                e.HasOne(c => c.CustomerProduct)
                    .WithMany()
                    .HasForeignKey(c => c.CustomerProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Savings Account Details ──────────────────────────────
            modelBuilder.Entity<SavingsAccountDetail>(e =>
            {
                e.HasKey(s => s.SavingsAccountId);
                e.Property(s => s.Status).HasConversion<string>();
                e.Property(s => s.AccountNumber).HasMaxLength(10);
                e.Property(s => s.Currency).HasMaxLength(3);
                e.Property(s => s.Balance).HasColumnType("decimal(18,2)");

                e.HasOne(s => s.CustomerProduct)
                    .WithMany()
                    .HasForeignKey(s => s.CustomerProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Pension Account Details ──────────────────────────────
            modelBuilder.Entity<PensionAccountDetail>(e =>
            {
                e.HasKey(p => p.PensionAccountId);
                e.Property(p => p.Status).HasConversion<string>();
                e.Property(p => p.ContributionScheme).HasConversion<string>();

                e.HasOne(p => p.CustomerProduct)
                    .WithMany()
                    .HasForeignKey(p => p.CustomerProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Stock Broking Account Details ────────────────────────
            modelBuilder.Entity<StockBrokingAccountDetail>(e =>
            {
                e.HasKey(s => s.StockBrokingAccountId);
                e.Property(s => s.Status).HasConversion<string>();

                e.HasOne(s => s.CustomerProduct)
                    .WithMany()
                    .HasForeignKey(s => s.CustomerProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
