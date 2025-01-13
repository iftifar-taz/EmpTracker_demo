using Asp.Versioning;
using EmpTracker.EmpService.Application.Behaviors.Validators;
using EmpTracker.EmpService.Application.Features.Employees.Commands;
using EmpTracker.EmpService.Core.Interfaces;
using EmpTracker.EmpService.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace EmpTracker.EmpService.Api.Configurations
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
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

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            return services;
        }
    }
}
