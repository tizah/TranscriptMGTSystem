using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TranscriptMGTSystem.ViewModels
{
    public class ResultVm
    {
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
    }
}