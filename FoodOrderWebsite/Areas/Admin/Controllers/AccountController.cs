using FoodOrderWebsite.Context;
using FoodOrderWebsite.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FoodOrderWebsite.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        FoodOrderEntities objFoodOrderEntities = new FoodOrderEntities();
        // GET: Admin/Account
        public ActionResult Index()
        {
            var listUser = objFoodOrderEntities.Users.ToList();
            return View(listUser);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var check = objFoodOrderEntities.Users.FirstOrDefault(u => u.Email == user.Email);
                if (check == null)
                {
                    if(user.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(user.ImageUpload.FileName);
                        string extension = Path.GetExtension(user.ImageUpload.FileName);
                        fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                        user.ImageURL = fileName;
                        user.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/members/"), fileName));
                    }

                    user.Password = GetMD5(user.Password);
                    objFoodOrderEntities.Configuration.ValidateOnSaveEnabled = false;
                    objFoodOrderEntities.Users.Add(user);
                    objFoodOrderEntities.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorRegister = "Email này đã tồn tại";
                    return View();
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var objUser = objFoodOrderEntities.Users.Where(u => u.UserID == id).FirstOrDefault();
            return View(objUser);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var objUser = objFoodOrderEntities.Users.Where(u => u.UserID == id).FirstOrDefault();

            objFoodOrderEntities.Users.Remove(objUser);
            objFoodOrderEntities.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var objUser = objFoodOrderEntities.Users.Where(u => u.UserID == id).FirstOrDefault();
            return View(objUser);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (user.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(user.ImageUpload.FileName);
                string extension = Path.GetExtension(user.ImageUpload.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                user.ImageURL = fileName;
                user.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/members/"), fileName));
            }

            user.Password = GetMD5(user.Password);
            objFoodOrderEntities.Configuration.ValidateOnSaveEnabled = false;
            objFoodOrderEntities.Entry(user).State = System.Data.Entity.EntityState.Modified;
            objFoodOrderEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Setting(int? id)
        {
            var user = objFoodOrderEntities.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new UserSetting
            {
                UserID = user.UserID,
                Name = user.Name,
                Phone = user.Phone,
                Email = user.Email,
                Address = user.Address,
                Password = user.Password,
                ImageURL = user.ImageURL,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setting(UserSetting model)
        {
            if (ModelState.IsValid)
            {

                if (model.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(model.ImageUpload.FileName);
                    string extension = Path.GetExtension(model.ImageUpload.FileName);
                    fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                    model.ImageURL = fileName;
                    model.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/customers/"), fileName));
                }

                model.Password = GetMD5(model.Password);

                var user = new User
                {
                    UserID = model.UserID,
                    Name = model.Name,
                    Phone = model.Phone,
                    Email = model.Email,
                    Address = model.Address,
                    Password = model.Password,
                    ImageURL = model.ImageURL,
                };

                objFoodOrderEntities.Configuration.ValidateOnSaveEnabled = false;
                objFoodOrderEntities.Entry(user).State = System.Data.Entity.EntityState.Modified;
                objFoodOrderEntities.SaveChanges();
                return RedirectToAction("Setting", "Account");
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect(Url.Content("~/Account/Login"));
        }

        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }
    }
}