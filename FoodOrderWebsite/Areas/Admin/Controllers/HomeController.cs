using FoodOrderWebsite.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodOrderWebsite.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        FoodOrderEntities objFoodOrderEntities = new FoodOrderEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {
            if(Session["UserID"] == null )
            {
                return Redirect(Url.Content("~/Account/Login"));
            }
            else
            {
                if((bool)Session["Admin"] == true)
                {
                    ViewBag.Title = "Dashboard";
                    return View();
                }
                else
                {
                    return Redirect(Url.Content("~/Home/Index"));
                }
            }
        }
    }
}