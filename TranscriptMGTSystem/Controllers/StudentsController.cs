using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TranscriptMGTSystem.Models;
using TranscriptMGTSystem.ViewModels;
using TranscriptModels;

namespace TranscriptMGTSystem.Controllers
{
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Students
        public ActionResult Index(string searchPerimeter)
        {
            StudentListViewModel model = new StudentListViewModel
            {

                Students = searchPerimeter == null
                                         ? db.Students.ToList()
                                         : db.Students.Where(e => e.MatricNo == searchPerimeter
                                                             || e.YearOfAttendance == searchPerimeter
                                                             || e.DeptName == searchPerimeter
                                                             || e.FacultyName == searchPerimeter
                                                             || e.FirstName == searchPerimeter
                                                             || e.LastName == searchPerimeter
                                                             || e.Nationality == searchPerimeter
                                                             || e.YearOfAward == searchPerimeter
                                                             || e.DeptName == searchPerimeter
                                                             || e.Email == searchPerimeter
                                                             || e.Gender == searchPerimeter
                                                             ).ToList()
            };
            return View(model);
        }

        //this is the method that is used to send Transcript as an email to either a user or a school
       // [HttpPost]
        public JsonResult SendMailToUser(string myemail, string mysubject, HttpPostedFileBase myfile)
        {
            bool result = false;
            result = SendEmail(myemail,mysubject,"this is transcript system from University of Lagos",myfile);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public bool SendEmail(string toEmail, string subject, string body, HttpPostedFileBase myfile)
        {
            try
            {
                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SendEmail"].ToString();

                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SendPassword"].ToString();

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail,senderPassword);
                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subject, body);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                if(myfile != null)
                {
                    mailMessage.Attachments.Add(new Attachment(myfile.InputStream, myfile.FileName));
                }
   
                
             
                client.Send(mailMessage);
                return true;
            }
            catch(Exception e)
            {
                    return false;
            }
        }

        //the partial view for transcript 
        public PartialViewResult TranscriptTemplate(int id)
        {
          
            StudentTranscriptViewModel model = new StudentTranscriptViewModel
            {
                Student = db.Students.Where(x => x.StudentId.Equals((int)id)).FirstOrDefault(),
                Courses = db.CourseGrades.Where(x => x.StudentId.Equals((int)id)).Include(x => x.Course)
                                       .ToList(),
                Cgpas = db.Results.Where(x => x.StudentId.Equals((int)id)).Include(x => x.CourseGrades).ToList()

            };
            return PartialView(model);
        }
        public async Task<ActionResult> GetIndex(string search)
        {
            #region Server Side filtering

            //Get parameter for sorting from grid table
            // get Start (paging start index) and length (page size for paging)
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns values when we click on Header Name of column
            //getting column name
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            //Soring direction(either desending or ascending)
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            string searchResult = Request.Form.GetValues("search[value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var studentIndex = new List<StudentIndexVM>();

            if (!string.IsNullOrEmpty(searchResult))
            {
                var studentList = await db.Students.ToListAsync();
                var v = studentList
                    .Where(x => x.StudentId.Equals(search.ToUpper().Trim())
                                        || x.MatricNo.ToUpper().Equals(search.ToUpper().Trim())
                                        || x.FullName.ToUpper().Equals(search.ToUpper().Trim())
                                        || x.Email.ToUpper().Equals(search.ToUpper().Trim())
                                        || x.DeptName.ToUpper().Equals(search.ToUpper().Trim())
                                         || x.FacultyName.ToUpper().Equals(search.ToUpper().Trim())

                                         || x.YearOfAttendance.ToUpper().Equals(search.ToUpper().Trim()

                                        )
                                        || x.Gender.ToUpper().Equals(search.ToUpper().Trim())
                                        || x.FirstName.ToUpper().Equals(search.ToUpper().Trim())
                                        || x.LastName.ToUpper().Equals(search.ToUpper().Trim())
                                         // || x.OtherNames.ToUpper().Equals(search.ToUpper().Trim())

                                         // || x.DegreeAwared.ToUpper().Equals(search.ToUpper().Trim()

                                         // )

                                         //|| x.ModeOfAdmission.ToUpper().Equals(search.ToUpper().Trim())

                                         || x.YearOfAward.ToUpper().Equals(search.ToUpper().Trim()

                                        )).ToList();
                // Mapping the student to the correct ViewModel for json display
                studentIndex.AddRange(MapToStudentIndex(v));
            }
            else
            {
                var v = await db.Students.ToListAsync();
                // Mapping the student to the correct ViewModel for json display
                studentIndex.AddRange(MapToStudentIndex(v));
            }

            totalRecords = studentIndex.Count();
            var data = studentIndex.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data },
                JsonRequestBehavior.AllowGet);

            #endregion Server Side filtering
        }


        private List<StudentIndexVM> MapToStudentIndex(List<Student> v)
        {
            //var studentIndex = new List<StudentIndexVM>();

            var studentIndex = v.Select(s => new StudentIndexVM()
            {
                StudentId = s.StudentId,
                MatricNo = s.MatricNo,

                FullName = s.FullName,
                Email = s.Email,
                DeptName = s.DeptName,

                FacultyName = s.FacultyName,
                YearOfAttendance = s.YearOfAttendance,

                Gender = s.Gender,



                FirstName = s.FirstName,
                LastName = s.LastName,

                YearOfAward = s.YearOfAward
            }).ToList();



            return studentIndex;
        }


        //student Bio data upload
        public async Task<ActionResult> UploadStudentRecords(HttpPostedFileBase excelfile)
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
                    ExcelValidation myExcel = new ExcelValidation();
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    int requiredField = 14;

                    string validCheck = myExcel.ValidateExcel(noOfRow, workSheet, requiredField);
                    if (!validCheck.Equals("Success"))
                    {
                        //string row = "";
                        //string column = "";
                        string[] ssizes = validCheck.Split(' ');
                        string[] myArray = new string[2];
                        for (int i = 0; i < ssizes.Length; i++)
                        {
                            myArray[i] = ssizes[i];
                            // myArray[i] = ssizes[];
                        }
                        string lineError = $"Line/Row number {myArray[0]}  and column {myArray[1]} is not rightly formatted, Please Check for anomalies ";
                        //ViewBag.LineError = lineError;
                        ViewBag.Message = lineError;
                        // return View("Index");
                    }
                    for (int row = 2; row <= noOfRow; row++)
                    {
                        var matricNo = workSheet.Cells[row, 1].Value.ToString().Trim();
                        var FirstName = workSheet.Cells[row, 2].Value.ToString().Trim();
                        var LastName = workSheet.Cells[row, 3].Value.ToString().Trim();
                        var OtherName = workSheet.Cells[row, 4].Value.ToString().Trim();
                        var Email = workSheet.Cells[row, 5].Value.ToString().Trim();
                        var DeptName = workSheet.Cells[row, 6].Value.ToString().Trim();
                        var FacultyName = workSheet.Cells[row, 7].Value.ToString().Trim();
                        var search = workSheet.Cells[row, 8].Value.ToString().Trim();
                        var ModeOfAdmission = workSheet.Cells[row, 9].Value.ToString().Trim();
                        var Nationality = workSheet.Cells[row, 10].Value.ToString().Trim();
                        var DOB = workSheet.Cells[row, 11].Value.ToString().Trim();
                        var YearOfAttendance = workSheet.Cells[row, 12].Value.ToString().Trim();
                        var DegreeAwarded = workSheet.Cells[row, 13].Value.ToString().Trim();
                        var YearOfAward = workSheet.Cells[row, 14].Value.ToString().Trim();


                        try
                        {
                            var studentMatNo = db.Students.FirstOrDefault(x => x.MatricNo.Equals(matricNo));

                            if (studentMatNo == null)
                            {
                                var student = new Student()
                                {
                                    MatricNo = matricNo,
                                    FirstName = FirstName,
                                    LastName = LastName,
                                    OtherNames = OtherName,
                                    Email = Email,
                                    Gender = search,
                                    FacultyName = FacultyName,
                                    DeptName = DeptName,
                                    ModeOfAdmission = ModeOfAdmission,
                                    Nationality = Nationality,
                                    DateOfBirth = Convert.ToDateTime(DOB),
                                    YearOfAttendance = YearOfAttendance,
                                    DegreeAwared = DegreeAwarded,
                                    YearOfAward = YearOfAward

                                };
                                db.Students.Add(student);
                                //db.SaveChanges();
                            }
                            recordCount++;
                            //lastrecord = $"The last Updated record has the Mat  Number {matricNo} and First Name {FirstName} and LastName {LastName}";

                        }

                        catch (Exception ex)
                        {
                            ViewBag.ErrorInfo = "The Dept code or Semester Name in the excel doesn't exist";
                            ViewBag.ErrorMessage = ex.Message;
                            return View("ErrorException");
                        }
                    }

                    await db.SaveChangesAsync();
                    message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                    ViewBag.Message = message;

                }
                return RedirectToAction("Index", "Students", new { message = message });
            }
            return View();
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TranscriptTemplate((int)id);
            StudentTranscriptViewModel model = new StudentTranscriptViewModel
            {
                Student = db.Students.Where(x => x.StudentId.Equals((int)id)).FirstOrDefault(),
                Courses = db.CourseGrades.Where(x => x.StudentId.Equals((int)id)).Include(x => x.Course)            
                                        .ToList(),
                Cgpas = db.Results.Where(x => x.StudentId.Equals((int)id)).Include(x => x.CourseGrades).ToList()

            };
          

            //.Include(x => x.Cgpas).Where(x => x.StudentId.Equals(id)).ToList();
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,MatricNo,FirstName,OtherNames,LastName,Email,DeptName,FacultyName,Gender,ModeOfAdmission,Nationality,DateOfBirth,YearOfAttendance,DegreeAwared,YearOfAward")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,MatricNo,FirstName,OtherNames,LastName,Email,DeptName,FacultyName,Gender,ModeOfAdmission,Nationality,DateOfBirth,YearOfAttendance,DegreeAwared,YearOfAward")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
