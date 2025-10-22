using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFICamarAPI.Application.Requests.Employee
{
    public class EmployeePreferenceVM
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string PreferencePlaceOne { get; set; }
        public string PreferencePlaceTwo { get; set; }
        public string PreferencePlaceThree { get; set; }
        public string IsActive { get; set; }
    }
}
