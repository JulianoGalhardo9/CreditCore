using Microsoft.EntityFrameworkCore;
using Identity.Domain.Entities;

namespace Identity.Infrastructure.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.Property(u => u.FullName)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(u => u.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(u => u.Role)
                   .HasConversion<int>()
                   .IsRequired();
        });
    }
}