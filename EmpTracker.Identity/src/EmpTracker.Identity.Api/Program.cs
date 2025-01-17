using System.Security.Claims;
using EmpTracker.Identity.Api.Configurations;
using EmpTracker.Identity.Api.Middlewares;
using EmpTracker.Identity.Core.Domain.Entities;
using EmpTracker.Identity.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLoging();
builder.Services.AddDbContext<DataContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
builder.Services.ConfigureMessageBus();
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.ConfigureSwagger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();

var app = builder.Build();

await app.ApplyPendingMigrations();
app.UseRequestLogging();

app.UseCustomSwagger();

app.UseHttpsRedirection();
app.UseCorseForAll();

app.UseJwtAuthentication();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();

internal sealed class CustomClaimsTransformation(IServiceProvider serviceProvider) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        //using IServiceScope scope = serviceProvider.CreateScope();
        //var _redisCacheService = scope.ServiceProvider.GetRequiredService<RedisCacheService>();
        //var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //if (userId == null)
        //{
        //    return principal;
        //}

        //var roles = await _redisCacheService.GetCacheAsync<IList<string>>(userId);
        //var claims = new List<Claim>();

        //foreach (var role in roles!)
        //{
        //    claims.Add(new Claim(ClaimTypes.Role, role));
        //}

        //var claimsIdentity = new ClaimsIdentity();
        //claimsIdentity.AddClaims(claims);
        //principal.AddIdentity(claimsIdentity);

        return principal;
    }
}
