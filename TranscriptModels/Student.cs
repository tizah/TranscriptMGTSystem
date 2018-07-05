using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TranscriptModels
{
    public class Student
    {
        public int StudentId { get; set; }
        [Display(Name = "Matric Number")]
        public string MatricNo { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Other Name")]
        public string OtherNames { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => LastName + " " + FirstName + " " + OtherNames;

        public string Email { get; set; }
        [Display(Name = "Department")]
        public string DeptName { get; set; }
        [Display(Name = "Faculty")]
        public string FacultyName { get; set; }
        public string Gender { get; set; }
        [Display(Name = "Mode Of Admission")]
        public string ModeOfAdmission { get; set; }
        public string Nationality { get; set; }
        [Display(Name = "Date Of Birthd")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Year Of Attendance")]
        public string YearOfAttendance { get; set; }
        [Display(Name = "Degree Awarded")]
        public string DegreeAwared { get; set; }
        [Display(Name = "Year Of Award")]
        public string YearOfAward { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }
}
