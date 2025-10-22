using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFICamarAPI.Application.Requests.PlaceOfPosting
{
    public class PostingPlaceVM
    {
        public int Id { get; set; }
        public string PlaceName { get; set; }
        public string IsActive { get; set; }
    }
}
