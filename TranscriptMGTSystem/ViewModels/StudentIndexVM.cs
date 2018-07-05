using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TranscriptMGTSystem.ViewModels
{
    public class StudentIndexVM
    {
     
            public int StudentId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string FullName { get; set; }


            public string MatricNo { get; set; }
            public string DeptName { get; set; }
            public string FacultyName { get; set; }
            public string Gender { get; set; }
            public string YearOfAttendance { get; set; }
            public string YearOfAward { get; set; }

        }
    
}