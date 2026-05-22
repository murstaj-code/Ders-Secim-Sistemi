using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ders_Seçim_Sistemi.Models.Entities
{
    public class Ders
    {
        public int Id { get; set; }

        public string Ad { get; set; }

        public int Kredi { get; set; }

        public int Kontenjan { get; set; }

        public int BolumId { get; set; }

        [ForeignKey("BolumId")]
        public virtual Bolum Bolum { get; set; }

        public int DanismanId { get; set; }

        [ForeignKey("DanismanId")]
        public virtual Danisman Danisman { get; set; }

        public string Gun { get; set; }

        public TimeSpan SaatBaslangic { get; set; }

        public TimeSpan SaatBitis { get; set; }
    }
}