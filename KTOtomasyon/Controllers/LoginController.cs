using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KTOtomasyon.Models;

namespace KTOtomasyon.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public AllList mylist = new AllList();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Users userModel)
        {
            using (var db = new KTOtomasyonEntities())
            {
                var userLogin1 = db.Users.SingleOrDefault(x => x.Mail == userModel.Mail && x.Password == userModel.Password && x.Role == 1 && x.IsDeleted == false);
                var userLogin2 = db.Users.SingleOrDefault(x => x.Mail == userModel.Mail && x.Password == userModel.Password && x.Role == 3 && x.IsDeleted == false);

                if (userLogin1 != null)
                {
                    Session["UserId"] = userLogin1.Id;
                    Session["Name"] = userLogin1.Name;

                    ViewBag.Mesaj = "Hoşgeldiniz Yetkili!";

                    return RedirectToAction("Index", "Home");
                }
                else if (userLogin2 != null)
                {
                    Session["UserId"] = userLogin2.Id;
                    Session["Name"] = userLogin2.Name;

                    ViewBag.Mesaj = "Hoşgeldiniz Yetkili!";

                    return RedirectToAction("Index", "Personel");
                }
                else
                {
                    ViewBag.Mesaj = "Hatalı kullanıcı adı ya da şifre!";
                    return View();
                }
            }
        }

        public ActionResult LogOut()
        {
            Session["UserId"] = null;
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult UserProfile(int id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                Users userdetail = null;

                using (var db = new KTOtomasyonEntities())
                {
                    userdetail = db.Users.Where(d => d.Id == id).First();
                }
                return View(userdetail);
            }
        }
    }
}