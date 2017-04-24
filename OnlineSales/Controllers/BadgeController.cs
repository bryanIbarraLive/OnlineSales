using OnlineSales.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace OnlineSales.Controllers
{
    public class BadgeController : Controller
    {
        // GET: Prep
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Count()
        {
            using (DBContext context = new DBContext())
            {
                int badge = 0;
                List<CartProduct> listCarts = new List<CartProduct>();
                Customer cust = context.Customers.Where(x => x.E_mail == User.Identity.Name).FirstOrDefault();
                if (cust != null)
                {
                    int ShoppingCart = context.ShoppingCarts.Where(x => x.CustomerID == cust.ID).First().ID;
                    listCarts = context.CartProducts.Where(x => x.ShoppingCartID == ShoppingCart).ToList();

                    foreach (CartProduct item in listCarts)
                    {
                        badge += item.Counter;
                    }
                }

                return Json(new { badge }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}