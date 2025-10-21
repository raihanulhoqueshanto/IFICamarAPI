using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFICamarAPI.Domain.Entities.Common.Models
{
    public class CommonEntity
    {
        [Column("ip")]
        public string? Ip { get; set; }

        [Column("created_by")]
        public string? CreatedBy { get; set; }

        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }

        [Column("updated_by")]
        public string? UpdatedBy { get; set; }

        [Column("updated_date")]
        public DateTime? UpdatedDate { get; set; }
    }
}
