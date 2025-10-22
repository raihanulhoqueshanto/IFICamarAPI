using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFICamarAPI.Domain.Entities.Common.Models;

namespace IFICamarAPI.Domain.Entities.PostingPlace
{
    [Table("posting_place")]
    public class PostingPlace : CommonEntity
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("place_name")]
        public string PlaceName { get; set; }

        [Column("is_active")]
        public string IsActive { get; set; }
    }
}
