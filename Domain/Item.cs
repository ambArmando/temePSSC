using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LanguageExt.Prelude;
using LanguageExt;

namespace lab1PSSC.Domain
{
    public record Item
    {
        public int Value { get; }

        internal Item(int quantity)
        {
            if (IsValid(quantity))
            {
                Value = quantity;
            }
            else
            {
                throw new InvalidItemException($"{quantity:0.##} is an invalid quantity value.");
            }
        }
        public override string ToString()
        {
            return $"{Value:0.##}";
        }

        public static Item operator +(Item a, Item b) => new Item((a.Value + b.Value));

        public static bool TryParseItemQuantity(string itemqString)
        {
            bool isValid = false;
            //itemq = null;
            if (int.TryParse(itemqString, out int numericItemq)) {
                if (IsValid(numericItemq)) {
                    isValid = true;
                   // itemq = new(numericItemq);
                }
            }
            return isValid;
        }

        public static Option<Item> TryParseItem(string itemqString)
        {
            if (int.TryParse(itemqString, out int numericItemq) && IsValid(numericItemq))
            {
                return Some<Item>(new(numericItemq));
            }
            else
            {
                return None;
            }
        }


        private static bool IsValid(int numericItemq) => numericItemq > 0 && numericItemq <= 1000;
    }
}
