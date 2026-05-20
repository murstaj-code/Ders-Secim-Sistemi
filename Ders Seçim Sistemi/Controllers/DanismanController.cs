using Ders_Seçim_Sistemi.Models.Entities;
using log4net;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Ders_Seçim_Sistemi.Controllers
{
    public class DanismanController : Controller
    {
        private AppDbContext db = new AppDbContext();
        private static readonly ILog log = LogManager.GetLogger(typeof(DanismanController));

        public ActionResult Index()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Danisman")
                return RedirectToAction("Login", "Account");

            return View();
        }

        // Danışmanın kendi sınıfındaki öğrenciler
        public ActionResult OgrenciListesi()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Danisman")
                return RedirectToAction("Login", "Account");

            var kullaniciId = (int)Session["KullaniciId"];
            var danisman = db.Danismanlar.FirstOrDefault(d => d.KullaniciId == kullaniciId);

            if (danisman == null)
            {
                log.Warn($"Danışman kaydı bulunamadı - KullaniciId: {kullaniciId}");
                ViewBag.Mesaj = "Danışman kaydı bulunamadı.";
                return View(new System.Collections.Generic.List<Ogrenci>());
            }

            var ogrenciler = db.Ogrenciler
                .Include("Kullanici")
                .Include("Bolum")
                .Where(o => o.DanismanId == danisman.Id)
                .ToList();

            return View(ogrenciler);
        }

        // Danışmanın dersini seçen ama kendi sınıfında olmayan öğrenciler
        public ActionResult DanisanListesi()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Danisman")
                return RedirectToAction("Login", "Account");

            var kullaniciId = (int)Session["KullaniciId"];
            var danisman = db.Danismanlar.FirstOrDefault(d => d.KullaniciId == kullaniciId);

            if (danisman == null)
            {
                log.Warn($"Danışman kaydı bulunamadı - KullaniciId: {kullaniciId}");
                ViewBag.Mesaj = "Danışman kaydı bulunamadı.";
                return View(new System.Collections.Generic.List<Ogrenci>());
            }

            var ogrenciler = db.DersSecimler
                .Include("Ogrenci")
                .Include("Ogrenci.Kullanici")
                .Include("Ogrenci.Bolum")
                .Where(s => s.Ders.DanismanId == danisman.Id && s.Ogrenci.DanismanId != danisman.Id)
                .Select(s => s.Ogrenci)
                .Distinct()
                .ToList();

            return View(ogrenciler);
        }

        // Onay paneli
        public ActionResult OnayPaneli(int? ogrenciId = null)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Danisman")
                return RedirectToAction("Login", "Account");

            var kullaniciId = (int)Session["KullaniciId"];
            var danisman = db.Danismanlar.FirstOrDefault(d => d.KullaniciId == kullaniciId);

            if (danisman == null)
            {
                log.Warn($"Danışman kaydı bulunamadı - KullaniciId: {kullaniciId}");
                return View(new System.Collections.Generic.List<DersSecim>());
            }

            var secimler = db.DersSecimler
                .Include("Ders")
                .Include("Ogrenci")
                .Include("Ogrenci.Kullanici")
                .Where(s => s.Ogrenci.DanismanId == danisman.Id);

            if (ogrenciId != null)
                secimler = secimler.Where(s => s.OgrenciId == ogrenciId);

            return View(secimler.ToList());
        }

        // AJAX - Onayla veya Reddet
        [HttpPost]
        public JsonResult SecimGuncelle(int secimId, string durum)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Danisman")
                return Json(new { basari = false, mesaj = "Yetkisiz erişim." });

            var secim = db.DersSecimler.Find(secimId);
            if (secim == null)
            {
                log.Warn($"Seçim bulunamadı - SecimId: {secimId}");
                return Json(new { basari = false, mesaj = "Seçim bulunamadı." });
            }

            secim.Durum = durum;
            db.SaveChanges();

            log.Info($"Ders seçimi güncellendi - SecimId: {secimId}, Durum: {durum}, Danışman: {Session["KullaniciAd"]}");

            return Json(new { basari = true, mesaj = durum + " işlemi başarılı." });
        }
    }
}