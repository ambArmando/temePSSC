using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1PSSC.Domain
{
    internal class InvalidItemRegistrationNumberException : Exception
    {
        public InvalidItemRegistrationNumberException(string? message) : base(message)
        {
        }
    }
}
