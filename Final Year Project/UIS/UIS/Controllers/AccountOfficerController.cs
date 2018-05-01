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

namespace UIS.Controllers
{
    public class AccountOfficerController : Controller
    {
        UisContext db = new UisContext();

      

        public ActionResult SemInfoList()
        {
            var studentDetail = from s in db.SemesterFees select s;

            if (studentDetail != null)
            {
                studentDetail = studentDetail.Where(c => c.status == "In Processing ...");
            }


            return View(studentDetail);

        }

        public ActionResult FiSemInfoList()
        {
            var studentDetail = from s in db.FiSemesterFees select s;

            if (studentDetail != null)
            {
                studentDetail = studentDetail.Where(c => c.status == "In Processing ...");
            }


            return View(studentDetail);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve([Bind(Include = "studentId, dateTime, payForSemester, receiptId, amount, status")] SemesterFee semesterFee)
        {
            if (ModelState.IsValid)
            {

                var model = (from p in db.SemesterFees where (p.studentId == semesterFee.studentId) && (p.payForSemester == semesterFee.payForSemester)select p).FirstOrDefault();


                model.status = "Approved!!!";
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SemInfoList");
            }
            return View(semesterFee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FiApproval([Bind(Include = "studentId, dateTime, payForSemester, receiptId, amount, status")] FiSemesterFee semesterFee)
        {
            if (ModelState.IsValid)
            {

                var model = (from p in db.FiSemesterFees where (p.studentId == semesterFee.studentId) && (p.receiptId == semesterFee.receiptId) select p).FirstOrDefault();


                model.status = "Approved!!!";
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
               
            }
            return RedirectToAction("FiSemInfoList");
        }

        public ActionResult RemoveReq(string id, string semester)
        {
            var query = (from s in db.SemesterFees where (s.studentId == id) && (s.payForSemester == semester) select s);

            foreach (var item in query)
            {
                db.SemesterFees.Remove(item);
                db.SaveChanges();
            }

           // db.SemesterFees.Remove(query);

            //Customer customer = db.Customers.Find(id);
            //db.Customers.Remove(customer);
            db.SaveChanges();

            return RedirectToAction("SemInfoList", "AccountOfficer");
        }

        public ActionResult SemFeeInfoCheck(string id, string semester, string amount)
        {
            ViewBag.studentId = id;
            ViewBag.semester = semester;
            ViewBag.amount = amount;

            ViewBag.queryList = findAll();
            return View();
        }

        public ActionResult FiSemFeeInfoCheck(string id, string semester, string amount)
        {
            ViewBag.studentId = id;
            ViewBag.semester = semester;
            ViewBag.amount = amount;

            ViewBag.queryList = findAll();
            return View();
        }

        public IEnumerable<TempBankModel> findAll()
        {
            try
            {
                string baseUrl = "http://localhost:8024/api/";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(

                    new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("seminfo").Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<IEnumerable<TempBankModel>>().Result;

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
    }
}