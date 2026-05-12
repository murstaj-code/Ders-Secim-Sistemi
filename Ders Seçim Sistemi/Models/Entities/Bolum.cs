using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Ders_Seçim_Sistemi.Models.Entities
{
    public class Bolum
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public virtual ICollection<Ogrenci> Ogrenciler { get; set; }
        public virtual ICollection<Ders> Dersler { get; set; }
    }
}