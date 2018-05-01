using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UIS.Models;

namespace UIS.Controllers
{
    public class HallOfficerController : Controller
    {
        //
        // GET: /HallOfficer/
        UisContext db = new UisContext();
        public ActionResult HallFeeInfoList()
        {
            var studentDetail = from s in db.HallFees select s;

            if (studentDetail != null)
            {
                studentDetail = studentDetail.Where(c => c.status == "In Processing ...");
            }


            return View(studentDetail);

        }


        public ActionResult HallFeeInfoCheck(string id, string semester, float amount)
        {
            var model = (from p in db.HallTransactions where (p.studentId == id) && (p.payForSemester == semester) && (p.amount == amount) select p).FirstOrDefault();

            if(model==null)
            {
                ViewBag.colourStatus = "0";
                ViewBag.HallFeeStatus = "Given Information Is Not Correct !!!";
            }
            else
            {
                ViewBag.colourStatus = "1";
                ViewBag.HallFeeStatus = "Given Information Is Correct !!!";
            }


            var studentDetail = from s in db.SemesterFees select s;

            if (studentDetail != null)
            {
                studentDetail = studentDetail.Where(c => c.studentId == id );
                studentDetail = studentDetail.Where(c => c.payForSemester == semester);
            }


            return View(studentDetail);
        }

        public ActionResult RemoveReq(string id, string semester)
        {
            var query = (from s in db.HallFees where (s.studentId == id) && (s.payForSemester == semester) select s);

            foreach (var item in query)
            {
                db.HallFees.Remove(item);
                db.SaveChanges();
            }

            db.SaveChanges();

            return RedirectToAction("HallFeeInfoList", "HallOfficer");
        }


        [HttpPost]
     //   [ValidateAntiForgeryToken]
        public ActionResult HallApprove(string studentId,string payForSemester)
        {
            if (ModelState.IsValid)
            {

                var semFeeModel = (from p in db.SemesterFees where (p.studentId == studentId) && (p.payForSemester == payForSemester) select p).FirstOrDefault();
                semFeeModel.hallClearance = "Approved!!!";
                db.Entry(semFeeModel).State = EntityState.Modified;
                db.SaveChanges();
               // return RedirectToAction("HallInfoList");
            }

           

            if (ModelState.IsValid)
            {

                var hallFeeModel = (from s in db.HallFees where (s.studentId == studentId) && (s.payForSemester == payForSemester) select s).FirstOrDefault();
                hallFeeModel.status = "Approved!!!";
                db.Entry(hallFeeModel).State = EntityState.Modified;
                db.SaveChanges();
               // return RedirectToAction("HallInfoList");
            }
            return RedirectToAction("HallFeeInfoList");
        }



	}
}