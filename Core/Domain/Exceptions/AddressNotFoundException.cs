using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class AddressNotFoundException(string userName) 
        : NotFoundException($"This user : {userName} has no address")
    {
    }
}
