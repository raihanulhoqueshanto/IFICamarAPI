using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFICamarAPI.Domain.Entities.Common.Models;

namespace IFICamarAPI.Domain.Entities.Employee
{
    [Table("employee_posting_info")]
    public class EmployeePostingInfo : CommonEntity
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("employee_id")]
        public string EmployeeId { get; set; }

        [Column("employee_name")]
        public string EmployeeName { get; set; }

        [Column("preference_place_one")]
        public string PreferencePlaceOne { get; set; }

        [Column("preference_place_two")]
        public string PreferencePlaceTwo { get; set; }

        [Column("preference_place_three")]
        public string PreferencePlaceThree { get; set; }

        [Column("is_active")]
        public string IsActive { get; set; }
    }
}
