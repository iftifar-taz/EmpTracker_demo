using EmpTracker.EmpService.Api.Configurations;
using EmpTracker.EmpService.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLoging();
builder.Services.ConfigureApplicationServices(builder.Configuration);
builder.Services.ConfigureMessageBus();
builder.Services.ConfigureGrpc(builder.Configuration);

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
