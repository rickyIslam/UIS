using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UIS.Models;
using System.Net;
using System.Dynamic;

namespace UIS.Controllers
{

    public class SemesterFeeController : Controller
    {
        private UisContext db = new UisContext();
        //
        // GET: /SemesterFee/
        //সেমিষ্টার ফি আপডেটের পেজে কিছু স্টুডেন্ট এর কিছু ইনফরমেশন্ দেখানোর জন্য ... 
        public ActionResult SubmitInfo()
        {
            var userId = Session["userId"].ToString();
            var studentDetail = from s in db.Students select s;
            var varable = db.enrollmentStatus.SingleOrDefault(u => u.faculty == "CSE");
            string enrollmentStatus = varable.status.ToString();

            ViewBag.status = enrollmentStatus;


            if (userId != null)
            {
                studentDetail = studentDetail.Where(c => c.studentId == userId);
            }

            return View(studentDetail);

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SubmitInfo(string receiptId, string payForSemester, float amount)
        {
              string userId = Session["userId"].ToString();
              string userFaculty = Session["userFaculty"].ToString();


              
                SemesterFee semesterFee = new SemesterFee();
                semesterFee.studentId = userId;
                semesterFee.faculty = userFaculty;
                //
                semesterFee.payForSemester = payForSemester;
                semesterFee.receiptId = receiptId;
                semesterFee.amount = amount;
                semesterFee.dateTime = System.DateTime.Now.ToString();
                semesterFee.hallClearance = "In Processing ...";
                semesterFee.status = "In Processing ...";

                db.SemesterFees.Add(semesterFee);
                db.SaveChanges();

            return RedirectToAction("SubmitInfo");
        }

        public ActionResult CheckStatus()
        {

                dynamic dynamicModel = new ExpandoObject();
                var userId = Session["userId"].ToString();
                var studentFees = from s in db.SemesterFees select s;
                var fiStudentFees = from s in db.FiSemesterFees select s;
               

                if (userId != null)
                {
                    studentFees = studentFees.Where(c => c.studentId == userId);
                    fiStudentFees = fiStudentFees.Where(d => d.studentId == userId);
                }

                dynamicModel.semesterFees = studentFees;
                dynamicModel.fiSemesterFees = fiStudentFees;


                return View(dynamicModel);
           
          
           
        }

        public ActionResult StudentProfileForAccountant()
        {
            return View();
        }

        public ActionResult enrollingCourses(string enrolledSemester)
        {

            string faculty = Session["userFaculty"].ToString();
            string userId = Session["userId"].ToString();

            var semFeeCheck = (from p in db.SemesterFees where (p.studentId == userId) && (p.payForSemester == enrolledSemester) select p).FirstOrDefault();
            var enrolledTableCheck = (from s in db.enrolledStudents where (s.studentId == userId) && (s.payForSemester == enrolledSemester) select s).FirstOrDefault();

            if (semFeeCheck == null && enrolledTableCheck == null)
            {
                
                Session["enrolledSemester"] = enrolledSemester;

                // if faculty is CSE 
                if (faculty == "CSE")
                {
                    var courses = from s in db.cse_courseLayout select s;
                    if (courses != null)
                    {
                        courses = courses.Where(u => u.forSemester == enrolledSemester);

                        ViewBag.cseCreditHour = db.cse_courseLayout.Where(c => c.forSemester == enrolledSemester).Select(c => c.creditHour).DefaultIfEmpty().Sum();

                        return View(courses);
                    }

                   // return View();
                }
            }
            else
            {
               // TempData["checkAvailablityMessage"] = " You Already Have a Pending Request !!! ";
               // ViewBag.availablity = "Information For This Semeser Is In Processing ... ";

                return RedirectToAction ("AvailablityResult", "SemesterFee");

            }


         return View();

        }

        public ActionResult AvailablityResult ()
        {
            return View();
        }

        public ActionResult EnrolledSemesters()
        {
            dynamic dynamicModel = new ExpandoObject();
            var userId = Session["userId"].ToString();
            var studentFees = from s in db.enrolledStudents select s;
            var fiStudentFees = from s in db.FiEnrolledStudents select s;


            if (userId != null)
            {
                studentFees = studentFees.Where(c => c.studentId == userId);
            }

            if (userId != null)
            {
                fiStudentFees = fiStudentFees.Where(c => c.studentId == userId);
            }

            dynamicModel.semesterFees = studentFees;
            dynamicModel.fiSemesterFees = fiStudentFees;

            return View(dynamicModel);
          
        }

        // Fi Course suruuuuuuuuu 
        public ActionResult FiCourses ()
        {
            ViewBag.listCourses = db.cse_courseLayout.ToList();
            return View();
        }

        public ActionResult AddCourses (string courseCode)
        {
          
            if (Session["cart"] == null)
            {
                List<FiEnrollment> cart = new List<FiEnrollment>();
               cart.Add(new FiEnrollment(db.cse_courseLayout.Find(courseCode)));
               Session["cart"] = cart;
            }
            else
            {
                 List<FiEnrollment> cart =  (List<FiEnrollment>)Session["cart"];
               cart.Add(new FiEnrollment(db.cse_courseLayout.Find(courseCode)));
               Session["cart"] = cart;
            }

            double totalCreditHour = totalHour();
            double totalAmount = totalCreditHour * 75;
            ViewBag.tch = totalCreditHour;
            ViewBag.tam = totalAmount;
          

                return View();
        }


        private int isExisting (string courseCode)
        {
           
            List<FiEnrollment> cart = (List<FiEnrollment>)Session["cart"];
            for (int i=0; i<cart.Count; i++)
                if (cart[i].Courses.courseCode == courseCode)
                return i ;
            

            return -1;
        }

        private double totalHour ()
        {
            double totalCreditHour = 0;
            List<FiEnrollment> cart = (List<FiEnrollment>)Session["cart"];
            for(int i=0;i<cart.Count;i++)
            {
                totalCreditHour = (double) totalCreditHour + (double) cart[i].Courses.creditHour;
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
            ViewBag.tch = totalCreditHour;
            ViewBag.tam = totalAmount;

            return View("AddCourses");
        }
   
    }
}