using FoodOrderWebsite.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodOrderWebsite.Areas.Admin.Controllers
{
    public class ContactController : Controller
    {
        FoodOrderEntities objFoodOrderEntities = new FoodOrderEntities();
        // GET: Admin/Contact
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var listContact = objFoodOrderEntities.Contacts.ToList();
            return View(listContact.ToPagedList(pageNumber, pageSize));
        }
    }
}