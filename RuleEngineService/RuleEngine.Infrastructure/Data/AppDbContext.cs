using Microsoft.EntityFrameworkCore;
using RuleEngine.Domain.Entities;

namespace RuleEngine.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<CreditAnalysis> CreditAnalyses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CreditAnalysis>(builder =>
        {
            builder.ToTable("CreditAnalyses");
            builder.HasKey(a => a.Id);
            builder.OwnsOne(a => a.Score, s =>
            {
                s.Property(p => p.Value).HasColumnName("Score_Value").IsRequired();
                s.Property(p => p.Description).HasColumnName("Score_Description").IsRequired();
            });

            builder.Property(a => a.Observation).HasMaxLength(500);
        });
    }
}