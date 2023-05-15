using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FoodOrderWebsite.Context;
using FoodOrderWebsite.Models;

namespace FoodOrderWebsite.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        FoodOrderEntities objFoodOrderEntities = new FoodOrderEntities();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var check = objFoodOrderEntities.Users.FirstOrDefault(u => u.Email == user.Email);
                if (check == null)
                {
                    user.Password = GetMD5(user.Password);
                    objFoodOrderEntities.Configuration.ValidateOnSaveEnabled = false;
                    objFoodOrderEntities.Users.Add(user);
                    objFoodOrderEntities.SaveChanges();
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ViewBag.ErrorRegister = "Email này đã tồn tại";
                    return View();
                }
            }
            return View();
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

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var data = objFoodOrderEntities.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["Name"] = data.FirstOrDefault().Name;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["UserID"] = data.FirstOrDefault().UserID;
                    Session["Admin"] = data.FirstOrDefault().IsAdmin;
                    Session["Image"] = data.FirstOrDefault().ImageURL;
                    if(data.FirstOrDefault().IsAdmin == true)
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "Đăng nhập thất bại";
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Setting(int? id)
        {
            var user = objFoodOrderEntities.Users.Find(id);
            if(user == null)
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
            if(ModelState.IsValid)
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
    }
}