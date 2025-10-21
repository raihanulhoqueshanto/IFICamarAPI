using IFICamarAPI.Application.Requests.Employee.Commands;
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
        public async Task<IActionResult> CreateOrUpdateEmployeeInfo(EmployeeInfo command)
        {
            try
            {
                var result = await _mediator.Send(new CreateOrUpdateEmployeeInfo(command));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
