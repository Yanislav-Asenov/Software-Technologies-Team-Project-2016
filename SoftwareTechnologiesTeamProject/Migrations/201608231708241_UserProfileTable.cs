namespace SoftwareTechnologiesTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProfileTable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Profiles", new[] { "UserId" });
            DropPrimaryKey("dbo.Profiles");
            AlterColumn("dbo.Profiles", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Profiles", "UserId");
            CreateIndex("dbo.Profiles", "UserId");
            DropColumn("dbo.Profiles", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Profiles", "Id", c => c.Int(nullable: false, identity: true));
            DropIndex("dbo.Profiles", new[] { "UserId" });
            DropPrimaryKey("dbo.Profiles");
            AlterColumn("dbo.Profiles", "UserId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.Profiles", "Id");
            CreateIndex("dbo.Profiles", "UserId");
        }
    }
}
