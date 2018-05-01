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
    public class HallFeeController : Controller
    {
        //
        // GET: /HallFee/

        UisContext db = new UisContext();
        public ActionResult HallSubmitInfo()
        {
            var userId = Session["userId"].ToString();
            var studentDetail = from s in db.Students select s;

            if (userId != null)
            {
                studentDetail = studentDetail.Where(c => c.studentId == userId);
            }

            return View(studentDetail);

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult HallSubmitInfo(string receiptId, string payForSemester, float amount)
        {
              string userId = Session["userId"].ToString();
            string userFaculty = Session["userFaculty"].ToString();


            var hallFeeCheck = (from s in db.HallFees where (s.studentId == userId) && (s.payForSemester == payForSemester) select s).FirstOrDefault();

            if(hallFeeCheck == null)
            {
                HallFee hallFee = new HallFee();
                hallFee.studentId = userId;
                hallFee.faculty = userFaculty;
                //
                hallFee.payForSemester = payForSemester;
                hallFee.receiptId = receiptId;
                hallFee.amount = amount;
                hallFee.dateTime = System.DateTime.Now.ToString();
                hallFee.status = "In Processing ...";

                db.HallFees.Add(hallFee);
                db.SaveChanges();
            }
            else
            {
               // ViewBag.availablity = "Information For This Semeser Is In Processing ... ";
                return RedirectToAction("AvailablityResult", "HallFee");
            }

            return RedirectToAction("HallSubmitInfo", "HallFee");
        }

        public ActionResult AvailablityResult()
        {
            return View();
        }

        public ActionResult HallCheckStatus()
        {


            var userId = Session["userId"].ToString();
            var studentFees = from s in db.HallFees select s;


            if (userId != null)
            {
                studentFees = studentFees.Where(c => c.studentId == userId);
            }



            return View(studentFees);



        }


	}
}