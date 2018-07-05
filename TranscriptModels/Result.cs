using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranscriptModels
{
    public class Result
    {
        public int ResultId { get; set; }
        public string Session { get; set; }
        public string Semester { get; set; }
        public string GradeName { get; set; }
        public int TotalUnitTaken { get; set; }
        public int TotalUnitPassed { get; set; }
        public int TotaGradePoints { get; set; }
        public double GradePointAverage { get; set; }
        public int UnitOfCompulsoryCourse { get; set; }
        public int CumulativeUnitTakenSoFar { get; set; }
        public int CumulativeUnitPassedSoFar { get; set; }
        public int CummulativeGradePoint { get; set; }
        public double CummulativeGradePointAverage { get; set; }

        public string OutstandingCourses { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public virtual ICollection<CourseGrade> CourseGrades { get; set; }
    }
}
