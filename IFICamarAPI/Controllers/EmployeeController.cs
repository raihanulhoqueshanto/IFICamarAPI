using IFICamarAPI.Application.Pagings;
using IFICamarAPI.Application.Requests.Employee.Commands;
using IFICamarAPI.Application.Requests.Employee.Queries;
using IFICamarAPI.Domain.Entities.Employee;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IFICamarAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateEmployeePostingInfo(EmployeePostingInfo command)
        {
            try
            {
                var result = await _mediator.Send(new CreateOrUpdateEmployeePostingInfo(command));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeePreferences(int? id, string? employeeId, string? employeeName, string? isActive, string? getAll, int currentPage, int itemsPerPage)
        {
            try
            {
                var result = await _mediator.Send(new GetEmployeePreferences(id, employeeId, employeeName, isActive, getAll, currentPage, itemsPerPage));

                PaginationHeader.Add(Response, result.CurrentPage, result.ItemsPerPage, result.TotalPages, result.TotalItems);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
