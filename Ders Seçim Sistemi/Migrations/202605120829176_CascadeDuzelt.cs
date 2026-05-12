namespace Ders_Seçim_Sistemi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CascadeDuzelt : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bolums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ad = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ad = c.String(),
                        Kredi = c.Int(nullable: false),
                        Kontenjan = c.Int(nullable: false),
                        BolumId = c.Int(nullable: false),
                        DanismanId = c.Int(),
                        Gun = c.String(),
                        SaatBaslangic = c.Time(nullable: false, precision: 7),
                        SaatBitis = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bolums", t => t.BolumId)
                .ForeignKey("dbo.Danismen", t => t.DanismanId)
                .Index(t => t.BolumId)
                .Index(t => t.DanismanId);
            
            CreateTable(
                "dbo.Danismen",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KullaniciId = c.Int(nullable: false),
                        BolumId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bolums", t => t.BolumId)
                .ForeignKey("dbo.Kullanicis", t => t.KullaniciId)
                .Index(t => t.KullaniciId)
                .Index(t => t.BolumId);
            
            CreateTable(
                "dbo.Kullanicis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ad = c.String(),
                        Soyad = c.String(),
                        Email = c.String(),
                        Parola = c.String(),
                        rol = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ogrencis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OgrenciNo = c.String(),
                        KullaniciId = c.Int(nullable: false),
                        BolumId = c.Int(nullable: false),
                        DanismanId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bolums", t => t.BolumId)
                .ForeignKey("dbo.Danismen", t => t.DanismanId)
                .ForeignKey("dbo.Kullanicis", t => t.KullaniciId)
                .Index(t => t.KullaniciId)
                .Index(t => t.BolumId)
                .Index(t => t.DanismanId);
            
            CreateTable(
                "dbo.DersSecims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OgrenciId = c.Int(nullable: false),
                        DersId = c.Int(nullable: false),
                        DonemId = c.Int(nullable: false),
                        Durum = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ders", t => t.DersId)
                .ForeignKey("dbo.Donems", t => t.DonemId)
                .ForeignKey("dbo.Ogrencis", t => t.OgrenciId)
                .Index(t => t.OgrenciId)
                .Index(t => t.DersId)
                .Index(t => t.DonemId);
            
            CreateTable(
                "dbo.Donems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ad = c.String(),
                        BaslangicTarihi = c.DateTime(nullable: false),
                        BitisTarihi = c.DateTime(nullable: false),
                        AktifMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ogrencis", "KullaniciId", "dbo.Kullanicis");
            DropForeignKey("dbo.DersSecims", "OgrenciId", "dbo.Ogrencis");
            DropForeignKey("dbo.DersSecims", "DonemId", "dbo.Donems");
            DropForeignKey("dbo.DersSecims", "DersId", "dbo.Ders");
            DropForeignKey("dbo.Ogrencis", "DanismanId", "dbo.Danismen");
            DropForeignKey("dbo.Ogrencis", "BolumId", "dbo.Bolums");
            DropForeignKey("dbo.Danismen", "KullaniciId", "dbo.Kullanicis");
            DropForeignKey("dbo.Ders", "DanismanId", "dbo.Danismen");
            DropForeignKey("dbo.Danismen", "BolumId", "dbo.Bolums");
            DropForeignKey("dbo.Ders", "BolumId", "dbo.Bolums");
            DropIndex("dbo.DersSecims", new[] { "DonemId" });
            DropIndex("dbo.DersSecims", new[] { "DersId" });
            DropIndex("dbo.DersSecims", new[] { "OgrenciId" });
            DropIndex("dbo.Ogrencis", new[] { "DanismanId" });
            DropIndex("dbo.Ogrencis", new[] { "BolumId" });
            DropIndex("dbo.Ogrencis", new[] { "KullaniciId" });
            DropIndex("dbo.Danismen", new[] { "BolumId" });
            DropIndex("dbo.Danismen", new[] { "KullaniciId" });
            DropIndex("dbo.Ders", new[] { "DanismanId" });
            DropIndex("dbo.Ders", new[] { "BolumId" });
            DropTable("dbo.Donems");
            DropTable("dbo.DersSecims");
            DropTable("dbo.Ogrencis");
            DropTable("dbo.Kullanicis");
            DropTable("dbo.Danismen");
            DropTable("dbo.Ders");
            DropTable("dbo.Bolums");
        }
    }
}
