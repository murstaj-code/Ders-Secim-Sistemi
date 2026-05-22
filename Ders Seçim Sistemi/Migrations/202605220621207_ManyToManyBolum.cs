namespace Ders_Seçim_Sistemi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyToManyBolum : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ders", "BolumId", "dbo.Bolums");
            DropIndex("dbo.Ders", new[] { "BolumId" });
            CreateTable(
                "dbo.DersBolums",
                c => new
                    {
                        Ders_Id = c.Int(nullable: false),
                        Bolum_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ders_Id, t.Bolum_Id })
                .ForeignKey("dbo.Ders", t => t.Ders_Id)
                .ForeignKey("dbo.Bolums", t => t.Bolum_Id)
                .Index(t => t.Ders_Id)
                .Index(t => t.Bolum_Id);
            
            DropColumn("dbo.Ders", "BolumId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ders", "BolumId", c => c.Int(nullable: false));
            DropForeignKey("dbo.DersBolums", "Bolum_Id", "dbo.Bolums");
            DropForeignKey("dbo.DersBolums", "Ders_Id", "dbo.Ders");
            DropIndex("dbo.DersBolums", new[] { "Bolum_Id" });
            DropIndex("dbo.DersBolums", new[] { "Ders_Id" });
            DropTable("dbo.DersBolums");
            CreateIndex("dbo.Ders", "BolumId");
            AddForeignKey("dbo.Ders", "BolumId", "dbo.Bolums", "Id");
        }
    }
}
