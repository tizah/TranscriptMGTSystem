using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TranscriptMGTSystem.Models;
using TranscriptModels;

namespace TranscriptMGTSystem.Controllers
{
    public class ApplicantTablesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApplicantTables
        [Authorize]
        public ActionResult Index()
        {
            return View(db.ApplicantTables.ToList());
        }

        // GET: ApplicantTables/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicantTable applicantTable = db.ApplicantTables.Find(id);
            if (applicantTable == null)
            {
                return HttpNotFound();
            }
            return View(applicantTable);
        }

        // GET: ApplicantTables/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicantTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApplicantTableId,Email,FirstName,LastName,OtherName,MatricNo,FacultyName")] ApplicantTable applicantTable)
        {
            if (ModelState.IsValid)
            {
                db.ApplicantTables.Add(applicantTable);
                db.SaveChanges();
                return RedirectToAction("ApplicantConfirmationPage");
            }

            return View(applicantTable);
        }

        public PartialViewResult ApplicantConfirmationPage()
        {
            return PartialView();
        }

        // GET: ApplicantTables/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicantTable applicantTable = db.ApplicantTables.Find(id);
            if (applicantTable == null)
            {
                return HttpNotFound();
            }
            return View(applicantTable);
        }

        // POST: ApplicantTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ApplicantTableId,Email,FirstName,LastName,OtherName,MatricNo,FacultyName")] ApplicantTable applicantTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicantTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicantTable);
        }

        // GET: ApplicantTables/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicantTable applicantTable = db.ApplicantTables.Find(id);
            if (applicantTable == null)
            {
                return HttpNotFound();
            }
            return View(applicantTable);
        }

        // POST: ApplicantTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicantTable applicantTable = db.ApplicantTables.Find(id);
            db.ApplicantTables.Remove(applicantTable);
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
