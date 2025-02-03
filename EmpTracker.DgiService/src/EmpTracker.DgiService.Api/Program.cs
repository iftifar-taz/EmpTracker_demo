using EmpTracker.DgiService.Api.Configurations;
using EmpTracker.DgiService.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLoging();

builder.Services.ConfigureDatabase(builder.Configuration)
    .ConfigureJwtAuthentication(builder.Configuration)
    .ConfigureApplicationServices()
    .ConfigureEventBus(builder.Configuration)
    .ConfigureGrpc()
    .ConfigureBackgroundJob(builder.Configuration)
    .ConfigureSwagger();

var app = builder.Build();

app.MapGrpcServices();

app.ApplyPendingMigrations();
app.UseRequestLogging();

app.UseCustomSwagger();
app.UseBackgroundJobDashboard();

app.UseHttpsRedirection();
app.UseCorseForAll();

app.UseJwtAuthentication();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

await app.RunAsync();
