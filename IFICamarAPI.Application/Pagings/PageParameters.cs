using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFICamarAPI.Application.Pagings
{
    public class PageParameters
    {
        const int maxPageSize = 100;
        private int _itemsPerPage = 10;
        public int CurrentPage { get; set; }
        public string OrderBy { get; set; }
        public int ItemsPerPage
        {
            get
            {
                return _itemsPerPage;
            }
            set
            {
                _itemsPerPage = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public PageParameters(int currentPage, int itemsPerpage)
        {
            CurrentPage = (currentPage == 0) ? 1 : currentPage;
            ItemsPerPage = (itemsPerpage == 0) ? _itemsPerPage : itemsPerpage;

        }

        public PageParameters()
        {
        }
    }
}
