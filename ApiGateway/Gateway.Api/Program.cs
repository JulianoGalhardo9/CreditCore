var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{

    options.SwaggerEndpoint("/api/User/swagger/v1/swagger.json", "Identity Service");
    options.SwaggerEndpoint("/api/Loan/swagger/v1/swagger.json", "Credit Service");
    options.RoutePrefix = string.Empty;
});

app.MapReverseProxy();

app.Run();