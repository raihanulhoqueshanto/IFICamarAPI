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
    [Table("employee_info")]
    public class EmployeeInfo : CommonEntity
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("nid")]
        public string Nid { get; set; }

        [Column("is_active")]
        public string IsActive { get; set; }
    }
}
