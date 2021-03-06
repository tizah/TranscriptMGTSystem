﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranscriptModels
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int CreditUint { get; set; }
        public virtual ICollection<CourseGrade> CourseGrades { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }
}
