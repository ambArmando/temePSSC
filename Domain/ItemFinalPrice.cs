using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab1PSSC.Domain;

namespace lab1PSSC.Domain
{
    public record ItemFinalPrice(ItemRegistrationNumber ItemRegistrationNumber, Item itemq, Address address, Payment payment, Item finalPrice)
    {
        public int ItemId { get; set; }
        public bool IsUpdated { get; set; }
    }

}
