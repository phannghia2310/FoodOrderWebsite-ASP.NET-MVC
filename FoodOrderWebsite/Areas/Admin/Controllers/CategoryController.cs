using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodOrderWebsite.Context;

namespace FoodOrderWebsite.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        FoodOrderEntities objFoodOrderEntities = new FoodOrderEntities();

        // GET: Admin/Category
        public ActionResult Index()
        {
            var listCategory = objFoodOrderEntities.Categories.ToList();
            return View(listCategory);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category objCategory)
        {
            try
            {
                objFoodOrderEntities.Categories.Add(objCategory);
                objFoodOrderEntities.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objCategory = objFoodOrderEntities.Categories.Where(c => c.CategoryID == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var objCategory = objFoodOrderEntities.Categories.Where(p => p.CategoryID == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var objCategory = objFoodOrderEntities.Categories.Where(p => p.CategoryID == id).FirstOrDefault();

            objFoodOrderEntities.Categories.Remove(objCategory);
            objFoodOrderEntities.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var objCategory = objFoodOrderEntities.Categories.Where(p => p.CategoryID == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpPost]
        public ActionResult Edit(Category objCategory)
        {
            objFoodOrderEntities.Entry(objCategory).State = System.Data.Entity.EntityState.Modified;
            objFoodOrderEntities.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}