using FoodOrderWebsite.Context;
using FoodOrderWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FoodOrderWebsite.Controllers
{
    public class HomeController : Controller
    {
        FoodOrderEntities objFoodOrderEntities = new FoodOrderEntities();
        public ActionResult Index()
        {
            HomeModel objHome = new HomeModel();
            objHome.listProduct = objFoodOrderEntities.Products.ToList();
            return View(objHome);
        }

        public ActionResult Menu()
        {
            HomeModel objHome = new HomeModel();

            objHome.listCategory = objFoodOrderEntities.Categories.ToList();
            objHome.listProduct = objFoodOrderEntities.Products.ToList();

            return View(objHome);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(Contact contact)
        {
            try
            {
                objFoodOrderEntities.Contacts.Add(contact);
                objFoodOrderEntities.SaveChanges();

                //ViewBag.SuccessContact = "Gửi thành công";
                return RedirectToAction("Contact");
                
            }
            catch (Exception)
            {
                ViewBag.ErrorContact = "Gửi không thành công";
                return View();
            }
        }
    }
}