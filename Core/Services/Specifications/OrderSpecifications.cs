﻿using Domain.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class OrderSpecifications : BaseSpecifications<Order>
    {
        // for get by id
        public OrderSpecifications(Guid id):base(o=>o.Id == id)
        {
            AddInclude(o=>o.Items);
            AddInclude(o=>o.DeliveryMethod);
        }

        // for get all
        public OrderSpecifications(string email) : base(o => o.UserEmail == email)
        {
            AddInclude(o => o.Items);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}
