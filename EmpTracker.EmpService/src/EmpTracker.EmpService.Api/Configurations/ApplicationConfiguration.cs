﻿using System.Security.Claims;
using Asp.Versioning;
using EmpTracker.EmpService.Application.Behaviors.Validators;
using EmpTracker.EmpService.Application.Features.Employees.Commands;
using EmpTracker.EmpService.Core.Interfaces;
using EmpTracker.EmpService.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;

namespace EmpTracker.EmpService.Api.Configurations
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<RedisCache>();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<CreateEmployeeCommand>();
            });

            services.AddValidatorsFromAssemblyContaining<CreateEmployeeCommandValidator>();
            services.AddFluentValidationAutoValidation();

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();

            return services;
        }

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
    }
}
