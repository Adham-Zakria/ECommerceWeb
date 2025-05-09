using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.Basket
{
    public class BasketDto
    {
        public string Id { get; set; } // Guid
        public IEnumerable<BasketItemDto> Items { get; set; }
    }
}
