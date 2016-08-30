namespace SoftwareTechnologiesTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProfilesTable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Profiles", new[] { "UserId" });
            RenameColumn(table: "dbo.Profiles", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Profiles", "CreationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Profiles", "User_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Profiles", "User_Id");
            DropColumn("dbo.Profiles", "ProfilePic");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Profiles", "ProfilePic", c => c.String(maxLength: 200));
            DropIndex("dbo.Profiles", new[] { "User_Id" });
            AlterColumn("dbo.Profiles", "User_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.Profiles", "CreationDate");
            RenameColumn(table: "dbo.Profiles", name: "User_Id", newName: "UserId");
            CreateIndex("dbo.Profiles", "UserId");
        }
    }
}
