using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

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
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
                throw;
            }
        }


        //Veritabanına log ekler.
        public static void AddToDBLog(this Exception exc, string MethodName, string Message="")
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


    }


}