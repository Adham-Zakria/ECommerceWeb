﻿using Shared.DataTransferObjects.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.Order
{
    public class OrderRequest
    {
        public string BasketId { get; set; }
        public AddressDto Address { get; set; }
        public int DeliveryMethodId { get; set; }
    }
}
