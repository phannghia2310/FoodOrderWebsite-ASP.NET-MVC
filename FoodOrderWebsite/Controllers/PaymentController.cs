using FoodOrderWebsite.Context;
using FoodOrderWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodOrderWebsite.Controllers
{
    public class PaymentController : Controller
    {
        FoodOrderEntities objFoodOrderEntities = new FoodOrderEntities();

        // GET: Payment
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    var cart = Session["Cart"] as CartModel;
                    Order order = new Order();
                    order.OrderName = "DonHang-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    order.UserID = int.Parse(Session["UserID"].ToString());
                    order.Total = cart.TotalPrice();
                    order.OrderDate = DateTime.Now;
                    order.Status = 1;
                    objFoodOrderEntities.Orders.Add(order);

                    foreach(var item in cart.Items)
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderID = order.OrderID;
                        orderDetail.ProductID = item._product.ProductID;
                        orderDetail.Price = item._product.Price;
                        orderDetail.Quantity = item._quantity;
                        objFoodOrderEntities.OrderDetails.Add(orderDetail);
                    }
                    objFoodOrderEntities.SaveChanges();
                    cart.ClearCart();
                }
                catch
                {
                    ViewBag.ErrorCheckout = "Đặt hàng không thành công..";
                }
            }
            return View();
        }
    }
}