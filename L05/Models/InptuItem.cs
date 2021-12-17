using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using lab1PSSC.Domain;

namespace L05.Models
{
    public class InputItem
    {
        [Required]
        [RegularExpression(ItemRegistrationNumber.Pattern)]
        public string RegistrationNumber { get; set; }

        [Required]
        public string Quantity { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int FinalPrice { get; set; }
    }
}
