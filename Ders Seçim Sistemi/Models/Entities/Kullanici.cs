using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ders_Seçim_Sistemi.Models.Entities
{
    public class Kullanici
    {
        public int Id { get; set; }
        public string Ad{ get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Parola { get; set; }   
        public string Rol { get; set; }
    }
}