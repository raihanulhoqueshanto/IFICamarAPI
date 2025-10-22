using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFICamarAPI.Application.Common.Models;
using IFICamarAPI.Application.Pagings;
using IFICamarAPI.Application.Requests.Employee.Queries;
using IFICamarAPI.Domain.Entities.Employee;

namespace IFICamarAPI.Application.Requests.Employee
{
    public interface IEmployeeService : IDisposable
    {
        Task<Result> CreateOrUpdateEmployeePostingInfo(EmployeePostingInfo request);
        Task<PagedList<EmployeePreferenceVM>> GetEmployeePreferences(GetEmployeePreferences request);
    }
}
