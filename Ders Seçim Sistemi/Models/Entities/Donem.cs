using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ders_Seçim_Sistemi.Models.Entities
{
    public class Donem
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public bool AktifMi { get; set; }
        public virtual ICollection<DersSecim> DersSecimler { get; set; }
    }
}