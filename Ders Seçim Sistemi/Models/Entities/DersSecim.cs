using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ders_Seçim_Sistemi.Models.Entities
{
    public class DersSecim
    {
        public int Id { get; set; }
        public int OgrenciId { get; set; }
        public int DersId { get; set; }
        public int DonemId { get; set; }
        public string Durum { get; set; }

        public virtual Ogrenci Ogrenci { get; set; }
        public virtual Ders Ders { get; set; }
        public virtual Donem Donem { get; set; }
    }
}