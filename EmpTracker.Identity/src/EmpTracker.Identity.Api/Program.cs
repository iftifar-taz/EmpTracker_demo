using EmpTracker.Identity.Api.Configurations;
using EmpTracker.Identity.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLoging();

builder.Services.ConfigureDatabase(builder.Configuration)
    .ConfigureJwtAuthentication(builder.Configuration)
    .ConfigureApplicationServices()
    .ConfigureEventBus(builder.Configuration)
    .ConfigureBackgroundJob(builder.Configuration)
    .ConfigureSwagger();

var app = builder.Build();

await app.ApplyPendingMigrations();
app.UseRequestLogging();

app.UseCustomSwagger();
app.UseBackgroundJobDashboard();

app.UseHttpsRedirection();
app.UseCorseForAll();

app.UseJwtAuthentication();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

await app.RunAsync();
