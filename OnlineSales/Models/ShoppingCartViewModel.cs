using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineSales.DL;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnlineSales.Models
{
    public class ShoppingCartViewModel
    {
        public List<Product> Products { get; set; }        
        public List<CreditCard> listUserCards { get; set; }
        public List<CartProduct> ListCarts { get; set; }
    }
}

