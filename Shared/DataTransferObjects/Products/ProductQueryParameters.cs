using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.Products
{
    public class ProductQueryParameters
    {
        public int? TypeId { get; set; }
        public int? BrandId { get; set; }
        public ProductSortingOptions ProductSortingOptions { get; set; }
        public string? Search { get; set; } 
        public int PageIndex { get; set; } = 1;

        private const int _defaultPageSize= 5;
        private const int _maxPageSize= 10;

        private int pageSize;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 0 && value < _maxPageSize ? value : _defaultPageSize; }
        }

    }
}
