using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ders_Seçim_Sistemi.Models.Entities
{
    public class Danisman
    {
            public int Id { get; set; }
            public int KullaniciId { get; set; }
            public int? BolumId { get; set; }
            public virtual Kullanici Kullanici { get; set; }
            public virtual Bolum Bolum { get; set; }
            public virtual ICollection<Ogrenci> Ogrenciler { get; set; }
            public virtual ICollection<Ders> Dersler { get; set; }
      
    }
}