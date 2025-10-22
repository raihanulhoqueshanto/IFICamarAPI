using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using IFICamarAPI.Application.Common.Models;
using IFICamarAPI.Application.Pagings;
using IFICamarAPI.Application.Requests.Employee;
using IFICamarAPI.Application.Requests.Employee.Queries;
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

        public async Task<Result> CreateOrUpdateEmployeePostingInfo(EmployeePostingInfo request)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(request.EmployeeId))
                {
                    return Result.Failure("Failed", "500", new[] { "Employee Id is required!" }, null);
                }

                var normalizedId = request.EmployeeId.Replace(" ", "").ToLower();

                // Update existing preference
                if (request.Id > 0)
                {
                    var existingPreference = await _context.EmployeePostingInfos.FindAsync(request.Id);

                    if (existingPreference == null)
                    {
                        return Result.Failure("Failed", "404", new[] { "Preferences of this employee not found!" }, null);
                    }

                    // Check for duplicate preference excluding current preference
                    var duplicatePreference = await _context.EmployeePostingInfos
                        .FirstOrDefaultAsync(e => e.EmployeeId.Replace(" ", "").ToLower() == normalizedId && e.Id != request.Id);

                    if (duplicatePreference != null)
                    {
                        return Result.Failure("Failed", "409", new[] { "Preferences of this employee already exists!" }, null);
                    }

                    // Update preference properties
                    existingPreference.EmployeeName = request.EmployeeName;
                    existingPreference.PreferencePlaceOne = request.PreferencePlaceOne;
                    existingPreference.PreferencePlaceTwo = request.PreferencePlaceTwo;
                    existingPreference.PreferencePlaceThree = request.PreferencePlaceThree;
                    existingPreference.IsActive = request.IsActive;

                    _context.EmployeePostingInfos.Update(existingPreference);

                    int result = await _context.SaveChangesAsync();

                    return result > 0
                        ? Result.Success("Success", "200", new[] { "Updated Successfully" }, null)
                        : Result.Failure("Failed", "500", new[] { "Operation failed. Please try again!" }, null);
                }
                // Create new preferences
                else
                {
                    // Check for duplicate preference
                    var duplicatePreference = await _context.EmployeePostingInfos
                        .FirstOrDefaultAsync(e => e.EmployeeId.Replace(" ", "").ToLower() == normalizedId);

                    if (duplicatePreference != null)
                    {
                        return Result.Failure("Failed", "409", new[] { "Duplicate data found. Preferences of this employee already exists." }, null);
                    }

                    await _context.EmployeePostingInfos.AddAsync(request);

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

        public async Task<PagedList<EmployeePreferenceVM>> GetEmployeePreferences(GetEmployeePreferences request)
        {
            string conditionClause = " ";
            var queryBuilder = new StringBuilder();
            var parameter = new DynamicParameters();

            queryBuilder.AppendLine("SELECT employee_posting_info.*, count(*) over() as TotalItems FROM employee_posting_info ");

            if (request.Id != null)
            {
                queryBuilder.AppendLine($"{Helper.GetSqlCondition(conditionClause, "AND")} id = @Id");
                conditionClause = " WHERE ";
                parameter.Add("Id", request.Id, DbType.Int32, ParameterDirection.Input);
            }

            if (!string.IsNullOrEmpty(request.EmployeeId))
            {
                queryBuilder.AppendLine($"{Helper.GetSqlCondition(conditionClause, "AND")} employee_id = @EmployeeId");
                conditionClause = " WHERE ";
                parameter.Add("EmployeeId", request.EmployeeId, DbType.String, ParameterDirection.Input);
            }

            if (!string.IsNullOrEmpty(request.EmployeeName))
            {
                queryBuilder.AppendLine($"{Helper.GetSqlCondition(conditionClause, "AND")} employee_name LIKE @EmployeeName");
                conditionClause = " WHERE ";
                parameter.Add("EmployeeName", $"%{request.EmployeeName}%", DbType.String, ParameterDirection.Input);
            }

            if (!string.IsNullOrEmpty(request.IsActive))
            {
                queryBuilder.AppendLine($"{Helper.GetSqlCondition(conditionClause, "AND")} is_active = @IsActive");
                conditionClause = " WHERE ";
                parameter.Add("IsActive", request.IsActive, DbType.String, ParameterDirection.Input);
            }

            if (!string.IsNullOrEmpty(request.GetAll) && request.GetAll.ToUpper() == "Y")
            {
                request.ItemsPerPage = 0;
            }
            else
            {
                queryBuilder.AppendLine("LIMIT @Offset, @ItemsPerPage");
                parameter.Add("Offset", (request.CurrentPage - 1) * request.ItemsPerPage, DbType.Int32, ParameterDirection.Input);
                parameter.Add("ItemsPerPage", request.ItemsPerPage, DbType.Int32, ParameterDirection.Input);
            }

            string query = queryBuilder.ToString();
            var result = await _mysqlContext.GetPagedListAsync<EmployeePreferenceVM>(request.CurrentPage, request.ItemsPerPage, query, parameter);
            return result;
        }
    }
}
