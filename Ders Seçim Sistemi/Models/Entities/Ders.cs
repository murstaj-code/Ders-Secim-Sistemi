using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ders_Seçim_Sistemi.Models.Entities
{
    public class Ders
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public int Kredi { get; set; }
        public int Kontenjan { get; set; }
        public int BolumId { get; set; }
        public int? DanismanId { get; set; }
        public string Gun { get; set; }
        public TimeSpan SaatBaslangic { get; set; }
        public TimeSpan SaatBitis { get; set; }
        public virtual Bolum Bolum { get; set; }
        public virtual Danisman Danisman { get; set; }
        public virtual ICollection <DersSecim> DersSecimler { get; set; }
    }
}