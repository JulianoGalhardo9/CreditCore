using Microsoft.EntityFrameworkCore;
using Identity.Application.Interfaces;
using Identity.Application.Handlers;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Repositories;
using Identity.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

// 1. Dizemos para a API que vamos usar Controllers (nossos Recepcionistas).
builder.Services.AddControllers();

// 2. Configuramos o Banco de Dados (DbContext).
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer("Server=localhost,1433;Database=CreditCore_Identity;User Id=sa;Password=SuaSenhaForte123!;TrustServerCertificate=True;");
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<RegisterUserCommandHandler>());

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

var app = builder.Build();

app.MapControllers();

app.Run();