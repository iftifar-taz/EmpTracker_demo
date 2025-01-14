using EmpTracker.ApiGateway.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.ConfigureRateLImiting();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseRateLimiter();

app.UseJwtAuthentication();

app.MapReverseProxy();
app.Run();