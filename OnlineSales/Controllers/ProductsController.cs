using Newtonsoft.Json;
using OnlineSales.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineSales
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult ProductsBase()
        {          
            using (DBContext ctx = new DBContext())
            {
                var query = ctx.Products.ToList();
                isRegistered();
                TempData["Title"] = "All our Products";
                TempData[ "data" ] = JsonConvert.SerializeObject(query);  
                return RedirectToAction("Products");
            }
        }

        public ActionResult ProductsByType(int Type)
        {
            using (DBContext ctx = new DBContext())
            {
                List < Product > listProducts = new List< Product >();
                string prodType = ctx.Products.Where(x => x.ID == Type).FirstOrDefault().Type;
                listProducts = ctx.Products.Where(x => x.Type == prodType).ToList();
                isRegistered();
                TempData["Title"] = prodType;
                TempData["data"] = JsonConvert.SerializeObject(listProducts);
                return RedirectToAction("Products");
            }
        }

        public ActionResult Details(int ProductID)
        {
            using (DBContext ctx = new DBContext())
            {
                ViewBag.LastPage = ControllerContext.HttpContext.Request.UrlReferrer.ToString();
                Product product = ctx.Products.Where(x => x.ID == ProductID).First();
                isRegistered();
                TempData["details"] = product;
                return new EmptyResult();
            }
        }

        public void isRegistered()
        {
            var registered = TempData["NoRegistered"];
            if (registered != null)
            {
                ViewBag.Message = true;
            }
        }

        public ActionResult SendData()
        {
            using (DBContext ctx = new DBContext())
            {
                var query = ctx.Products.ToList();
                isRegistered();
                var Jproduct = JsonConvert.SerializeObject(query);
                ViewBag.fullDataProducts = Jproduct;
                return new EmptyResult();
            }
        }

        public ActionResult Products()
        {
            if(TempData["data"] == null)
            {
                return RedirectToAction("ProductsBase");
            }
            return View();
        }
    }
}
