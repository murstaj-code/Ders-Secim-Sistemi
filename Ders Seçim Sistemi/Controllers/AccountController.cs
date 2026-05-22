using Ders_Seçim_Sistemi.Models.Entities;
using log4net;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ders_Seçim_Sistemi.Controllers
{
    public class AccountController : Controller
    {
        private AppDbContext db = new AppDbContext();
        private static readonly ILog log = LogManager.GetLogger(typeof(AccountController));

        // GET: Account/Login
        public ActionResult Login()
        {
            if (Session["Rol"] != null)
            {
                if (Session["Rol"].ToString() == "Admin")
                    return RedirectToAction("Index", "Admin");
                else if (Session["Rol"].ToString() == "Ogrenci")
                    return RedirectToAction("Index", "Ogrenci");
                else if (Session["Rol"].ToString() == "Danisman")
                    return RedirectToAction("Index", "Danisman");
            }
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public ActionResult Login(string email, string parola)
        {
            var kullanici = db.Kullanicilar
                .FirstOrDefault(e => e.Email == email && e.Parola == parola);

            if (kullanici == null)
            {
                log.Warn($"Başarısız giriş denemesi: {email}");
                ViewBag.Hata = "Giriş bilgileriniz eksik veya hatalı.";
                return View();
            }

            log.Info($"Kullanıcı giriş yaptı: {email}, Rol: {kullanici.Rol}");

            Session["KullaniciId"] = kullanici.Id;
            Session["KullaniciAd"] = kullanici.Ad;
            Session["Rol"] = kullanici.Rol;

            if (kullanici.Rol == "Admin")
                return RedirectToAction("Index", "Admin");
            else if (kullanici.Rol == "Ogrenci")
                return RedirectToAction("Index", "Ogrenci");
            else if (kullanici.Rol == "Danisman")
                return RedirectToAction("Index", "Danisman");

            return View();
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            log.Info($"Kullanıcı çıkış yaptı: {Session["KullaniciAd"]}");
            Session.Clear();
            Session.Abandon();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            return RedirectToAction("Login");
        }
    }
}