namespace TranscriptMGTSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToCourseGrade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourseGrades", "Result_ResultId", c => c.Int());
            CreateIndex("dbo.CourseGrades", "Result_ResultId");
            AddForeignKey("dbo.CourseGrades", "Result_ResultId", "dbo.Results", "ResultId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseGrades", "Result_ResultId", "dbo.Results");
            DropIndex("dbo.CourseGrades", new[] { "Result_ResultId" });
            DropColumn("dbo.CourseGrades", "Result_ResultId");
        }
    }
}
