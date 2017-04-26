using OnlineSales.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSales.Models;
using System.Net;
using System.Net.Mail;
using Newtonsoft.Json;
using OnlineSales.Controllers;

namespace OnlineSales
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult Index()
        {
            using (DBContext ctx = new DBContext())
            {

                ShoppingCartViewModel listModel = new ShoppingCartViewModel();
                listModel.Products = new List<Product>();
                listModel.listUserCards = new List<CreditCard>();
                listModel.ListCarts = new List<CartProduct>();


                int custId = ctx.Customers.Where(x => x.E_mail == User.Identity.Name).First().ID;
                int ShoppingCart = ctx.ShoppingCarts.Where(x => x.CustomerID == custId).First().ID;
                listModel.ListCarts = ctx.CartProducts.Where(x => x.ShoppingCartID == ShoppingCart).ToList();

                foreach (CartProduct item in listModel.ListCarts)
                {
                    listModel.Products.Add(ctx.Products.Where(p => p.ID == item.ProductID).First());
                }

                listModel.listUserCards = ctx.CreditCards.Where(c => c.CustomerID == custId).ToList();
                ViewBag.dataProducts = JsonConvert.SerializeObject(listModel.Products);
                ViewBag.dataQuantity = JsonConvert.SerializeObject(listModel.ListCarts);
                return View(listModel);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JavaScriptResult AddToCart(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                var message = "alert('The item can not be added if you are not registred');";
                return JavaScript(message);
            }
            else
            {
                using (DBContext ctx = new DBContext())
                {
                    CartProduct cart = new CartProduct();
                    int cust = ctx.Customers.Where(x => x.E_mail == User.Identity.Name).First().ID;
                    cart.ShoppingCartID = ctx.ShoppingCarts.Where(x => x.CustomerID == cust).First().ID;
                    cart.ProductID = id;
                    CartProduct hasAny = ctx.CartProducts.Where(x => x.ShoppingCartID == cart.ShoppingCartID && x.ProductID == cart.ProductID).FirstOrDefault();
                    string message;
                    if (hasAny != null)
                    {
                        hasAny.Counter += 1;
                        ctx.SaveChanges();
                        TempData["Added"] = hasAny.Counter;
                        message = "alert('The item was added to your Shopping Cart');";
                        return JavaScript(message);
                    }
                    cart.Counter = 1;
                    ctx.CartProducts.Add(cart);
                    ctx.SaveChanges();
                    TempData["Added"] = 1;
                    message = "alert('The item was added to your Shopping Cart');";
                    return JavaScript(message);
                }
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteFromCart(int Id)
        {
            using (DBContext ctx = new DBContext())
            {
                CartProduct cart = new CartProduct();
                int cust = ctx.Customers.Where(x => x.E_mail == User.Identity.Name).First().ID;
                cart.ShoppingCartID = ctx.ShoppingCarts.Where(x => x.CustomerID == cust).First().ID;
                cart.ProductID = Id;
                CartProduct hasMore = ctx.CartProducts.Where(x => x.ShoppingCartID == cart.ShoppingCartID && x.ProductID == cart.ProductID).First();
                if (hasMore.Counter > 1)
                {
                    hasMore.Counter -= 1;
                    ctx.SaveChanges();
                }
                else
                {
                    ctx.CartProducts.Remove(
                        ctx.CartProducts.Where(x => x.ProductID == cart.ProductID)
                        .Where(x => x.ShoppingCartID == cart.ShoppingCartID).First());
                    ctx.SaveChanges();
                }
            }
            return  new EmptyResult();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Buy(CreditCard model)
        {
            if (model == null)
            {
                TempData["Error"]= 0;
            }
            else
            {
                using (DBContext ctx = new DBContext())
                {
                    CartProduct cart = new CartProduct();
                    int custId = ctx.Customers.Where(x => x.E_mail == User.Identity.Name).First().ID;
                    cart.ShoppingCartID = ctx.ShoppingCarts.Where(x => x.CustomerID == custId).First().ID;
                    List<CartProduct> listCartProducts = new List<CartProduct>();
                    listCartProducts = ctx.CartProducts.Where(x => x.ShoppingCartID == cart.ShoppingCartID).ToList();

                    foreach (var item in listCartProducts)
                    {
                        ctx.CartProducts.Remove(item);
                    }
                    ctx.SaveChanges();
                    TempData["Succes"] = 32;

                    //Code to send the purchase email
                    //var body = "<p>Email From: OnlineSales </p><p>Muchas gracias por tu compra,"
                    //            + " acontinuacion te damos un detalle de las cosas que adquiriste</p>" + "<row>";

                    //foreach (var item in listCartProducts)
                    //{
                    //    var product = ctx.Products.Where(x => x.ID == item.ProductID).FirstOrDefault();

                    //    body += "<li>" + product.Name.ToString() + "</li>";
                    //    body += "<li>" + product.Price.ToString() + " X " + item.Counter.ToString() + " = "
                    //        + Convert.ToString(product.Price * item.Counter) + "</li>";
                    //}
                    //body += "</row>";
                    //var message = new MailMessage();
                    //message.To.Add(new MailAddress(User.Identity.Name.ToString()));
                    //message.Subject = "Compra en OnlineSales";
                    //message.Body = string.Format(body);
                    //message.IsBodyHtml = true;

                    try
                    {
                        Email Cr = new Email();
                        MailMessage mnsj = new MailMessage();

                        mnsj.Subject = "Hola Mundo";

                        mnsj.To.Add(new MailAddress("bryan.ibarra@softtek.com"));

                        mnsj.From = new MailAddress("OnlineSales@Ventas.com", "Test Correo");

                        mnsj.Body = "  Mensaje de Prueba \n\n Enviado desde C#\n\n *VER EL ARCHIVO ADJUNTO*";
                        /* Enviar */
                        Cr.MandarCorreo(mnsj);
                    }
                    catch (Exception e)
                    {
                        return JavaScript("alert('error al enviar el correo');");
                    }
                }
            }
            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}