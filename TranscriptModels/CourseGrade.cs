using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranscriptModels
{
    public class CourseGrade
    {
        public int CourseGradeId { get; set; }
        public string GradeName { get; set; }


        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
       
    }
}
