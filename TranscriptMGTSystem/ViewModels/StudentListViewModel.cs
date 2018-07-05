using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TranscriptModels;

namespace TranscriptMGTSystem.ViewModels
{
    public class StudentListViewModel
    {
        public IEnumerable<Student> Students { get; set; }
    }
}