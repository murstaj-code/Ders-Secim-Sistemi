using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ders_Seçim_Sistemi.Models.Entities
{ 
    public class Ogrenci
        {
            public int Id { get; set; }
            public string OgrenciNo { get; set; }
            public int KullaniciId { get; set; }
            public int BolumId { get; set; }
            public int? DanismanId { get; set; }
            public virtual Kullanici Kullanici { get; set; }
            public virtual Bolum Bolum { get; set; }
            public virtual Danisman Danisman { get; set; }
            public virtual ICollection<DersSecim> DersSecimler { get; set; }
        }
}