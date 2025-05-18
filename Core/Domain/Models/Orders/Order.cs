using Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Orders
{
    public enum PaymentStatus
    {
        Pending=0,
        PaymentReceived=1,
        PaymentFailed =2
    }
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
            
        }
        public Order(List<OrderItems> items, OrderAddress address, decimal subTotal, string email, DeliveryMethod method)
        {
            Items = items;
            Address = address;
            SubTotal = subTotal;
            UserEmail = email;
            DeliveryMethod = method;
        }
        public string UserEmail { get; set; }
        public List<OrderItems> Items { get; set; } = [];
        public OrderAddress Address { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
        public decimal SubTotal { get; set; } // total price of the order without shiping price
        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public DeliveryMethod DeliveryMethod { get; set; }
        public int DeliveryMethodId { get; set; }
    }
}
