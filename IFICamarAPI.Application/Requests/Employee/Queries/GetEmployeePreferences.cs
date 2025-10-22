using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFICamarAPI.Application.Pagings;
using MediatR;

namespace IFICamarAPI.Application.Requests.Employee.Queries
{
    public class GetEmployeePreferences : PageParameters, IRequest<PagedList<EmployeePreferenceVM>>
    {
        public int? Id { get; set; }
        public string? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? IsActive { get; set; }
        public string? GetAll { get; set; }

        public GetEmployeePreferences(int? id, string? employeeId, string? employeeName, string? isActive, string? getAll, int currentPage, int itemsPerPage) : base(currentPage, itemsPerPage)
        {
            Id = id;
            EmployeeId = employeeId;
            EmployeeName = employeeName;
            IsActive = isActive;
            GetAll = getAll;
        }
    }

    public class GetEmployeePreferencesHandler : IRequestHandler<GetEmployeePreferences, PagedList<EmployeePreferenceVM>>
    {
        private readonly IEmployeeService _employeeService;

        public GetEmployeePreferencesHandler(IEmployeeService employeeService)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        }

        public async Task<PagedList<EmployeePreferenceVM>> Handle(GetEmployeePreferences request, CancellationToken cancellationToken)
        {
            return await _employeeService.GetEmployeePreferences(request);
        }
    }
}
