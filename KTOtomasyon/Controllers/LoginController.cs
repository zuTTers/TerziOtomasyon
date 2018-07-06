using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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
                    //NowSendMail();
                    SendMail();
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
                    var mymaildata = db.vLastTotalOrder.OrderByDescending(x => x.Sira).First();
                    var mylogdata = db.Logs.OrderByDescending(x => x.Log_Id).First();

                    if (mylogdata != null && mymaildata != null)
                    {
                        mail.Subject = "Simple Terzi Sipariş Rapor";
                        mail.Body = "Bugün : Toplam sipariş miktarı '";
                        mail.Body += mymaildata.SipMiktar.Value + "' ve sipariş tutarı '" + mymaildata.SipTutar.Value + "'₺ dir.";
                        mail.Body += "Hata mesajı method adı :  '" + mylogdata.MethodName + "' ve Exception mesajı : '" + mylogdata.ExMessage;
                    }
                    else
                    {
                        mail.Subject = "Simple Terzi Sipariş Rapor";
                        mail.Body = "Bugün, sipariş yok. '";
                        mail.Body += "Hata mesajı method adı :  '" + mylogdata.MethodName + "' ve Exception mesajı : '" + mylogdata.ExMessage;
                    }

                }
                smtp.Send(mail);

            }
            catch (Exception ex)
            {
                ex.AddToDBLog("NowSendMail", ex.Message);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult SendMail()
        {
            SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("simpleterzi3428@outlook.com", "3428simple");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            smtp.Timeout = 600000;


            string body = "";
            String ConnStr = "Data Source=ZUTTERS;Initial Catalog=KTOtomasyon;User ID=zubeyir;Password=123456789;";
            String SQL = "SELECT Sira, SipMiktar, SipTutar FROM vLastTotalOrder "
               + "WHERE Sira IS NOT NULL";
            SqlDataAdapter TitlesAdpt = new SqlDataAdapter(SQL, ConnStr);
            DataSet Titles = new DataSet();
            try
            {
                TitlesAdpt.Fill(Titles);
                
                //Header
                body = "<h3 align='center'>Simple Terzi Haftanın Raporu</h3>";

                //Content
                body += "<table width='70%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0' class='table table-striped text-center' align='center'>";
                body += "<tr bgcolor='#c7d8a7'>";
                body += "<td><strong>Sıra<strong></td>";
                body += "<td><strong>Sipariş Miktarı<strong></td>";
                body += "<td><strong>Sipariş Tutarı<strong></td>";
                body += "</tr>";
                foreach (DataRow Title in Titles.Tables[0].Rows)
                {
                    body += "<tr>";
                    body += "<td>" + Title[0] + ".</td>";
                    body += "<td>" + Title[1] + " Adet </td>";
                    body += "<td>" + String.Format("{0:c}", Title[2]) + "</td>";
                    body += "</tr>";
                }
                body += "</table>";

                //Footer
                body += "</hr>";
                body += "<h5 align='center'>";
                body += "zuTTers, İstanbul 2018 ";
                body += "<a href='twitter.com/zkocalioglu'><font color='red'> İletişim </font></a> | Zübeyir Koçalioğlu tarafından gönderildi.";
                body += "</h5>";

                


                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("simpleterzi3428@outlook.com", "Simple Terzi - Axis");
                mail.To.Add(new MailAddress("simpleterzi3428@outlook.com"));
                mail.Bcc.Add(new MailAddress("zubeyir.kocalioglu@gmail.com", "Zübeyir KOÇALİOĞLU"));
                mail.Subject = "Simple Terzi Rapor";
                mail.Body = body;
                mail.IsBodyHtml = true;

                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                Shared.AddToDBMail(mail.Body, mail.Subject, mail.From.ToString(), mail.To.ToString());

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