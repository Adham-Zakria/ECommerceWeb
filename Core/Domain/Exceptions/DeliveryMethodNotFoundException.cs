using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class DeliveryMethodNotFoundException(int id) :
        NotFoundException($"Delivery method with this Id : {id} is not found")
    {
    }
}
