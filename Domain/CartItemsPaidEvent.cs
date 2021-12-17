using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharp.Choices;

namespace lab1PSSC.Domain
{
    [AsChoice]
    public static partial class CartItemsPaidEvent
    {
        public interface ICartItemsPaidEvent { }

        public record CartItemsSucceededPayEvent : ICartItemsPaidEvent
        {
            public string Csv { get; }
            public string PaidItems { get; }

            public IEnumerable<ItemFinalPrice> Items { get; }

            internal CartItemsSucceededPayEvent(string csv, string paidItems)
            {
                Csv = csv;
                PaidItems = paidItems;
            }

            internal CartItemsSucceededPayEvent(string csv, IEnumerable<ItemFinalPrice> items)
            {
                Csv = csv;
                Items = items;
            }
        }

        public record CartItemsFailedPayEvent : ICartItemsPaidEvent
        {
            public string Reason { get; }

            internal CartItemsFailedPayEvent(/*IReadOnlyCollection<UnvalidatedCustomerItem> itemList,*/ string reason)
            {
                Reason = reason;
            }
        }
    }
}
