using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1PSSC.Domain
{
    public class PayItemsCommand
    {
        public PayItemsCommand(IReadOnlyCollection<UnvalidatedCustomerItem> inputCartItems) {
            InputCartItems = inputCartItems;
        }

        public IReadOnlyCollection<UnvalidatedCustomerItem> InputCartItems { get; }

    }
}
