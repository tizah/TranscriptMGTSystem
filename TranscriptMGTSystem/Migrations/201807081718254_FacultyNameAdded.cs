namespace TranscriptMGTSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FacultyNameAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Results", "FacultyName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Results", "FacultyName");
        }
    }
}
