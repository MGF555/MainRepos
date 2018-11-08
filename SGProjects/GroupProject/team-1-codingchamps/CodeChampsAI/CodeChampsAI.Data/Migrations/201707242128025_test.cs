namespace CodeChampsAI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        IsFeatured = c.Boolean(nullable: false),
                        ApprovalStatus = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Subject = c.String(maxLength: 100),
                        Body = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagId = c.Int(nullable: false, identity: true),
                        TagName = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.TagId);
            
            CreateTable(
                "dbo.StaticPages",
                c => new
                    {
                        StaticPageId = c.Int(nullable: false, identity: true),
                        Subject = c.String(maxLength: 100),
                        Body = c.String(),
                        LinkName = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.StaticPageId);
            
            CreateTable(
                "dbo.TagPosts",
                c => new
                    {
                        Tag_TagId = c.Int(nullable: false),
                        Post_PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagId, t.Post_PostId })
                .ForeignKey("dbo.Tags", t => t.Tag_TagId)
                .ForeignKey("dbo.Posts", t => t.Post_PostId)
                .Index(t => t.Tag_TagId)
                .Index(t => t.Post_PostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TagPosts", "Post_PostId", "dbo.Posts");
            DropForeignKey("dbo.TagPosts", "Tag_TagId", "dbo.Tags");
            DropIndex("dbo.TagPosts", new[] { "Post_PostId" });
            DropIndex("dbo.TagPosts", new[] { "Tag_TagId" });
            DropIndex("dbo.Posts", new[] { "UserId" });
            DropTable("dbo.TagPosts");
            DropTable("dbo.StaticPages");
            DropTable("dbo.Tags");
            DropTable("dbo.Posts");
        }
    }
}
