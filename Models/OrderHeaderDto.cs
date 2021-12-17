using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1PSSCAmbrusArmando.Models
{
    public class OrderHeaderDto
    {
        public int OrderId { get; set; }
        public string Address { get; set; }
        public int Total { get; set; }
    }
}
