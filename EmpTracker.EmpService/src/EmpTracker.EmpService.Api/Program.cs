using EmpTracker.DptService.Core.Protos;
using EmpTracker.EmpService.Api.Configurations;
using EmpTracker.EmpService.Api.Middlewares;
using EmpTracker.EmpService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add gRPC client service.
builder.Services.AddGrpcClient<DepartmentGrpc.DepartmentGrpcClient>(o =>
{
    o.Address = new Uri("https://localhost:7102");
});

builder.ConfigureLoging();
builder.Services.AddDbContext<DataContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.ConfigureMessageBus();

builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.ConfigureSwagger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.ApplyPendingMigrations();
app.UseRequestLogging();

app.UseCustomSwagger();

app.UseHttpsRedirection();
app.UseCorseForAll();

app.UseJwtAuthentication();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
