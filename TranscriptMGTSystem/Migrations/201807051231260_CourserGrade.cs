namespace TranscriptMGTSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourserGrade : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseGrades",
                c => new
                    {
                        CourseGradeId = c.Int(nullable: false, identity: true),
                        GradeName = c.String(),
                        CourseId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CourseGradeId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseGrades", "StudentId", "dbo.Students");
            DropForeignKey("dbo.CourseGrades", "CourseId", "dbo.Courses");
            DropIndex("dbo.CourseGrades", new[] { "StudentId" });
            DropIndex("dbo.CourseGrades", new[] { "CourseId" });
            DropTable("dbo.CourseGrades");
        }
    }
}
