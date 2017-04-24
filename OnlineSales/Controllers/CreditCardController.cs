using Microsoft.AspNet.Identity;
using OnlineSales.DL;
using OnlineSales.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace OnlineSales.Controllers
{
    public class CreditCardController : Controller
    {
        // GET: CreditCard
        public ActionResult Index()
        {
            using (DBContext ctx = new DBContext())
            {
                var CustomerQuery = ctx.Customers.Where(x => x.E_mail == User.Identity.Name).First();
                var CreditQuery = ctx.CreditCards.Where(x => x.CustomerID == CustomerQuery.ID).ToList();
                return View(CreditQuery);
            }
        }

        public ActionResult RegisterCreditCard()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterCreditCard(CreditCardViewModel model)
        {
            if (ModelState.IsValid)
            {
                CreditCard credit = new CreditCard();
                model.Year += 2000;
                credit.ValidTru = Convert.ToDateTime(model.Month + "/" + "1" + "/" + model.Year);
                using (DBContext ctx = new DBContext())
                {
                    var query = ctx.Customers.Where(x => x.E_mail == User.Identity.Name).First();
                    credit.Number = model.Number;
                    credit.CVV = model.CVV;
                    credit.CustomerID = query.ID;
                    ctx.CreditCards.Add(credit);
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public ActionResult DeleteCreditCard(int ID)
        {
            using (DBContext ctx = new DBContext())
            {
                var query = ctx.CreditCards.Where(x => x.ID == ID).First();
                ctx.CreditCards.Remove(query);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
        }



    }
}