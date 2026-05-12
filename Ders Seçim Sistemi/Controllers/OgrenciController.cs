using Ders_Seçim_Sistemi.Models.Entities;
using System.Linq;
using System.Web.Mvc;

namespace Ders_Seçim_Sistemi.Controllers
{
    public class OgrenciController : Controller
    {
        private AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Ogrenci")
                return RedirectToAction("Login", "Account");

            return View();
        }
    }
}