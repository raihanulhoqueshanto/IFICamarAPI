using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFICamarAPI.Application.Common.Models;
using IFICamarAPI.Application.Pagings;
using IFICamarAPI.Domain.Entities.Employee;

namespace IFICamarAPI.Application.Requests.Employee
{
    public interface IEmployeeService : IDisposable
    {
        Task<Result> CreateOrUpdateEmployeeInfo(EmployeeInfo request);
        //Task<PagedList<BrandVM>> GetBrand(GetBrand request);
    }
}
