using System;
using System.Collections.Generic;
using static LanguageExt.Prelude;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LanguageExt;

namespace lab1PSSC.Domain
{
    public record ItemRegistrationNumber
    {
        public const string Pattern = "^pr[0-9]";
        private static readonly Regex ValidPattern = new("^pr[0-9]");

        public string Value { get; }

        internal ItemRegistrationNumber(string value)
        {
            if (ValidPattern.IsMatch(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidItemRegistrationNumberException("");
            }
        }
        public static bool ValidateInputRegistrationNumber(string itemCode, out ItemRegistrationNumber itemc)
        {
            bool isValid = false;
            itemc = null;
            if (IsValid(itemCode)) {
                isValid = true;
                itemc = new(itemCode);
            }
            return isValid;
        }

        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

        public override string ToString()
        {
            return Value;
        }

        public static Option<ItemRegistrationNumber> TryParse(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<ItemRegistrationNumber>(new(stringValue));
            }
            else
            {
                return None;
            }
        }

    }
}
