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
    public class CreateOrUpdateEmployeeInfo : IRequest<Result>
    {
        public EmployeeInfo EmployeeInfo { get; set; }

        public CreateOrUpdateEmployeeInfo(EmployeeInfo employeeInfo)
        {
            EmployeeInfo = employeeInfo;
        }
    }

    public class CreateOrUpdateEmployeeInfoHandler : IRequestHandler<CreateOrUpdateEmployeeInfo, Result>
    {
        private readonly IEmployeeService _employeeService;

        public CreateOrUpdateEmployeeInfoHandler(IEmployeeService employeeService)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        }

        public async Task<Result> Handle(CreateOrUpdateEmployeeInfo request, CancellationToken cancellationToken)
        {
            var result = await _employeeService.CreateOrUpdateEmployeeInfo(request.EmployeeInfo);
            return result;
        }
    }
}
