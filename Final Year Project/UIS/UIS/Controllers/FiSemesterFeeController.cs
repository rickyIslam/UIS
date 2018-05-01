using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UIS.Models;
using System.Text;
using System.Data.Entity.Validation;

namespace UIS.Controllers
{
    public class FiSemesterFeeController : Controller
    {
        UisContext db = new UisContext();
        //
        // GET: /FiSemesterFee/
        public ActionResult FiSubmitInfo()
        {
            var userId = Session["userId"].ToString();
            var studentDetail = from s in db.Students select s;

            if (userId != null)
            {
                studentDetail = studentDetail.Where(c => c.studentId == userId);
            }

            return View(studentDetail);

        }

        //Fi Cart ------------------------------------------------------------------------------- 

        public ActionResult FiCourses(string enrolledSemester)
        {
            Session["FienrolledSemester"] = enrolledSemester;
            var courses = from s in db.cse_courseLayout select s;
            if (courses != null)
            {
                courses = courses.Where(u => u.forSemester == enrolledSemester);
                ViewBag.listCourses = courses;

                // ViewBag.listCourses = db.cse_courseLayout.Where(c => c.forSemester == enrolledSemester).Select(c => c.creditHour).DefaultIfEmpty().Sum();


            }
            return View();

        }

        public ActionResult AddCourses(string courseCode)
        {

            if (Session["cart"] == null)
            {
                List<FiEnrollment> cart = new List<FiEnrollment>();
                cart.Add(new FiEnrollment(db.cse_courseLayout.Find(courseCode)));
                Session["cart"] = cart;
            }
            else
            {
                List<FiEnrollment> cart = (List<FiEnrollment>)Session["cart"];
                cart.Add(new FiEnrollment(db.cse_courseLayout.Find(courseCode)));
                Session["cart"] = cart;
            }

            double totalCreditHour = totalHour();
            double totalAmount = Math.Floor(totalCreditHour * 75)+1;
                              
                //totalCreditHour * 75;

            Session["tch"] = null;
            Session["tam"] = null;


            Session["tch"] = totalCreditHour;
            Session["tam"] = totalAmount;

            //---------------------------------

          

            return View();
        }


        private int isExisting(string courseCode)
        {

            List<FiEnrollment> cart = (List<FiEnrollment>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Courses.courseCode == courseCode)
                    return i;


            return -1;
        }

        private double totalHour()
        {
            double totalCreditHour = 0;
            List<FiEnrollment> cart = (List<FiEnrollment>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                totalCreditHour = (double)totalCreditHour + (double)cart[i].Courses.creditHour;
            }

            return totalCreditHour;
        }

        public ActionResult RemoveCourses(string courseCode)
        {

            int index = isExisting(courseCode);

            // ViewBag.index = index;
            List<FiEnrollment> cart = (List<FiEnrollment>)Session["cart"];
            cart.RemoveAt(index);
            Session["cart"] = cart;

            double totalCreditHour = totalHour();
            double totalAmount = totalCreditHour * 300;
            Session["tch"] = totalCreditHour;
            Session["tam"] = totalAmount;

            return View("AddCourses");
        }


        // Adding FI courses TO Database ,,,,,,,,,,,,, 

        public ActionResult addToDatabase()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addToDatabase(string receiptId, string payForSemester, float amount)
        {
            string errorMessage = null;
            string courseTitle = null;
            string courseCode = null;
            string userId = Session["userId"].ToString();
            string userFaculty = Session["userFaculty"].ToString();


            foreach (FiEnrollment item in (List<FiEnrollment>)Session["cart"])
            {
                courseTitle += item.Courses.courseTitle+",";
                courseCode += item.Courses.courseCode+",";

            }


           

            FiSemesterFee detail = new FiSemesterFee();
            detail.studentId = userId;
            detail.payForSemester = payForSemester;
            detail.receiptId = receiptId;
            detail.amount = amount;
            detail.faculty = userFaculty;
            detail.dateTime = System.DateTime.Now.ToString();
            detail.CourseCode = courseCode;
            detail.CourseTitle = courseTitle;
            detail.status = "In Processing ...";
            // "In Processing ...";

            try
            {
                db.FiSemesterFees.Add(detail);
                db.SaveChanges();
            }
            catch (DbEntityValidationException ee)
            {
                foreach (var error in ee.EntityValidationErrors)
                {
                    foreach (var thisError in error.ValidationErrors)
                    {
                         errorMessage = thisError.ErrorMessage;
                    }
                }
            }

            Session["cart"] = null;
            Session["tch"] = null;
            Session["tam"] = null;

           // return Content(errorMessage);

             return RedirectToAction("CheckStatus", "SemesterFee");
        }

    }
}