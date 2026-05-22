using Ders_Seçim_Sistemi.Models.Entities;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

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
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
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

            if (bolum == null)
                return RedirectToAction("BolumListesi");

            // Bölüme bağlı dersler var mı kontrol et
            if (db.Dersler.Any(d => d.BolumId == id) || db.Ogrenciler.Any(o => o.BolumId == id))
            {
                TempData["Hata"] = "Bu bölüme bağlı dersler veya öğrenciler var, silinemez.";
                return RedirectToAction("BolumListesi");
            }

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
            ViewBag.DanismanId = new SelectList(
    db.Danismanlar.Include("Kullanici").ToList()
    .Select(d => new { Id = d.Id, AdSoyad = d.Kullanici.Ad + " " + d.Kullanici.Soyad }),
    "Id", "AdSoyad");
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
        // Danışman Güncelle - GET
        public ActionResult DanismanGuncelle(int id)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            var danisman = db.Danismanlar.Include("Kullanici").FirstOrDefault(d => d.Id == id);
            if (danisman == null)
                return RedirectToAction("DanismanListesi");

            ViewBag.BolumId = new SelectList(db.Bolumler.ToList(), "Id", "Ad", danisman.BolumId);
            return View(danisman);
        }

        // Danışman Güncelle - POST
        [HttpPost]
        public ActionResult DanismanGuncelle(int id, string Ad, string Soyad, string Email, string Parola, int? BolumId)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            if (BolumId == null)
            {
                TempData["Hata"] = "Lütfen tüm alanları doldurunuz.";
                return RedirectToAction("DanismanGuncelle", new { id = id });
            }

            var danisman = db.Danismanlar.Include("Kullanici").FirstOrDefault(d => d.Id == id);
            danisman.Kullanici.Ad = Ad;
            danisman.Kullanici.Soyad = Soyad;
            danisman.Kullanici.Email = Email;
            if (!string.IsNullOrEmpty(Parola))
                danisman.Kullanici.Parola = Parola;
            danisman.BolumId = (int)BolumId;

            db.SaveChanges();
            return RedirectToAction("DanismanListesi");
        }
        // Ders Güncelle - GET
        public ActionResult DersGuncelle(int id)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            var ders = db.Dersler.Find(id);
            if (ders == null)
                return RedirectToAction("DersListesi");

            ViewBag.BolumId = new SelectList(db.Bolumler.ToList(), "Id", "Ad", ders.BolumId);
            ViewBag.DanismanId = new SelectList(
                db.Danismanlar.Include("Kullanici").ToList()
                .Select(d => new { Id = d.Id, AdSoyad = d.Kullanici.Ad + " " + d.Kullanici.Soyad }),
                "Id", "AdSoyad", ders.DanismanId);
            return View(ders);
        }

        // Ders Güncelle - POST
       [HttpPost]
