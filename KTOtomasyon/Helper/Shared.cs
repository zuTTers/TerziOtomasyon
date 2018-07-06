using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;

namespace KTOtomasyon.Controllers
{

    public class ReturnValue
    {
        public string error; //Hata mesajı içeriği
        public string message; //Server taraflı işlem sonu dönen mesaj içeriği
        public bool requiredLogin; //Session kontrolü
        public object retObject; //İşlem sonu dönen data objesi
        public bool success; //İşlem başarı kontrolü

        public ReturnValue() { }
    }

    public static class Shared
    {
        //Oturum kontrolu yapar.
        public static bool CheckSession()
        {
            if (HttpContext.Current.Session["UserId"] == null)
            {
                return false;
            }
            return true;
        }

        //Veritabanını yedekler.
        public static void BackupDB()
        {
            SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLKTOtomasyon"].ConnectionString);
            SqlCommand sqlcmd;

            string destdir = "C:\\backupdb";

            if (!System.IO.Directory.Exists(destdir))
            {
                System.IO.Directory.CreateDirectory("C:\\backupdb");
            }
            try
            {
                sqlcon.Open();
                sqlcmd = new SqlCommand("backup database KTOtomasyon to disk='" + destdir + "\\" + DateTime.Now.ToString("dd.MM.yyyy_HHmmss") + ".bak'", sqlcon);
                sqlcmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                AddToDBLog(ex, "Shared.BackupDB", ex.Message);
            }
        }

        //Veritabanını restore eder.
        public static void RestoreDB()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLKTOtomasyon"].ConnectionString);
                SqlCommand sqlcmd;

                //string destdir = "C:\\backupdb\\11082014_121403.Bak";

                sqlcon.Open();
                sqlcmd = new SqlCommand("Restore database KTOtomasyon from disk='C:\\backupdb\\14032018_082848.Bak' ", sqlcon);
                sqlcmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                AddToDBLog(ex, "Shared.RestoreDB", ex.Message);
            }
        }


        //Veritabanına log ekler.
        public static void AddToDBLog(this Exception exc, string MethodName, string Message = "")
        {
            Logs logum = new Logs();

            using (var db = new KTOtomasyonEntities())
            {
                logum.CreatedDate = DateTime.Now;
                logum.CreatedUser = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                logum.ExMessage = exc.ToString();
                logum.MethodName = MethodName;
                logum.Message = Message;

                db.Logs.Add(logum);
                db.SaveChanges();
            }
        }

        //Veritabanına mail ekler.
        public static void AddToDBMail(string subject, string body, string fromadd, string toadd)
        {
            Mails mailim = new Mails();

            using (var db = new KTOtomasyonEntities())
            {
                mailim.CreatedDate = DateTime.Now;
                mailim.CreatedUser = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                mailim.MailTo = toadd.ToString();
                mailim.MailFrom = fromadd.ToString();
                mailim.MailBody = body.ToString();
                mailim.MailSubject = subject.ToString();
                mailim.SendDate = DateTime.Now;
                mailim.IsSend = true;
                mailim.IsBodyHtml = false;

                db.Mails.Add(mailim);
                db.SaveChanges();
            }
        }

        //Sisteme giriş emaili gönderir
        public static void LoginSendMail()
        {
            SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587); //587
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("simpleterzi3428@outlook.com", "3428simple");

            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("simpleterzi3428@outlook.com", "Simple Terzi - Axis");
                mail.To.Add(new MailAddress("simpleterzi3428@outlook.com"));
                mail.Subject = "Simple Terzi Giriş";
                mail.Body = "Simple Terzi giriş yapıldı.";
                mail.Bcc.Add(new MailAddress("zubeyir.kocalioglu@gmail.com", "Zübeyir KOÇALİOĞLU"));

                AddToDBMail(mail.Body, mail.Subject, mail.From.ToString(), mail.To.ToString());

                smtp.Send(mail);

            }
            catch (Exception ex)
            {
                ex.AddToDBLog("SendMail", ex.Message);
            }

        }

        //Body ve subject mail gönderir
        public static void DefaultSendMail()
        {

            ReturnValue retVal = new ReturnValue();
            if (Shared.CheckSession() == false)
            {
                retVal.requiredLogin = true;
                retVal.message = "Lütfen giriş yapınız.";
                retVal.success = false;
                
            }
            try
            {
                retVal.success = false;

                SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587); //587
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("simpleterzi3428@outlook.com", "3428simple");

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("simpleterzi3428@outlook.com", "Simple Terzi - Axis");
                mail.To.Add(new MailAddress("simpleterzi3428@outlook.com"));
                mail.Bcc.Add(new MailAddress("zubeyir.kocalioglu@gmail.com", "Zübeyir KOÇALİOĞLU"));

                Mails addmail = new Mails();
                string SiparisMiktar;
                string SiparisTutar;

                using (var db = new KTOtomasyonEntities())
                {
                    var MailData = db.vLastTotalOrder.OrderByDescending(x => x.Sira).FirstOrDefault();
                    if (MailData != null)
                    {
                        SiparisMiktar = MailData.SipMiktar.Value.ToString();
                        SiparisTutar = MailData.SipTutar.Value.ToString();
                    }
                    else
                    {
                        SiparisMiktar = "Yok";
                        SiparisTutar = "Yok";
                    }

                }

                mail.Subject = "Simple Terzi Sipariş Rapor";
                mail.Body = "Bugün : Toplam sipariş miktarı '";
                mail.Body += SiparisMiktar + "' ve sipariş tutarı '" + SiparisTutar + "'₺ dir.";

                smtp.Send(mail);

                retVal.success = true;
                retVal.message = "mail gönderildi";

            }
            catch (Exception ex)
            {
                ex.AddToDBLog("DefaultSendMail", ex.Message);
                retVal.message = "mail gönderilemedi";
                retVal.error = ex.Message;
                retVal.success = true;
            }

        }


    }


}