using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1PSSC.Domain
{
    public record ValidatedCustomerItem(ItemRegistrationNumber ItemRegistrationNumber, Item item, Address address, Payment paid);
}