public ActionResult DersGuncelle(int id, string Ad, int? Kredi, int? Kontenjan, int? BolumId, int? DanismanId, string Gun, TimeSpan SaatBaslangic, TimeSpan SaatBitis)
{
    if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
        return RedirectToAction("Login", "Account");

    if (BolumId == null || DanismanId == null || string.IsNullOrEmpty(Gun))
    {
        TempData["Hata"] = "Lütfen tüm alanları doldurunuz.";
        return RedirectToAction("DersGuncelle", new { id = id });
    }

    var ders = db.Dersler.Find(id);
    ders.Ad = Ad;
    ders.Kredi = (int)Kredi;
    ders.Kontenjan = (int)Kontenjan;
    ders.BolumId = (int)BolumId;
    ders.DanismanId = (int)DanismanId;
    ders.Gun = Gun;
    ders.SaatBaslangic = SaatBaslangic;
    ders.SaatBitis = SaatBitis;

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
        public ActionResult DanismanEkle(Kullanici kullanici, int? BolumId)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            if (BolumId == null)
            {
                TempData["Hata"] = "Lütfen bölüm seçiniz.";
                ViewBag.BolumId = new SelectList(db.Bolumler.ToList(), "Id", "Ad");
                return View(kullanici);
            }

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

            // Bağlı ders var mı kontrol et
            if (db.Dersler.Any(d => d.DanismanId == id))
            {
                TempData["Hata"] = "Bu danışmana bağlı dersler var, önce dersleri silin.";
                return RedirectToAction("DanismanListesi");
            }

            // Bağlı öğrenci var mı kontrol et
            if (db.Ogrenciler.Any(o => o.DanismanId == id))
            {
                TempData["Hata"] = "Bu danışmana bağlı öğrenciler var, önce öğrencileri silin.";
                return RedirectToAction("DanismanListesi");
            }

            var danisman = db.Danismanlar.Find(id);
            var kullanici = db.Kullanicilar.Find(danisman.KullaniciId);
            db.Danismanlar.Remove(danisman);
            db.Kullanicilar.Remove(kullanici);
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
            ViewBag.DanismanId = new SelectList(
    db.Danismanlar.Include("Kullanici").ToList()
    .Select(d => new { Id = d.Id, AdSoyad = d.Kullanici.Ad + " " + d.Kullanici.Soyad }),
    "Id", "AdSoyad");
            return View();
        }

        // Öğrenci Ekle - POST
        [HttpPost]
        public ActionResult OgrenciEkle(Kullanici kullanici, int? BolumId, int? DanismanId, string OgrenciNo)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrEmpty(kullanici.Ad) || string.IsNullOrEmpty(kullanici.Soyad) ||
                string.IsNullOrEmpty(kullanici.Email) || string.IsNullOrEmpty(kullanici.Parola) ||
                string.IsNullOrEmpty(OgrenciNo) || BolumId == null || DanismanId == null)
            {
                TempData["Hata"] = "Boş alanları doldurunuz.";
                ViewBag.BolumId = new SelectList(db.Bolumler.ToList(), "Id", "Ad");
                ViewBag.DanismanId = new SelectList(
                    db.Danismanlar.Include("Kullanici").ToList()
                    .Select(d => new { Id = d.Id, AdSoyad = d.Kullanici.Ad + " " + d.Kullanici.Soyad }),
                    "Id", "AdSoyad");
                return View(kullanici);
            }

            kullanici.Rol = "Ogrenci";
            db.Kullanicilar.Add(kullanici);
            db.SaveChanges();

            var ogrenci = new Ogrenci
            {
                KullaniciId = kullanici.Id,
                BolumId = (int)BolumId,
                DanismanId = (int)DanismanId,
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

            // Önce öğrencinin ders seçimlerini sil
            var secimler = db.DersSecimler.Where(s => s.OgrenciId == id).ToList();
            db.DersSecimler.RemoveRange(secimler);

            // Sonra öğrenciyi sil
            var ogrenci = db.Ogrenciler.Find(id);
            var kullanici = db.Kullanicilar.Find(ogrenci.KullaniciId);
            db.Ogrenciler.Remove(ogrenci);
            db.Kullanicilar.Remove(kullanici);

            db.SaveChanges();
            return RedirectToAction("OgrenciListesi");
        }
        // Öğrenci Güncelle - GET
        public ActionResult OgrenciGuncelle(int? id= null)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            var ogrenci = db.Ogrenciler.Include("Kullanici").FirstOrDefault(o => o.Id == id);
            if (ogrenci == null)
                return RedirectToAction("Login", "Account");

            ViewBag.BolumId = new SelectList(db.Bolumler.ToList(), "Id", "Ad", ogrenci.BolumId);
            ViewBag.DanismanId = new SelectList(
                db.Danismanlar.Include("Kullanici").ToList()
                .Select(d => new { Id = d.Id, AdSoyad = d.Kullanici.Ad + " " + d.Kullanici.Soyad }),
                "Id", "AdSoyad", ogrenci.DanismanId);

            return View(ogrenci);
        }

        // Öğrenci Güncelle - POST
        [HttpPost]
        public ActionResult OgrenciGuncelle(int id, string Ad, string Soyad, string Email, string Parola, int? BolumId, int? DanismanId, string OgrenciNo)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            if (BolumId == null || DanismanId == null)
            {
                TempData["Hata"] = "Boş alanları doldurunuz.";
                return RedirectToAction("OgrenciGuncelle", new { id = id });
            }

            var ogrenci = db.Ogrenciler.Include("Kullanici").FirstOrDefault(o => o.Id == id);
            ogrenci.Kullanici.Ad = Ad;
            ogrenci.Kullanici.Soyad = Soyad;
            ogrenci.Kullanici.Email = Email;
            if (!string.IsNullOrEmpty(Parola))
                ogrenci.Kullanici.Parola = Parola;
            ogrenci.OgrenciNo = OgrenciNo;
            ogrenci.BolumId = (int)BolumId;
            ogrenci.DanismanId = (int)DanismanId;

            db.SaveChanges();
            return RedirectToAction("OgrenciListesi");
        }
    }
}


