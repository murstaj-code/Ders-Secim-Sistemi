using Ders_Seçim_Sistemi.Models.Entities;
using System.Linq;
using System.Web.Mvc;

namespace Ders_Seçim_Sistemi.Controllers
{
    public class AdminController : Controller
    {
        private AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            return View();
        }
        // Bölüm Listesi
        public ActionResult BolumListesi()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            var bolumler = db.Bolumler.ToList();
            return View(bolumler);
        }

        // Bölüm Ekle - GET
        public ActionResult BolumEkle()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            return View();
        }

        // Bölüm Ekle - POST
        [HttpPost]
        public ActionResult BolumEkle(Bolum bolum)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            db.Bolumler.Add(bolum);
            db.SaveChanges();
            return RedirectToAction("BolumListesi");
        }

        // Bölüm Sil
        public ActionResult BolumSil(int id)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            var bolum = db.Bolumler.Find(id);
            db.Bolumler.Remove(bolum);
            db.SaveChanges();
            return RedirectToAction("BolumListesi");
        }

        // Dönem Listesi
public ActionResult DonemListesi()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            var donemler = db.Donemler.ToList();
            return View(donemler);
        }

        // Dönem Ekle - GET
        public ActionResult DonemEkle()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            return View();
        }

        // Dönem Ekle - POST
        [HttpPost]
        public ActionResult DonemEkle(Donem donem)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            db.Donemler.Add(donem);
            db.SaveChanges();
            return RedirectToAction("DonemListesi");
        }

        // Dönem Sil
        public ActionResult DonemSil(int id)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            var donem = db.Donemler.Find(id);
            db.Donemler.Remove(donem);
            db.SaveChanges();
            return RedirectToAction("DonemListesi");
        }

        // Dönem Aktif/Pasif
        public ActionResult DonemAktifDegistir(int id)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            var donem = db.Donemler.Find(id);
            donem.AktifMi = !donem.AktifMi;
            db.SaveChanges();
            return RedirectToAction("DonemListesi");
        }
            // Ders Listesi
public ActionResult DersListesi()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            var dersler = db.Dersler.Include("Bolum").Include("Danisman").ToList();
            return View(dersler);
        }

        // Ders Ekle - GET
        public ActionResult DersEkle()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            ViewBag.BolumId = new SelectList(db.Bolumler.ToList(), "Id", "Ad");
            ViewBag.DanismanId = new SelectList(db.Danismanlar.ToList(), "Id", "Id");
            return View();
        }

        // Ders Ekle - POST
        [HttpPost]
        public ActionResult DersEkle(Ders ders)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            db.Dersler.Add(ders);
            db.SaveChanges();
            return RedirectToAction("DersListesi");
        }

        // Ders Sil
        public ActionResult DersSil(int id)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            var ders = db.Dersler.Find(id);
            db.Dersler.Remove(ders);
            db.SaveChanges();
            return RedirectToAction("DersListesi");
        }

        // Danışman Listesi
        public ActionResult DanismanListesi()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            var danismanlar = db.Danismanlar.Include("Kullanici").ToList();
            return View(danismanlar);
        }

        // Danışman Ekle - GET
        public ActionResult DanismanEkle()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            ViewBag.BolumId = new SelectList(db.Bolumler.ToList(), "Id", "Ad");
            return View();
        }

        // Danışman Ekle - POST
        [HttpPost]
        public ActionResult DanismanEkle(Kullanici kullanici, int BolumId)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            kullanici.Rol = "Danisman";
            db.Kullanicilar.Add(kullanici);
            db.SaveChanges();

            var danisman = new Danisman { KullaniciId = kullanici.Id, BolumId = BolumId };
            db.Danismanlar.Add(danisman);
            db.SaveChanges();

            return RedirectToAction("DanismanListesi");
        }

        // Danışman Sil
        public ActionResult DanismanSil(int id)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            var danisman = db.Danismanlar.Find(id);
            db.Danismanlar.Remove(danisman);
            db.SaveChanges();
            return RedirectToAction("DanismanListesi");
        }

        // Öğrenci Listesi
        public ActionResult OgrenciListesi()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            var ogrenciler = db.Ogrenciler.Include("Kullanici").Include("Bolum").Include("Danisman").ToList();
            return View(ogrenciler);
        }

        // Öğrenci Ekle - GET
        public ActionResult OgrenciEkle()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            ViewBag.BolumId = new SelectList(db.Bolumler.ToList(), "Id", "Ad");
            ViewBag.DanismanId = new SelectList(db.Danismanlar.Include("Kullanici").ToList(), "Id", "Id");
            return View();
        }

        // Öğrenci Ekle - POST
        [HttpPost]
        public ActionResult OgrenciEkle(Kullanici kullanici, int BolumId, int DanismanId, string OgrenciNo)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            kullanici.Rol = "Ogrenci";
            db.Kullanicilar.Add(kullanici);
            db.SaveChanges();

            var ogrenci = new Ogrenci
            {
                KullaniciId = kullanici.Id,
                BolumId = BolumId,
                DanismanId = DanismanId,
                OgrenciNo = OgrenciNo
            };
            db.Ogrenciler.Add(ogrenci);
            db.SaveChanges();

            return RedirectToAction("OgrenciListesi");
        }

        // Öğrenci Sil
        public ActionResult OgrenciSil(int id)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            var ogrenci = db.Ogrenciler.Find(id);
            db.Ogrenciler.Remove(ogrenci);
            db.SaveChanges();
            return RedirectToAction("OgrenciListesi");
        }
    }

}


