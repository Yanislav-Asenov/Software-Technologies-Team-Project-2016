namespace SoftwareTechnologiesTeamProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletedCoeffiecints : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Matches", "HomeCoefficient");
            DropColumn("dbo.Matches", "DrawCoefficient");
            DropColumn("dbo.Matches", "AwayCoefficient");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Matches", "AwayCoefficient", c => c.String());
            AddColumn("dbo.Matches", "DrawCoefficient", c => c.String());
            AddColumn("dbo.Matches", "HomeCoefficient", c => c.String());
        }
    }
}
