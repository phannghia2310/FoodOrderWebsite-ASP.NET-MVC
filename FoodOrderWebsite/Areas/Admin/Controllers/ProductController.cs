using FoodOrderWebsite.Context;
using FoodOrderWebsite.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodOrderWebsite.Areas.Admin.Controllers
{ 
    public class ProductController : Controller
    {
        FoodOrderEntities objFoodOrderEntities = new FoodOrderEntities();
        // GET: Admin/Product
        public ActionResult Index(string CurrentFilter, string SearchString, int? page)
        {
            var lstProduct = new List<Product>();
            if(SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = CurrentFilter;
            }

            if(!string.IsNullOrEmpty(SearchString))
            {
                lstProduct = objFoodOrderEntities.Products.Where(p => p.ProductName.Contains(SearchString)).ToList();
            }
            else
            {
                lstProduct = objFoodOrderEntities.Products.ToList();
            }

            ViewBag.CurrentFilter = SearchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            lstProduct = lstProduct.OrderBy(p => p.ProductID).ToList();
            return View(lstProduct.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            var objCategory = objFoodOrderEntities.Categories.ToList();
            ViewBag.Category = new SelectList(objCategory, "CategoryID", "CategoryName");

            return View();
        }

        [HttpPost]
        public ActionResult Create(Product objProduct)
        {
            try
            {
                if (objProduct.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                    string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                    fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                    objProduct.ImageURL = fileName;
                    objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
                }
                objFoodOrderEntities.Products.Add(objProduct);
                objFoodOrderEntities.SaveChanges();
                ModelState.Clear();
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
            var objProduct = objFoodOrderEntities.Products.Where(p => p.ProductID == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var objProduct = objFoodOrderEntities.Products.Where(p => p.ProductID == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var objProduct = objFoodOrderEntities.Products.Where(p => p.ProductID == id).FirstOrDefault();

            objFoodOrderEntities.Products.Remove(objProduct);
            objFoodOrderEntities.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var objCategory = objFoodOrderEntities.Categories.ToList();
            ViewBag.Category = new SelectList(objCategory, "CategoryID", "CategoryName");

            var objProduct = objFoodOrderEntities.Products.Where(p => p.ProductID == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Edit(Product objProduct)
        {
            if (objProduct.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objProduct.ImageURL = fileName;
                objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
            }
            objFoodOrderEntities.Entry(objProduct).State = System.Data.Entity.EntityState.Modified;
            objFoodOrderEntities.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}   