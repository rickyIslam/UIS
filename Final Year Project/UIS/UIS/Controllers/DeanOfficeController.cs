using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UIS.Models;
using System.Data.Entity;
using Rotativa;

namespace UIS.Controllers
{
    public class DeanOfficeController : Controller
    {
        //
        // GET: /DeanOffice/
        UisContext db = new UisContext();

        public ActionResult EnrollmentStatus()
        {
            string faculty = "CSE";
            var currentStatus = from s in db.enrollmentStatus select s;
            if (currentStatus != null)
            {
                currentStatus = currentStatus.Where(c => c.faculty == faculty);
            }

            return View(currentStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnrollmentStatus([Bind(Include = "status,faculty")] enrollmentStatu enrollmentStatus)
        {
            if (ModelState.IsValid)
            {

                var model = (from p in db.enrollmentStatus where (p.faculty == enrollmentStatus.faculty) select p).FirstOrDefault();


                model.status = enrollmentStatus.status;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EnrollmentStatus","DeanOffice");
            }
            return View();
        }

        public ActionResult admitCardStatus()
        {
           // string faculty = "CSE";
            var currentStatus = from s in db.adminStatus select s;
            if (currentStatus != null)
            {
                currentStatus = currentStatus.Where(c => c.ID == 1);
            }

            return View(currentStatus);
        }

        [HttpPost,ValidateInput(false)]
        public ActionResult admitCardStatus(string status,string semester,string examDate)
        {

            if (ModelState.IsValid)
            {
                var rowSelect = (from p in db.adminStatus where (p.ID == 1) select p).FirstOrDefault();

                if(status=="ON")
                {
                    rowSelect.admitStatus = status;
                    rowSelect.admitSemester = semester;
                    rowSelect.examDate = examDate;
                }

                else
                {
                    rowSelect.admitStatus = status;
                    rowSelect.admitSemester = "Not Determined!!!";
                    rowSelect.examDate = "Not Determined!!!";
                }

               

                db.Entry(rowSelect).State = EntityState.Modified;
                db.SaveChanges();
              
            }
            return RedirectToAction("admitCardStatus", "DeanOffice");
        }


        public ActionResult downloadPdf()
        {
            var userId = Session["userId"].ToString();
            var report = new ActionAsPdf("studentShip", new {userId = userId});
            return report;
        }

        public ActionResult studentShip(string userId)
        {
           // string userId = functionTest();//Session["userId"].ToString();
            var studentDetail = from s in db.Students select s;

            if (userId != null)
            {
                studentDetail = studentDetail.Where(c => c.studentId == userId);
            }

            return View(studentDetail);

        }

        // regular admit card
        public ActionResult adminCard(string userId)
        {
         //  string userId = Session["userId"].ToString();

           var examDetail = (from s in db.adminStatus where (s.ID == 1) select s).FirstOrDefault();
            if(examDetail != null)
            {
                string semester = examDetail.admitSemester;
                string examDate = examDetail.examDate;

                ViewBag.examSemester = semester;
                ViewBag.examDate = examDate;
            }


            var studentDetail = from s in db.Students select s;

            if (studentDetail != null)
            {
                studentDetail = studentDetail.Where(c => c.studentId == userId);
            }

            return View(studentDetail);
        }

        // FI admit card ---------------------------------------------

        public ActionResult fiAdmitCard(string receiptId)
        {
            //  string userId = Session["userId"].ToString();

            ViewBag.examDate = "12-10-17";

            var studentDetail = from s in db.FiEnrolledStudents select s;

            if (studentDetail != null)
            {
                studentDetail = studentDetail.Where(c => c.receiptId == receiptId);
            }

            return View(studentDetail);
        }

        // FI PDF ADMIT Card ------------------------------------------------------------
        public ActionResult pdfAdmit()
        {
            var userId = Session["userId"].ToString();
            var report = new ActionAsPdf("adminCard", new { userId = userId });
            return report;

        }


        public ActionResult fiPdfAdmit()
        {
           
            var receiptId = Session["receiptId"].ToString();
            var report = new ActionAsPdf("fiAdmitCard", new { receiptId = receiptId });
            return report;
        }




        // Regular admit check ---------------------------------------------------------------------------------------------------------
        public ActionResult admitCheck()
        {
            var auth = (from s in db.adminStatus where (s.admitStatus == "ON") select s).FirstOrDefault(); //db.adminStatus.Single(u => u.admitStatus == "ON");
            var userId = Session["userId"].ToString();
           // var semester = 

            if (auth != null)
            {
                string semester = auth.admitSemester;
               // string admitStatus = auth.admitStatus;
               
                ViewBag.examSemester = semester;
               // ViewBag.examDate = examDate;
                ViewBag.admitCheck = "Dear Student ! Admit Card Providing Session Is Going On !!";

                var transactionCheck = (from s in db.enrolledStudents where (s.studentId == userId) && (s.payForSemester == semester) select s).FirstOrDefault();
                if(transactionCheck != null)
                {
                    ViewBag.abc = "Transaction Matched For This Semester !!!";
                    ViewBag.admitStatus = "ON";
                }
                else
                {
                    ViewBag.abc = "Transaction didn't Matched For This Semester !!!";
                }
            }

            else
            {
                ViewBag.admitCheck = "Dear Student ! Admit Card Providing Session Is Stopped !!";
            }

            return View();
        }

        public ActionResult fiReceiptId()
        {
            return View();
        }


        // -------------------------------FI admit Check ------------------------------------------------------------

   
        [HttpPost]
        public ActionResult fiAdmitCheck(string receiptId)
        {
            var auth = (from s in db.FiEnrolledStudents where (s.receiptId == receiptId) select s).FirstOrDefault();

            if(auth != null)
            {
                Session["receiptId"] = receiptId;
                return RedirectToAction("fiPdfAdmit", "DeanOffice");
            }
            else
            {

            }

            return View();
        }

        public ActionResult enrollReq()
        {

            //var userId = Session["userId"].ToString();
            var selectedStudents = from s in db.SemesterFees select s;


            if (selectedStudents != null)
            {
                selectedStudents = selectedStudents.Where(c => c.status == "Approved!!!");
                selectedStudents = selectedStudents.Where(c => c.hallClearance == "Approved!!!");

            }



            return View(selectedStudents);
        }

        // FI Enroll Request-----

        public ActionResult FiEnrollReq()
        {

            //var userId = Session["userId"].ToString();
            var selectedStudents = from s in db.FiSemesterFees select s;


            if (selectedStudents != null)
            {
                selectedStudents = selectedStudents.Where(c => c.status == "Approved!!!");

            }



            return View(selectedStudents);
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult enrollConfirm(string studentId, string faculty, string dateTime, string payForSemester, string receiptId, float amount)
        {
            enrolledStudent enrolled = new enrolledStudent();
            enrolled.studentId = studentId;
            enrolled.faculty = faculty;
            enrolled.dateTime = dateTime;
            enrolled.payForSemester = payForSemester;
            enrolled.receiptId = receiptId;
            enrolled.amount = amount;

            db.enrolledStudents.Add(enrolled);
            db.SaveChanges();


            var items = (from s in db.SemesterFees where (s.studentId == studentId) && (s.payForSemester == payForSemester) select s).FirstOrDefault();
            db.SemesterFees.Remove(items);
            db.SaveChanges();

            return RedirectToAction("enrollReq");
        }

        // Fi enrollment confirmation...... 

        [HttpPost, ValidateInput(false)]
        public ActionResult FiEnrollConfirm(string studentId, string faculty, string dateTime, string payForSemester, string receiptId, float amount, string CourseTitle, string CourseCode)
        {
            FiEnrolledStudent enrolled = new FiEnrolledStudent();
            enrolled.studentId = studentId;
            enrolled.faculty = faculty;
            enrolled.dateTime = dateTime;
            enrolled.payForSemester = payForSemester;
            enrolled.receiptId = receiptId;
            enrolled.amount = amount;
            enrolled.CourseCode = CourseCode;
            enrolled.CourseTitle = CourseTitle;

            db.FiEnrolledStudents.Add(enrolled);
            db.SaveChanges();


            var items = (from s in db.FiSemesterFees where (s.studentId == studentId) && (s.receiptId == receiptId) select s).FirstOrDefault();
            db.FiSemesterFees.Remove(items);
            db.SaveChanges();

            return RedirectToAction("FiEnrollReq");
        }

        //
        public ActionResult stuCer(string userId)
        {
            //string userId = Session["userId"].ToString();
            var studentDetail = from s in db.Students select s;

            if (userId != null)
            {
                studentDetail = studentDetail.Where(c => c.studentId == userId);
            }

            return View(studentDetail);
        }

        public ActionResult pdfStuCer()
        {
            var userId = Session["userId"].ToString();
            var report = new ActionAsPdf("stuCer", new { userId = userId });
            return report;

        }

        public ActionResult studentShipSubmit()
        {
            string userId = Session["userId"].ToString();
            var studentDetail = from s in db.Students select s;

            if (userId != null)
            {
                studentDetail = studentDetail.Where(c => c.studentId == userId);
            }

            return View(studentDetail);

        }

        // Bank Checking Before Provinding studentship certificatie

        public ActionResult studentShipApprove(string receiptId, string amount)
        {
            var userId = Session["userId"].ToString();
            var checkAvailablity = (from s in db.DeanApprovedEntries where (s.studentId == userId) && (s.receiptId == receiptId) select s).FirstOrDefault();


            if (checkAvailablity != null)
            {
                ViewBag.buttonStatus = "off";
                ViewBag.avlMsg = "This Transaction Is Done Already !!! You Can't Continue !!!";
            }
            else
            {

                ViewBag.buttonStatus = "on";
                ViewBag.avlMsg = "New Transaction !!!";

            }

            ViewBag.studentId = userId;
            ViewBag.receiptId = receiptId;
            ViewBag.amount = amount;
            ViewBag.studentShipList = findAll();


            return View();
        }

        // Function Suru ....
        public IEnumerable<ModelDeanOffice> findAll()
        {
            try
            {
                string baseUrl = "http://localhost:8024/api/";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(

                    new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("deanoffice").Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<IEnumerable<ModelDeanOffice>>().Result;

                }

                else
                {
                    return null;
                }


            }
            catch
            {
                return null;
            }
        }

        //Function Ses



        public ActionResult ApprovementEntry()
        {
            return View();

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ApprovementEntry(string studentId, string faculty, string dateTime, string purpose, string receiptId, float amount)
        {
            DeanApprovedEntry dae = new DeanApprovedEntry();

            dae.studentId = studentId;
            dae.faculty = faculty;
            dae.dateTime = dateTime;
            dae.purpose = purpose;
            dae.receiptId = receiptId;
            dae.amount = amount;

            db.DeanApprovedEntries.Add(dae);
            db.SaveChanges();

            return View();

        }



    }
}