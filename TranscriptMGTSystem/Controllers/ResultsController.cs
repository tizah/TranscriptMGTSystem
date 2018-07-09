using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TranscriptMGTSystem.Models;
using TranscriptMGTSystem.ViewModels;
using TranscriptModels;

namespace TranscriptMGTSystem.Controllers
{
    public class ResultsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Results
        public ActionResult Index()
        {
            var results = db.Results.Include(r => r.Course).Include(r => r.Student);
            return View(results.ToList());
        }


        public async Task<ActionResult> UploadStudentGrade(HttpPostedFileBase excelfile)
        {
            HttpPostedFileBase file = Request.Files["excelfile"];
            if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
            {
                string lastrecord = "";
                int recordCount = 0;
                string message = "";
                string fileContentType = file.ContentType;
                byte[] fileBytes = new byte[file.ContentLength];
                var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                // Read data from excel file
                using (var package = new ExcelPackage(file.InputStream))
                {
                    //ExcelValidation myExcel = new ExcelValidation();
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row - 5; //added minus 5 because i want to start reading the excel sheet from the 5th row

                    
                    var courseColumnStop = noOfCol - 14; // added minus 14 because the number of columns apart from the courses are 14 in total

                    // List<string> courseList = new List<string>();
                    var courseListvm = new List<CourseUploadVm>();
                    var courses = new List<Course>();
                    var studentRecord = new Result();
                    for (int row = 1; row <= 1; row++)
                    {
                        var myrow = row;

                        for (int col = 4; col <= courseColumnStop + 3; col++)
                        {

                            var mycourse = new CourseUploadVm()
                            {
                                CourseId = col,
                                CourseName = workSheet.Cells[myrow, col].Value.ToString().Trim() == null ? "." : workSheet.Cells[myrow, col].Value.ToString().Trim() ,
                                CourseCode = workSheet.Cells[myrow + 1, col].Value == null ? string.Empty: workSheet.Cells[myrow + 1, col].Value.ToString().Trim(),
                           
                                CreditUnit = workSheet.Cells[myrow + 2, col].Value == null ? string.Empty : workSheet.Cells[myrow + 2, col].Value.ToString().Trim(),
                            };

                            courseListvm.Add(mycourse);
                            // myrow = myrow + 1;
                            var hasCourse = db.Courses.Where(x => x.CourseCode.Equals(mycourse.CourseCode.ToUpper()) && x.CourseName.Equals(mycourse.CourseName.ToUpper())).FirstOrDefault();

                            if (hasCourse == null)
                            {
                                var myCoursesSaved = new Course()
                                {
                                    CourseCode = mycourse.CourseCode,
                                    CourseName = mycourse.CourseName,
                                    CreditUint = Convert.ToInt32(mycourse.CreditUnit)

                                };

                                // courses.Add(myCoursesSaved);
                                db.Courses.Add(myCoursesSaved);
                                db.SaveChanges();

                            }

                        }

                    }

                    for (int row = 5; row <= noOfRow; row++)
                    {

                        var gradingVm = new List<ResultVm>();
                        var myCourseGrade = new List<CourseGradeVm>();
                        var matricNo = workSheet.Cells[row, 1].Value == null ? string.Empty : workSheet.Cells[row, 1].Value.ToString().Trim();
                        var matricNoFromDb = db.Students.Where(x => x.MatricNo.Equals(matricNo)).Select(x => x.StudentId).FirstOrDefault();
                        if (matricNoFromDb != null)
                        {

                            var Session = workSheet.Cells[row, 2].Value == null  ? string.Empty : workSheet.Cells[row, 2].Value.ToString().Trim();
                            var Semester = workSheet.Cells[row, 3].Value == null ? string.Empty : workSheet.Cells[row, 3].Value.ToString().Trim();
                            var courseGrade = new CourseGrade();
                            for (int col = 4; col <= courseColumnStop + 3; col++)
                            {
                                var courseCode = courseListvm.Where(x => x.CourseId.Equals(col)).Select(s => s.CourseCode).FirstOrDefault();
                                var courseId = db.Courses.Where(x => x.CourseCode.Equals(courseCode))
                                                                       .Select(s => s.CourseId).FirstOrDefault();
                                courseGrade.CourseId = courseId;

                                courseGrade.GradeName = workSheet.Cells[row, col].Value == null ? string.Empty : workSheet.Cells[row, col].Value.ToString().Trim();
                                courseGrade.StudentId = matricNoFromDb;
                                //studentRecord.Session = Session;
                                //studentRecord.Semester = Semester;
                            
                                //write a validation to check for duplicate record
                                db.CourseGrades.Add(courseGrade);
                                db.SaveChanges();

                            }

                            var Startup = courseColumnStop + 4;
                            //essence of writing ternary operator is to test for null cells in the excel sheet and replace with a default value
                            for (int col = Startup; col <= Startup; col++)
                            {
                                ResultVm myGrading = new ResultVm();
                                
                                myGrading.TotalUnitTaken = Convert.ToInt32(workSheet.Cells[row, col].Value == null ? 0 : Convert.ToInt32(workSheet.Cells[row, col].Value.ToString().Trim()));
                                myGrading.TotalUnitPassed = Convert.ToInt32(workSheet.Cells[row, col + 1].Value == null ? 0 : Convert.ToInt32(workSheet.Cells[row, col + 1].Value.ToString().Trim()));
                                myGrading.TotaGradePoints = Convert.ToInt32(workSheet.Cells[row, col + 2].Value == null ? 0 : Convert.ToInt32(workSheet.Cells[row, col + 2].Value == null ? 0 : Convert.ToInt32(workSheet.Cells[row, col + 2].Value.ToString().Trim())));
                                myGrading.GradePointAverage = Convert.ToDouble(workSheet.Cells[row, col + 3].Value == null ? 0 : Convert.ToDouble(workSheet.Cells[row, col + 3].Value.ToString().Trim()));
                                myGrading.UnitOfCompulsoryCourse = Convert.ToInt32(workSheet.Cells[row, col + 4].Value == null ? 0 : Convert.ToInt32(workSheet.Cells[row, col + 4].Value.ToString().Trim()));
                                myGrading.CumulativeUnitTakenSoFar = Convert.ToInt32(workSheet.Cells[row, col + 5].Value == null ? 0 : Convert.ToInt32(workSheet.Cells[row, col + 5].Value.ToString().Trim()));
                                myGrading.CumulativeUnitPassedSoFar = Convert.ToInt32(workSheet.Cells[row, col + 6].Value == null ? 0 : Convert.ToInt32(workSheet.Cells[row, col + 6].Value.ToString().Trim()));
                                myGrading.CummulativeGradePoint = Convert.ToInt32(workSheet.Cells[row, col + 7].Value == null ? 0 : Convert.ToInt32(workSheet.Cells[row, col + 7].Value.ToString().Trim()));
                                myGrading.CummulativeGradePointAverage = Convert.ToDouble(workSheet.Cells[row, col + 8].Value == null ? 0 : Convert.ToDouble(workSheet.Cells[row, col + 8].Value.ToString().Trim()));
                                myGrading.OutstandingCourses = workSheet.Cells[row, col + 9].Value == null ? " "  : workSheet.Cells[row, col + 9].Value.ToString().Trim();
                                myGrading.FacultyName = workSheet.Cells[row, col + 10].Value == null ? " " : workSheet.Cells[row, col + 10].Value.ToString().Trim();
                                //};
                                gradingVm.Add(myGrading);

                                //test if exact record exit in db
                                var mysession = db.Results.Where(x => x.Session.Equals(Session)).ToString();
                                var mysemester = db.Results.Where(x => x.Semester.Equals(Semester)).ToString();

                                var studentIdFromDb = db.Students.Where(x => x.MatricNo.Equals(matricNo)).Select(x => x.StudentId).FirstOrDefault();

                                var studentTranscriptRecord = db.Results.Where(x => x.StudentId.Equals(studentIdFromDb)).FirstOrDefault();

                                if (studentTranscriptRecord != null)
                                {
                                    if (studentTranscriptRecord.CummulativeGradePoint == myGrading.CummulativeGradePoint && studentTranscriptRecord.CummulativeGradePointAverage == myGrading.CummulativeGradePointAverage && studentTranscriptRecord.OutstandingCourses == myGrading.OutstandingCourses && studentTranscriptRecord.Semester == Semester && studentTranscriptRecord.Session == Session && studentTranscriptRecord.GradePointAverage == myGrading.GradePointAverage)
                                    {
                                        // error message to say that record already exist
                                        ViewBag.Message = "this record already exist";

                                    }
                                }
                               
                                else
                                {
                                    studentRecord.Semester = Semester;
                                    studentRecord.Session = Session;
                                    studentRecord.TotalUnitTaken = myGrading.TotalUnitTaken;
                                    studentRecord.TotalUnitPassed = myGrading.TotalUnitPassed;
                                    studentRecord.TotaGradePoints = myGrading.TotaGradePoints;
                                    studentRecord.StudentId = matricNoFromDb;
                                    studentRecord.CourseId = courseGrade.CourseId;
                                    studentRecord.CumulativeUnitTakenSoFar = myGrading.CumulativeUnitTakenSoFar;
                                    studentRecord.CumulativeUnitPassedSoFar = myGrading.CumulativeUnitPassedSoFar;
                                    studentRecord.CummulativeGradePoint = myGrading.CummulativeGradePoint;
                                    studentRecord.CummulativeGradePointAverage = myGrading.CummulativeGradePointAverage;
                                    studentRecord.OutstandingCourses = myGrading.OutstandingCourses;
                                    studentRecord.FacultyName = myGrading.FacultyName;
                                    db.Results.Add(studentRecord);
                                }


                               
                            }

                        }
                       
             
                        message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                        ViewBag.Message = message;

                    }
                   await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Students", new { message = message });
                }
            }
            return View();
        }

