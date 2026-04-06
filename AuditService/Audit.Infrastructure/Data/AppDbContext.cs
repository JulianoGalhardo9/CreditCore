namespace Audit.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AuditLog>(builder =>
        {
            builder.ToTable("AuditLogs");
            builder.HasKey(a => a.Id);
            
            builder.Property(a => a.EventName)
                   .IsRequired()
                   .HasMaxLength(100);
                   
            builder.Property(a => a.Payload)
                   .IsRequired();
        });
    }
}