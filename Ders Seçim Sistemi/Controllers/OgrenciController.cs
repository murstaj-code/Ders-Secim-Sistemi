using Ders_Seçim_Sistemi.Models.Entities;
using log4net;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Ders_Seçim_Sistemi.Controllers
{
    public class OgrenciController : Controller
    {
        private AppDbContext db = new AppDbContext();
        private static readonly ILog log = LogManager.GetLogger(typeof(OgrenciController));

        // Dashboard
        public ActionResult Index()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Ogrenci")
                return RedirectToAction("Login", "Account");

            var kullaniciId = (int)Session["KullaniciId"];
            var ogrenci = db.Ogrenciler
                .Include("Danisman")
                .Include("Danisman.Kullanici")
                .Include("Bolum")
                .FirstOrDefault(o => o.KullaniciId == kullaniciId);

            var aktifDonem = db.Donemler.FirstOrDefault(d => d.AktifMi == true);

            ViewBag.Ogrenci = ogrenci;
            ViewBag.AktifDonem = aktifDonem;

            return View();
        }

        // Ders Seçim sayfası
        public ActionResult DersSecim()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Ogrenci")
                return RedirectToAction("Login", "Account");

            var kullaniciId = (int)Session["KullaniciId"];
            var ogrenci = db.Ogrenciler.FirstOrDefault(o => o.KullaniciId == kullaniciId);
            var aktifDonem = db.Donemler.FirstOrDefault(d => d.AktifMi == true);

            if (aktifDonem == null)
            {
                ViewBag.Mesaj = "Şu an aktif bir dönem bulunmamaktadır.";
                return View();
            }

            var dersler = db.Dersler
                .Where(d => d.BolumId == ogrenci.BolumId)
                .ToList()
                .Where(d => db.DersSecimler.Count(s => s.DersId == d.Id && s.Durum != "Reddedildi") < d.Kontenjan)
                .ToList();

            var secilenDersIds = db.DersSecimler
                .Where(s => s.OgrenciId == ogrenci.Id && s.DonemId == aktifDonem.Id)
                .Select(s => s.DersId)
                .ToList();

            ViewBag.Ogrenci = ogrenci;
            ViewBag.AktifDonem = aktifDonem;
            ViewBag.SecilenDersIds = secilenDersIds;

            return View(dersler);
        }

        // AJAX - Ders seç veya iptal et
        [HttpPost]
        public JsonResult DersSecimYap(int dersId)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Ogrenci")
                return Json(new { basari = false, mesaj = "Yetkisiz erişim." });

            var kullaniciId = (int)Session["KullaniciId"];
            var ogrenci = db.Ogrenciler.FirstOrDefault(o => o.KullaniciId == kullaniciId);
            var aktifDonem = db.Donemler.FirstOrDefault(d => d.AktifMi == true);
            var ders = db.Dersler.Find(dersId);

            // Zaten seçilmiş mi? İptal et
            var mevcutSecim = db.DersSecimler
                .FirstOrDefault(s => s.OgrenciId == ogrenci.Id && s.DersId == dersId && s.DonemId == aktifDonem.Id);

            if (mevcutSecim != null)
            {
                db.DersSecimler.Remove(mevcutSecim);
                db.SaveChanges();
                log.Info($"Ders seçimi kaldırıldı - OgrenciId: {ogrenci.Id}, DersId: {dersId}");
                return Json(new { basari = true, mesaj = "Ders seçimi kaldırıldı.", secildi = false });
            }

            // Kredi limiti kontrolü
            var toplamKredi = db.DersSecimler
                .Where(s => s.OgrenciId == ogrenci.Id && s.DonemId == aktifDonem.Id && s.Durum != "Reddedildi")
                .Sum(s => (int?)s.Ders.Kredi) ?? 0;

            if (toplamKredi + ders.Kredi > 30)
            {
                log.Warn($"Kredi limiti aşıldı - OgrenciId: {ogrenci.Id}, DersId: {dersId}");
                return Json(new { basari = false, mesaj = "Kredi limitini aştınız (max 30)." });
            }

            // Saat çakışması kontrolü
            var seciliDersler = db.DersSecimler
                .Include("Ders")
                .Where(s => s.OgrenciId == ogrenci.Id && s.DonemId == aktifDonem.Id && s.Durum != "Reddedildi")
                .Select(s => s.Ders)
                .ToList();

            var cakisma = seciliDersler.Any(d =>
                d.Gun == ders.Gun &&
                d.SaatBaslangic < ders.SaatBitis &&
                d.SaatBitis > ders.SaatBaslangic);

            if (cakisma)
            {
                log.Warn($"Saat çakışması - OgrenciId: {ogrenci.Id}, DersId: {dersId}");
                return Json(new { basari = false, mesaj = "Bu dersin saati seçtiğiniz başka bir dersle çakışıyor." });
            }

            // Kontenjan kontrolü
            var mevcutKayit = db.DersSecimler
                .Count(s => s.DersId == dersId && s.Durum != "Reddedildi");

            if (mevcutKayit >= ders.Kontenjan)
            {
                log.Warn($"Kontenjan dolu - OgrenciId: {ogrenci.Id}, DersId: {dersId}");
                return Json(new { basari = false, mesaj = "Bu dersin kontenjanı dolmuştur." });
            }

            // Ders seçimi ekle
            var yeniSecim = new DersSecim
            {
                OgrenciId = ogrenci.Id,
                DersId = dersId,
                DonemId = aktifDonem.Id,
                Durum = "Bekliyor"
            };

            db.DersSecimler.Add(yeniSecim);
            db.SaveChanges();
            log.Info($"Ders seçildi - OgrenciId: {ogrenci.Id}, DersId: {dersId}");
            return Json(new { basari = true, mesaj = "Ders seçildi.", secildi = true });
        }

        // Seçilen Dersler sayfası
        public ActionResult SecilenDersler()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Ogrenci")
                return RedirectToAction("Login", "Account");

            var kullaniciId = (int)Session["KullaniciId"];
            var ogrenci = db.Ogrenciler.FirstOrDefault(o => o.KullaniciId == kullaniciId);
            var aktifDonem = db.Donemler.FirstOrDefault(d => d.AktifMi == true);

            if (aktifDonem == null)
            {
                ViewBag.Mesaj = "Şu an aktif bir dönem bulunmamaktadır.";
                return View(new List<DersSecim>());
            }

            var secimler = db.DersSecimler
                .Include("Ders")
                .Where(s => s.OgrenciId == ogrenci.Id && s.DonemId == aktifDonem.Id)
                .ToList();

            return View(secimler);
        }
    }
}