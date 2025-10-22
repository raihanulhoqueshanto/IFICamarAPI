using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using IFICamarAPI.Application.Common.Models;
using IFICamarAPI.Application.Pagings;
using IFICamarAPI.Application.Requests.PlaceOfPosting;
using IFICamarAPI.Application.Requests.PlaceOfPosting.Queries;
using IFICamarAPI.Domain.Entities.PostingPlace;
using IFICamarAPI.Insfrastructure.Data;
using IFICamarAPI.Insfrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace IFICamarAPI.Insfrastructure.Services
{
    public class PostingPlaceService : IPostingPlaceService
    {
        private readonly ApplicationDbContext _context;
        private readonly MysqlDbContext _mysqlContext;
        private readonly IHostEnvironment _hostEnvironment;

        public PostingPlaceService(ApplicationDbContext context, MysqlDbContext mysqlContext, IHostEnvironment hostEnvironment)
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

        public async Task<Result> CreateOrUpdatePostingPlace(PostingPlace request)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(request.PlaceName))
                {
                    return Result.Failure("Failed", "500", new[] { "Name is required!" }, null);
                }

                var normalizedName = request.PlaceName.Replace(" ", "").ToLower();

                // Update existing posting place
                if (request.Id > 0)
                {
                    var existingPlace = await _context.PostingPlaces.FindAsync(request.Id);

                    if (existingPlace == null)
                    {
                        return Result.Failure("Failed", "404", new[] { "Posting place not found!" }, null);
                    }

                    // Check for duplicate posting place excluding current place
                    var duplicatePlace = await _context.PostingPlaces
                        .FirstOrDefaultAsync(p => p.PlaceName.Replace(" ", "").ToLower() == normalizedName && p.Id != request.Id);

                    if (duplicatePlace != null)
                    {
                        return Result.Failure("Failed", "409", new[] { "Posting place already exists!" }, null);
                    }

                    // Update posting place properties
                    existingPlace.PlaceName = request.PlaceName;
                    existingPlace.IsActive = request.IsActive;

                    _context.PostingPlaces.Update(existingPlace);

                    int result = await _context.SaveChangesAsync();

                    return result > 0
                        ? Result.Success("Success", "200", new[] { "Updated Successfully" }, null)
                        : Result.Failure("Failed", "500", new[] { "Operation failed. Please try again!" }, null);
                }
                // Create new posting place
                else
                {
                    // Check for duplicate posting place
                    var duplicatePlace = await _context.PostingPlaces
                        .FirstOrDefaultAsync(p => p.PlaceName.Replace(" ", "").ToLower() == normalizedName);

                    if (duplicatePlace != null)
                    {
                        return Result.Failure("Failed", "409", new[] { "Duplicate data found. Posting place already exists." }, null);
                    }

                    await _context.PostingPlaces.AddAsync(request);

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

        public async Task<PagedList<PostingPlaceVM>> GetPostingPlaces(GetPostingPlaces request)
        {
            string conditionClause = " ";
            var queryBuilder = new StringBuilder();
            var parameter = new DynamicParameters();

            queryBuilder.AppendLine("SELECT posting_place.*, count(*) over() as TotalItems FROM posting_place ");

            if (request.Id != null)
            {
                queryBuilder.AppendLine($"{Helper.GetSqlCondition(conditionClause, "AND")} id = @Id");
                conditionClause = " WHERE ";
                parameter.Add("Id", request.Id, DbType.Int32, ParameterDirection.Input);
            }

            if (!string.IsNullOrEmpty(request.PlaceName))
            {
                queryBuilder.AppendLine($"{Helper.GetSqlCondition(conditionClause, "AND")} place_name LIKE @PlaceName");
                conditionClause = " WHERE ";
                parameter.Add("PlaceName", $"%{request.PlaceName}%", DbType.String, ParameterDirection.Input);
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
            var result = await _mysqlContext.GetPagedListAsync<PostingPlaceVM>(request.CurrentPage, request.ItemsPerPage, query, parameter);
            return result;
        }
    }
}
