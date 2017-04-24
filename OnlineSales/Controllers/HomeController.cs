using Newtonsoft.Json;
using OnlineSales.DL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineSales.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (DBContext ctx = new DBContext())
            {
                List<Product> productsToShow = ctx.Products.GroupBy(x => x.Type).Select(x => x.FirstOrDefault()).ToList();
                ViewBag.data = JsonConvert.SerializeObject(productsToShow);
                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "What it's the purpose of this page??";

            return View();
        }

        public ActionResult Team()
        {
            ViewBag.Message = "Members";

            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

       
    }
}