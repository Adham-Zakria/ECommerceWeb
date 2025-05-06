using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Products
{
    public class Product : BaseEntity<int>
    {
        public string Description { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public int BrandId { get; set; } //FK
        public ProductBrand ProductBrand { get; set; } // Nav Prop
        public int TypeId { get; set; } //FK
        public ProductType ProductType { get; set; } // Nav Prop
    }
}
