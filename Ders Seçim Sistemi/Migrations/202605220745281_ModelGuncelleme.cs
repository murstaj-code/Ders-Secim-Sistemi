namespace Ders_Seçim_Sistemi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelGuncelleme : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DersBolums", "Ders_Id", "dbo.Ders");
            DropForeignKey("dbo.DersBolums", "Bolum_Id", "dbo.Bolums");
            DropIndex("dbo.Ders", new[] { "DanismanId" });
            DropIndex("dbo.DersBolums", new[] { "Ders_Id" });
            DropIndex("dbo.DersBolums", new[] { "Bolum_Id" });
            AddColumn("dbo.Ders", "BolumId", c => c.Int(nullable: false));
            AlterColumn("dbo.Ders", "DanismanId", c => c.Int(nullable: false));
            CreateIndex("dbo.Ders", "BolumId");
            CreateIndex("dbo.Ders", "DanismanId");
            AddForeignKey("dbo.Ders", "BolumId", "dbo.Bolums", "Id");
            DropTable("dbo.DersBolums");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DersBolums",
                c => new
                    {
                        Ders_Id = c.Int(nullable: false),
                        Bolum_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ders_Id, t.Bolum_Id });
            
            DropForeignKey("dbo.Ders", "BolumId", "dbo.Bolums");
            DropIndex("dbo.Ders", new[] { "DanismanId" });
            DropIndex("dbo.Ders", new[] { "BolumId" });
            AlterColumn("dbo.Ders", "DanismanId", c => c.Int());
            DropColumn("dbo.Ders", "BolumId");
            CreateIndex("dbo.DersBolums", "Bolum_Id");
            CreateIndex("dbo.DersBolums", "Ders_Id");
            CreateIndex("dbo.Ders", "DanismanId");
            AddForeignKey("dbo.DersBolums", "Bolum_Id", "dbo.Bolums", "Id");
            AddForeignKey("dbo.DersBolums", "Ders_Id", "dbo.Ders", "Id");
        }
    }
}
