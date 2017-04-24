using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineSales.DL;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnlineSales.Models
{
    public class CreditCardViewModel
    {
        [Required]
        [Display(Name = "Number")]
        [Range(1000000000000000, 9999999999999999, ErrorMessage = "must be 16 digits")]
        public long Number { get; set; }

        [Required]
        [Display(Name = "CVV")]
        [Range(100, 999, ErrorMessage = "Must be 3 digits")]
        public int CVV { get; set; }

        [Required]
        [Display(Name = "Year")]
        [Range(16, 25, ErrorMessage = "That year it's not valid, (YY)")]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Month")]
        [RegularExpression("^([0-9]|1[0-2])$", ErrorMessage = "That month it's not valid, (MM)")]
        public int Month { get; set; }
    }
}


