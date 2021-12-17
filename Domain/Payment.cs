using LanguageExt;
using static LanguageExt.Prelude;


namespace lab1PSSC.Domain
{
    public record Payment
    {
        public string Value { get; }
        public int intValue { get; }

        public Payment(int value)
        {
            if (IsValid(value))
            {
                intValue = value;
            }
            else
            {
                throw new InvalidPaymentException($"{value:0.##} is an invalid price value.");
            }
        }

        public static Option<Payment> TryParsePayment(int price)
        {
            if (price > 0)
            {
                return Some<Payment>(new(price));
            }
            else
            {
                return None;
            }
        }

        private static bool IsValid(int price) => price > 0;

        internal Payment(string state)
        {
            if (state.Equals("y") || state.Equals("n"))
            {
                Value = state;
            }
            else
            {
                throw new InvalidPaymentException($"{state:0.##} is an invalid paymnet value.");
            }
        }
        public static bool ValidateInputPayment(string paymentinfo, out Payment payment)
        {
            bool isValid = false;
            payment = null;
            if (paymentinfo.Equals("y") || paymentinfo.Equals("n"))
            {
                isValid = true;
                payment = new(paymentinfo);
            }
            return isValid;
        }

        public static Option<Payment> TryParsePayment(string payment)
        {
            if (payment.Equals("y") || payment.Equals("n"))
            {
                return Some<Payment>(new(payment));
            }
            else
            {
                return None;
            }
        }

        public override string ToString()
        {
            return $"{Value:0.##}";
        }
    }
}