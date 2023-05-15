using FoodOrderWebsite.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodOrderWebsite.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        FoodOrderEntities objFoodOrderEntities = new FoodOrderEntities();
        List<string> statusName = new List<string>() { "Đã hủy" , "Đã xác nhận" };
        // GET: Admin/Order
        public ActionResult Index()
        {
            var listOrder = objFoodOrderEntities.Orders.ToList();
            return View(listOrder);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach(var status in statusName)
            {
                selectList.Add(new SelectListItem()
                {
                    Text = status,
                    Value = (statusName.IndexOf(status)).ToString()
                }) ;
            }

            ViewBag.StatusList = new SelectList(selectList, "Value", "Text");

            var objOrder = objFoodOrderEntities.Orders.Where(o => o.OrderID == id).FirstOrDefault();
            return View(objOrder);
        }

        [HttpPost]
        public ActionResult Edit(Order order) 
        {
            objFoodOrderEntities.Entry(order).State = System.Data.Entity.EntityState.Modified;
            objFoodOrderEntities.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}