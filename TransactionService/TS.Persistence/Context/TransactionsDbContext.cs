using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TS.Contracts.Persistence;
using TS.Domain.Entities;
using TS.Persistence.Settings;

namespace TS.Persistence.Context;

public partial class TransactionsDbContext : DbContext, ITransactionsDbContext
{
    private readonly StorageSettings _settings;
    public TransactionsDbContext(StorageSettings settings)
    {
        _settings = settings;
        ChangeTracker.QueryTrackingBehavior =
            QueryTrackingBehavior.NoTracking; //once we need to track an object we overright by using AsTracking
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql($"{_settings.DefaultConnection}");
    }
//     public TransactionsDbContext()
//     {
//     }
//
//     public TransactionsDbContext(DbContextOptions<TransactionsDbContext> options)
//         : base(options)
//     {
//     }
//     
//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//         => optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Username=tatiana;Password=123;Database=TransactionsDB");

    public virtual DbSet<Account> Account { get; set; }

    public virtual DbSet<Currency> Currency { get; set; }

    public virtual DbSet<RecurrentTransaction> RecurrentTransaction { get; set; }

    public virtual DbSet<Transaction> Transaction { get; set; }

    public virtual DbSet<Transactiontype> Transactiontype { get; set; }
    public virtual DbSet<PeriodType> PeriodType { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("accounts_pkey");

            entity.ToTable("account");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Guid).HasColumnName("Guid");
            entity.Property(e => e.BranchId).HasColumnName("branch_id");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("currency_pkey");

            entity.ToTable("currency");

            entity.HasIndex(e => e.CurrencyCode, "currency_currency_code_key").IsUnique();

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(3)
                .HasColumnName("currency_code");
            entity.Property(e => e.CurrencyName)
                .HasMaxLength(50)
                .HasColumnName("currency_name");
        });

        modelBuilder.Entity<RecurrentTransaction>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("recurrent_transactions_pkey");

            entity.ToTable("recurrent_transaction");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn(); // PostgreSQL sequence for auto-incrementing

            entity.Property(e => e.TransactionId)
                .HasColumnName("transaction_id");

            entity.Property(e => e.PeriodTypeId) // Corrected property name
                .HasColumnName("period_type_id"); // Ensure column name matches your database schema

            entity.Property(e => e.NextExecutionDate)
                .HasColumnType("timestamp without time zone")
                .IsRequired(false);

            entity.HasOne(d => d.Transaction)
                .WithMany(p => p.RecurrentTransaction)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("recurrent_transactions_transactions_fk");

            entity.HasOne(rt => rt.PeriodType)
                .WithMany()
                .HasForeignKey(rt => rt.PeriodTypeId)
                .HasConstraintName("recurrent_transactions_period_types_fk"); 
        });


        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transactions_pkey");

            entity.ToTable("transaction");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Amount)
                .HasPrecision(15, 2)
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CurrencyId).HasColumnName("currency_id");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.TransactionTypeId).HasColumnName("transaction_type_id");

            entity.HasOne(d => d.Currency).WithMany(p => p.Transaction)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("transactions_currency_id_fkey");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.Transaction)
                .HasForeignKey(d => d.TransactionTypeId)
                .HasConstraintName("transactions_transaction_type_id_fkey");
            entity.HasOne(e => e.Account)
                .WithMany(a => a.Transaction)
                .HasForeignKey(e => e.AccountId);
        });

        modelBuilder.Entity<Transactiontype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transactiontypes_pkey");

            entity.ToTable("transactiontype");

            entity.HasIndex(e => e.TypeName, "transactiontypes_type_name_key").IsUnique();

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.TypeName)
                .HasMaxLength(20)
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<PeriodType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransactionsDbContext).Assembly);
        SeedData(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    public static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transactiontype>().HasData(GetTransactiontypeeSeeds());
        modelBuilder.Entity<Currency>().HasData(GetCurrencySeeds());
        modelBuilder.Entity<PeriodType>().HasData(GetPeriodTypeSeeds());
    }
    private static List<Transactiontype> GetTransactiontypeeSeeds()
    {
        // Return a list of Location seed data
        return new List<Transactiontype>
        {
            new Transactiontype { Id = 1, TypeName = "Deposit" },
            new Transactiontype { Id = 2, TypeName = "Withdrawal" },
            new Transactiontype { Id = 3, TypeName = "Transfer" }
        };
    }
        private static List<PeriodType> GetPeriodTypeSeeds()
        {

            return new List<PeriodType>
            {
                new PeriodType { Id = 1, Name = "Daily" },
                new PeriodType { Id = 2, Name = "Weekly" },
                new PeriodType { Id = 3, Name = "Monthly" },
                new PeriodType { Id = 4, Name = "Yearly" }
            };
        }
    private static List<Currency> GetCurrencySeeds()
    {
        return new List<Currency>
        {
            new Currency { Id = 1, CurrencyCode = "USD", CurrencyName = "US Dollar" },
            new Currency { Id = 2, CurrencyCode = "EUR", CurrencyName = "Euro" },
            new Currency { Id = 3, CurrencyCode = "LBP", CurrencyName = "Lebanese Pound" }
        };
    }
}
