using System.Reflection;
using Common.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Persistence.Data;

public class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string workingDirectory = Environment.CurrentDirectory;
        string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
        string dbpath = Path.Combine(projectDirectory + "/src/Persistence");
        optionsBuilder.UseSqlite($"Data Source={dbpath}/database.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var accountHolders = modelBuilder.Entity<AccountHolder>();
        accountHolders.Property(u => u.Name)
            .HasColumnType("varchar")
            .HasConversion<string>()
            .HasMaxLength(50);

        accountHolders.Property(u => u.Surname)
            .HasColumnType("varchar")
            .HasConversion<string>()
            .HasMaxLength(50);
        accountHolders.Property(u => u.BSN)
            .HasColumnType("varchar")
            .HasConversion<string>()
            .HasMaxLength(50);

        accountHolders.Property(x => x.Id).HasColumnType("varchar").IsRequired();

        accountHolders.Property<DateTime>("CreateDate")
            .HasColumnType("datetime2");

        accountHolders.Property<DateTime>("ModifyDate")
            .HasColumnType("datetime2");
        accountHolders.HasMany(a => a.Accounts).WithOne(s => s.Holder).HasForeignKey(p => p.HolderId);

        var accounts = modelBuilder.Entity<Account>();
        accounts.Property(x => x.Balance).HasColumnType("decimal");
        accounts.Property(x => x.AccountNumber).HasColumnType("varchar").IsRequired();
        accounts.Property(x => x.IBAN).HasColumnType("varchar").HasConversion<string>().HasMaxLength(50).IsRequired();
        accounts.Property(x => x.HolderId).HasColumnType("int").IsRequired();


        accounts.Property(x => x.Id).HasColumnType("varchar").IsRequired();

        accounts.Property<DateTime>("CreateDate")
            .HasColumnType("datetime2");

        accounts.Property<DateTime>("ModifyDate")
            .HasColumnType("datetime2");

        var transactionFees = modelBuilder.Entity<TransactionFee>();
        transactionFees.Property(x=>x.Id).IsRequired().HasColumnType("varchar");
        transactionFees.Property(x=>x.Type).HasColumnType("int");
        transactionFees.Property(x=>x.Percentage).HasColumnType("decimal");
        transactionFees.Property(x=>x.CreateDate).HasColumnType("datetime2");
        transactionFees.Property(x=>x.ModifyDate).HasColumnType("datetime2");

        transactionFees.HasData(new TransactionFee
        {
            Id = new Guid().ToString(),
            Percentage = 1,
            Type = (int) TransactionFeeType.ACCOUNT_TOPUP,
            CreateDate = DateTime.Now,
            ModifyDate = DateTime.Now
        });

        var ibanInfos = modelBuilder.Entity<IBANStore>();
        transactionFees.Property(x=>x.Id).IsRequired().HasColumnType("int");

        ibanInfos.Property(u => u.IBAN)
            .HasColumnType("varchar")
            .HasConversion<string>()
            .HasMaxLength(18);

        ibanInfos.Property(u => u.AccountNumber)
            .HasColumnType("varchar")
            .HasConversion<string>()
            .HasMaxLength(10);

        ibanInfos.Property(u => u.IsActive)
            .HasColumnType("byte");

        modelBuilder.Entity<IBANStore>()
            .HasData(
                new IBANStore
                {
                    Id = 1,
                    IBAN = "NL21ABNA8261521222",
                    AccountNumber = "8261521222",
                    IsActive = false
                },
                new IBANStore
                {
                    Id = 2,
                    IBAN = "NL18RABO4883846911",
                    AccountNumber = "4883846911",
                    IsActive = false
                },
                new IBANStore
                {
                    Id = 3,
                    IBAN = "NL35ABNA3767744449",
                    AccountNumber = "3767744449",
                    IsActive = false
                },
                new IBANStore
                {
                    Id = 4,
                    IBAN = "NL22RABO1011562413",
                    AccountNumber = "1011562413",
                    IsActive = false
                },
                new IBANStore
                {
                    Id = 5,
                    IBAN = "NL60INGB5251802137",
                    AccountNumber = "5251802137",
                    IsActive = false
                },
                new IBANStore
                {
                    Id = 6,
                    IBAN = "NL34INGB4520711568",
                    AccountNumber = "4520711568",
                    IsActive = false
                },
                new IBANStore
                {
                    Id = 7,
                    IBAN = "NL64INGB1008270121",
                    AccountNumber = "1008270121",
                    IsActive = false
                },
                new IBANStore
                {
                    Id = 8,
                    IBAN = "NL77ABNA3403751775",
                    AccountNumber = "3403751775",
                    IsActive = false
                },
                new IBANStore
                {
                    Id = 9,
                    IBAN = "NL22RABO1011562413",
                    AccountNumber = "1011562413",
                    IsActive = false
                },
                new IBANStore
                {
                    Id = 10,
                    IBAN = "NL69ABNA4293946624",
                    AccountNumber = "4293946624",
                    IsActive = false
                },
                new IBANStore
                {
                    Id = 11,
                    IBAN = "NL11RABO2682297498",
                    AccountNumber = "2682297498",
                    IsActive = false
                },
                new IBANStore
                {
                    Id = 12,
                    IBAN = "NL62INGB2067756052",
                    AccountNumber = "2067756052",
                    IsActive = false
                },
                new IBANStore
                {
                    Id = 13,
                    IBAN = "NL55ABNA2859779760",
                    AccountNumber = "2859779760",
                    IsActive = false
                },
                new IBANStore
                {
                    Id = 14,
                    IBAN = "NL91INGB5055036109",
                    AccountNumber = "5055036109",
                    IsActive = false
                },
                new IBANStore
                {
                    Id = 15,
                    IBAN = "NL33RABO9589603858",
                    AccountNumber = "9589603858",
                    IsActive = false
                },
                new IBANStore
                {
                    Id = 16,
                    IBAN = "NL61ABNA8902022560",
                    AccountNumber = "8902022560",
                    IsActive = false
                }
            );
    }

    public DbSet<AccountHolder> AccountHolders { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<TransactionFee> TransactionFees { get; set; }
    
    public DbSet<TransactionHistory> TransactionHistories { get; set; }

    public DbSet<IBANStore> IbanStores { get; set; }
}