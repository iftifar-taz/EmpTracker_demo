using EmpTracker.DptService.Api.Configurations;
using EmpTracker.DptService.Api.Middlewares;
using EmpTracker.DptService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLoging();
builder.Services.AddDbContext<DataContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.ConfigureGrpc();
builder.Services.ConfigureMessageBus();
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.ConfigureSwagger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapGrpcServices();

app.ApplyPendingMigrations();
app.UseRequestLogging();

app.UseCustomSwagger();

app.UseHttpsRedirection();
app.UseCorseForAll();

app.UseJwtAuthentication();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