        //method to try and eliminate null values in excelsheet
        private int ReplaceNullValues(object value)
        {
            if (value == null || value.ToString() == "." )
            {
                return 0;
            }
            return Convert.ToInt32(value);
        }


        // GET: Results/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // GET: Results/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode");
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "MatricNo");
            return View();
        }

        // POST: Results/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ResultId,Session,Semester,GradeName,TotalUnitTaken,TotalUnitPassed,TotaGradePoints,GradePointAverage,UnitOfCompulsoryCourse,CumulativeUnitTakenSoFar,CumulativeUnitPassedSoFar,CummulativeGradePoint,CummulativeGradePointAverage,OutstandingCourses,StudentId,CourseId")] Result result)
        {
            if (ModelState.IsValid)
            {
                db.Results.Add(result);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", result.CourseId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "MatricNo", result.StudentId);
            return View(result);
        }

        // GET: Results/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", result.CourseId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "MatricNo", result.StudentId);
            return View(result);
        }

        // POST: Results/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResultId,Session,Semester,GradeName,TotalUnitTaken,TotalUnitPassed,TotaGradePoints,GradePointAverage,UnitOfCompulsoryCourse,CumulativeUnitTakenSoFar,CumulativeUnitPassedSoFar,CummulativeGradePoint,CummulativeGradePointAverage,OutstandingCourses,StudentId,CourseId")] Result result)
        {
            if (ModelState.IsValid)
            {
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", result.CourseId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "MatricNo", result.StudentId);
            return View(result);
        }

        // GET: Results/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // POST: Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Result result = db.Results.Find(id);
            db.Results.Remove(result);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
