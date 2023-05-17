using FoodOrderWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodOrderWebsite.Context;

namespace FoodOrderWebsite.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart

        private readonly FoodOrderEntities objFoodOrderEntities;

        public CartController()
        {
            objFoodOrderEntities = new FoodOrderEntities();
        }

        private CartModel GetCart()
        {
            var cart = Session["Cart"] as CartModel;
            if(cart == null)
            {
                cart = new CartModel();
                Session["Cart"] = cart;
            }
            return cart;
        }

        private void SaveCart(CartModel cart)
        {
            Session["Cart"] = cart;
        }

        public ActionResult ShowToCart()
        {
            var cart = GetCart();
            return View(cart);
        }

        public ActionResult AddToCart(int id)
        {
            var product = objFoodOrderEntities.Products.FirstOrDefault(p => p.ProductID == id);
            if(product != null)
            {
                var cart = GetCart();
                cart.Add(product);
                SaveCart(cart);
            }

            return RedirectToAction("ShowToCart");
        }

        public ActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            var product = objFoodOrderEntities.Products.FirstOrDefault(p => p.ProductID == id);
            if(product != null)
            {
                cart.Remove(product);
                SaveCart(cart);
            }

            return RedirectToAction("ShowToCart");
        }

        [HttpPost]
        public ActionResult RemoveAll()
        {
            var cart = GetCart();
            if(cart != null)
            {
                cart.ClearCart();
                SaveCart(cart);
            }
            return RedirectToAction("ShowToCart");
        }

        public ActionResult UpdateCart(FormCollection form) 
        {
            var cart = GetCart();
            int idProduct = int.Parse(form["ProductID"]);
            int newQuantity = int.Parse(form["Quantity"]);
            cart.Update(idProduct, newQuantity);
            return RedirectToAction("ShowToCart", "Cart");
        }

        public PartialViewResult BagCart()
        {
            int total_item = 0;
            var cart = GetCart();
            if(cart != null)
                total_item = cart.TotalQuanity();
                ViewBag.QuantityCart = total_item;
                return PartialView("BagCart");
        }

        public ActionResult CartHistory(int? id)
        {
            var order = objFoodOrderEntities.Orders.Where(o => o.UserID == id).ToList();
            var orderdetail = objFoodOrderEntities.OrderDetails.ToList();

            List<CartHistory> itemshistory = new List<CartHistory>();

            foreach (var o in order)
            {
                foreach (var item in orderdetail)
                {
                    if (item.OrderID == o.OrderID)
                    {
                        var product = objFoodOrderEntities.Products.Find(item.ProductID);
                        var model = new CartHistory
                        {
                            OrderID = item.OrderID,
                            OrderName = o.OrderName,
                            OrderDate = o.OrderDate,
                            ImageURL = product.ImageURL,
                            ProductName = product.ProductName,
                            Status = o.Status,
                            Quantity = item.Quantity,
                            Total = item.Quantity * item.Price,
                        };

                        itemshistory.Add(model);
                    }
                }
            }
            return View(itemshistory);
        }
    }
}