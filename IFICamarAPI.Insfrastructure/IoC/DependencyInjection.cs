using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFICamarAPI.Application.Requests.Employee;
using IFICamarAPI.Application.Requests.PlaceOfPosting;
using IFICamarAPI.Insfrastructure.Data;
using IFICamarAPI.Insfrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IFICamarAPI.Insfrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)),
                    options => options.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null
                    ));
                options.EnableSensitiveDataLogging();
            });

            services.AddScoped<MysqlDbContext>();

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IPostingPlaceService, PostingPlaceService>();

            return services;
        }
    }
}
