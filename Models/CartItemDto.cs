using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1PSSCAmbrusArmando.Models
{
    public record CartItemDto
    {
        public string Name { get; init; }
        public string ItemRegistrationNumber { get; init; }

        public int Itemq {get; init;}

        public string Address { get; init; }

        public int FinalPrice { get; init; }
    }
}
