using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFICamarAPI.Application.Pagings
{
    public class PageHeaderVM
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
    }
}
