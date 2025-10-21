using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFICamarAPI.Application.Common.Models;
using IFICamarAPI.Application.Requests.Employee;
using IFICamarAPI.Domain.Entities.Employee;
using IFICamarAPI.Insfrastructure.Data;
using IFICamarAPI.Insfrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace IFICamarAPI.Insfrastructure.Services
{
    internal class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly MysqlDbContext _mysqlContext;
        private readonly IHostEnvironment _hostEnvironment;

        public EmployeeService(ApplicationDbContext context, MysqlDbContext mysqlContext, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _mysqlContext = mysqlContext;
            _hostEnvironment = hostEnvironment;
        }

        public void Dispose()
        {
            _context.Dispose();
            _mysqlContext.Dispose();
        }

        public async Task<Result> CreateOrUpdateEmployeeInfo(EmployeeInfo request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Nid))
                {
                    return Result.Failure("Failed", "500", new[] { "Nid is required!" }, null);
                }

                if (request.Id > 0)
                {
                    var existingEmployee = await _context.EmployeeInfos.FindAsync(request.Id);

                    if (existingEmployee == null)
                    {
                        return Result.Failure("Failed", "404", new[] { "Employee not found!" }, null);
                    }

                    var duplicateEmployee = await _context.EmployeeInfos.FirstOrDefaultAsync(e => e.Nid.Trim() == request.Nid.Trim() && e.Id != request.Id);

                    if (duplicateEmployee != null)
                    {
                        return Result.Failure("Failed", "409", new[] { "This nid already exists!" }, null);
                    }

                    existingEmployee.FirstName = request.FirstName;
                    existingEmployee.LastName = request.LastName;
                    existingEmployee.Nid = request.Nid;
                    existingEmployee.IsActive = request.IsActive;

                    _context.EmployeeInfos.Update(existingEmployee);

                    int result = await _context.SaveChangesAsync();

                    return result > 0
                        ? Result.Success("Success", "200", new[] { "Updated Successfully" }, null)
                        : Result.Failure("Failed", "500", new[] { "Operation failed. Please try again!" }, null);
                }
                else
                {
                    var duplicateEmployee = await _context.EmployeeInfos
                        .FirstOrDefaultAsync(b => b.Nid.Trim() == request.Nid.Trim());

                    if (duplicateEmployee != null)
                    {
                        return Result.Failure("Failed", "409", new[] { "Duplicate data found. This Nid already exists." }, null);
                    }

                    await _context.EmployeeInfos.AddAsync(request);

                    int result = await _context.SaveChangesAsync();

                    return result > 0
                        ? Result.Success("Success", "200", new[] { "Saved Successfully" }, null)
                        : Result.Failure("Failed", "500", new[] { "Operation failed. Please try again!" }, null);
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return Result.Failure("Failed", "500", new[] { errorMessage }, null);
            }
        }
    }
}
