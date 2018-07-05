using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TranscriptModels;

namespace TranscriptMGTSystem.ViewModels
{
    public class StudentTranscriptViewModel
    {

        public Student Student { get; set; }
        public IEnumerable<CourseGrade> Courses { get; set; }
        public IEnumerable<Result> Cgpas { get; set; }
    }
}