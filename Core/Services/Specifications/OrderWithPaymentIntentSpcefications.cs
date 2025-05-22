using Domain.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class OrderWithPaymentIntentSpcefications(string paymentIntentId)
        : BaseSpecifications<Order>(o=>o.PaymentIntentId == paymentIntentId)
    {
    }
}
