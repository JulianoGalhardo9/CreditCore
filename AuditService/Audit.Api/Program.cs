using MassTransit;
using Microsoft.EntityFrameworkCore;
using Audit.Application.Consumers;
using Audit.Application.Interfaces;
using Audit.Infrastructure.Data;
using Audit.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Server=sqlserver,1433;Database=AuditDb;User Id=SA;Password=SuaSenhaForte123!;TrustServerCertificate=True";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreditRequestedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        
        cfg.ConfigureEndpoints(context); 
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Audit.Infrastructure.Data.AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();