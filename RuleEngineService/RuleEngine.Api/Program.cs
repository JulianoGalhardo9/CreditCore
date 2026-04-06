using MassTransit;
using Microsoft.EntityFrameworkCore;
using RuleEngine.Application.Consumers;
using RuleEngine.Application.Interfaces;
using RuleEngine.Infrastructure.Data;
using RuleEngine.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Server=localhost,1433;Database=RuleEngineDb;User Id=SA;Password=SuaSenhaForte123!;TrustServerCertificate=True";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<ICreditAnalysisRepository, CreditAnalysisRepository>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreditRequestedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("rule-engine-credit-requested-queue", e =>
        {
            e.ConfigureConsumer<CreditRequestedEventConsumer>(context);
        });
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();