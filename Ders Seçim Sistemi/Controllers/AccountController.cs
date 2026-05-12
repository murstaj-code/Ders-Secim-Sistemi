using Ders_Seçim_Sistemi.Models.Entities;
using System.Linq;
using System.Web.Mvc;

namespace Ders_Seçim_Sistemi.Controllers
{
    public class AccountController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Account/Login
        public ActionResult Login()
        {
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
                ViewBag.Hata = "Giriş bilgileriniz eksik veya hatalı.";
                return View();
            }

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
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}