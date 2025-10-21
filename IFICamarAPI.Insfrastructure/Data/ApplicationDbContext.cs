using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFICamarAPI.Domain.Entities.Common.Models;
using IFICamarAPI.Domain.Entities.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IFICamarAPI.Insfrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<EmployeeInfo> EmployeeInfos { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;

                foreach (var entry in ChangeTracker.Entries<CommonEntity>())
                {
                    // Get client IP from headers (Cloudflare and Proxy aware)
                    string ip = httpContext?.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ?? httpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? httpContext?.Connection.RemoteIpAddress?.ToString();

                    // Handle loopback (::1) for localhost
                    if (ip == "::1")
                    {
                        ip = "127.0.0.1";
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.Ip = ip;
                            //entry.Entity.CreatedBy = _currentUserService.UserId;
                            entry.Entity.CreatedBy = "Shanto";
                            entry.Entity.CreatedDate = DateTime.Now;
                            break;

                        case EntityState.Modified:
                            entry.Entity.Ip = ip;
                            //entry.Entity.UpdatedBy = _currentUserService.UserId;
                            entry.Entity.UpdatedBy = "Shanto";
                            entry.Entity.UpdatedDate = DateTime.Now;
                            break;
                    }
                }

                return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log the error (you can add logging here)
                throw;
            }
            catch (Exception ex)
            {
                // Log the error (you can add logging here)
                throw;
            }
        }
    }
}
