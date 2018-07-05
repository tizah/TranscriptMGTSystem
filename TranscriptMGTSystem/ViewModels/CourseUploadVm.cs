using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TranscriptMGTSystem.ViewModels
{
    public class CourseUploadVm
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string CreditUnit { get; set; }
    }
}