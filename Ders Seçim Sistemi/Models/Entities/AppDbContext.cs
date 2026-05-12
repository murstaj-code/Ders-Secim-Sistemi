using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Ders_Seçim_Sistemi.Models.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=DersSecimDB")
        {
        }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Ogrenci> Ogrenciler { get; set; }
        public DbSet<Danisman> Danismanlar { get; set; }
        public DbSet<Bolum> Bolumler { get; set; }
        public DbSet<Ders> Dersler { get; set; }
        public DbSet<Donem> Donemler { get; set; }
        public DbSet<DersSecim> DersSecimler { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.ManyToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}