using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFICamarAPI.Application.Common.Models;
using IFICamarAPI.Domain.Entities.Employee;
using MediatR;

namespace IFICamarAPI.Application.Requests.Employee.Commands
{
    public class CreateOrUpdateEmployeePostingInfo : IRequest<Result>
    {
        public EmployeePostingInfo EmployeePostingInfo { get; set; }

        public CreateOrUpdateEmployeePostingInfo(EmployeePostingInfo employeePostingInfo)
        {
            EmployeePostingInfo = employeePostingInfo;
        }
    }

    public class CreateOrUpdateEmployeePostingInfoHandler : IRequestHandler<CreateOrUpdateEmployeePostingInfo, Result>
    {
        private readonly IEmployeeService _employeeService;

        public CreateOrUpdateEmployeePostingInfoHandler(IEmployeeService employeeService)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        }

        public async Task<Result> Handle(CreateOrUpdateEmployeePostingInfo request, CancellationToken cancellationToken)
        {
            var result = await _employeeService.CreateOrUpdateEmployeePostingInfo(request.EmployeePostingInfo);
            return result;
        }
    }
}
