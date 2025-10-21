using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IFICamarAPI.Application.Pagings
{
    public static class PaginationHeader
    {
        public static void Add(this HttpResponse response,
           int currentPage, int itemsPerPage, int totalPages, int totalItems, decimal totalAmount = 0)
        {
            var paginationHeader = new
            {
                CurrentPage = currentPage,
                ItemsPerPage = itemsPerPage,
                TotalPages = totalPages,
                TotalItems = totalItems,
                TotalAmount = totalAmount
            };
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination",
                JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
