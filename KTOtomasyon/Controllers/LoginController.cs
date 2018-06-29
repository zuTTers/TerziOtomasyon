using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using KTOtomasyon.Models;

namespace KTOtomasyon.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Users user)
        {
            using (var db = new KTOtomasyonEntities())
            {
                var userLogin = db.Users.FirstOrDefault(x => x.Mail == user.Mail && x.Password == user.Password && x.IsDeleted == false);

                if (userLogin != null)
                {
                    Session["UserId"] = userLogin.User_Id;
                    Session["Name"] = userLogin.Name;
                    NowSendMail();
                    //Shared.LoginSendMail();
                    //Shared.DefaultSendMail();
                    return RedirectToAction("Index", "Home");
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
                    userdetail = db.Users.Where(d => d.User_Id == id).First();
                }
                return View(userdetail);
            }
        }

        public ActionResult Support()
        {
            return View();
        }

        //Body ve subject mail gönderir
        public ActionResult NowSendMail()
        {
            try
            {
                SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587); //587
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("simpleterzi3428@outlook.com", "3428simple");

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("simpleterzi3428@outlook.com", "Simple Terzi - Axis");
                mail.To.Add(new MailAddress("simpleterzi3428@outlook.com"));
                mail.Bcc.Add(new MailAddress("zubeyir.kocalioglu@gmail.com", "Zübeyir KOÇALİOĞLU"));

                //Mails addmail = new Mails();

                using (var db = new KTOtomasyonEntities())
                {
                    var Data = db.vLastTotalOrder.OrderByDescending(x => x.Sira).ToList();
                    var ThisMessageBody = Data.First();

                    mail.Subject = "Simple Terzi Sipariş Rapor";
                    mail.Body = "Bugün : Toplam sipariş miktarı '";
                    mail.Body += ThisMessageBody.SipMiktar + "' ve sipariş tutarı '" + ThisMessageBody.SipTutar + "'₺ dir.";

                }
                smtp.Send(mail);

            }
            catch (Exception ex)
            {
                ex.AddToDBLog("SendMail", ex.Message);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}