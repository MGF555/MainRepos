namespace DVDLibrary.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Directors",
                c => new
                    {
                        DirectorId = c.Int(nullable: false, identity: true),
                        DirectorName = c.String(),
                    })
                .PrimaryKey(t => t.DirectorId);
            
            CreateTable(
                "dbo.Dvds",
                c => new
                    {
                        DvdId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ReleaseYear = c.Int(nullable: false),
                        Notes = c.String(),
                        RatingId = c.Int(nullable: false),
                        DirectorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DvdId)
                .ForeignKey("dbo.Directors", t => t.DirectorId, cascadeDelete: true)
                .ForeignKey("dbo.Ratings", t => t.RatingId, cascadeDelete: true)
                .Index(t => t.RatingId)
                .Index(t => t.DirectorId);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        RatingId = c.Int(nullable: false, identity: true),
                        RatingName = c.String(),
                    })
                .PrimaryKey(t => t.RatingId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dvds", "RatingId", "dbo.Ratings");
            DropForeignKey("dbo.Dvds", "DirectorId", "dbo.Directors");
            DropIndex("dbo.Dvds", new[] { "DirectorId" });
            DropIndex("dbo.Dvds", new[] { "RatingId" });
            DropTable("dbo.Ratings");
            DropTable("dbo.Dvds");
            DropTable("dbo.Directors");
        }
    }
}
