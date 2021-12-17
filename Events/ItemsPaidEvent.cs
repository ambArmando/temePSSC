using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab1PSSCAmbrusArmando.Models;

namespace lab1PSSCAmbrusArmando.Events
{
    public record ItemsPaidEvent
    {
        public List<CartItemDto> Items { get; init; }
    }
}
