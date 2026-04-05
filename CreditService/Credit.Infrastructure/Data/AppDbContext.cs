using Microsoft.EntityFrameworkCore;
using Credit.Domain.Entities;

namespace Credit.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Loan> Loans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Loan>(builder =>
        {
            builder.ToTable("Loans");
            
            builder.HasKey(l => l.Id);
            
            builder.Property(l => l.Amount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(l => l.Installments)
                   .IsRequired();

            builder.Property(l => l.Status)
                   .HasConversion<int>()
                   .IsRequired();
        });
    }
}