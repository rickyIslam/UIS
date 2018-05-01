using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UIS.Models;

namespace UIS.Controllers
{
    public class AdminPanelController : Controller
    {
        //
        // GET: /AdminPanel/
        UisContext db = new UisContext();

        public ActionResult RegisterStudent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterStudent(Student std)
        {
            return View();
        }

        public ActionResult studentList()
        {
            return View(db.Students.ToList());
        }

        public ActionResult redirection(string Id)
        {
            Session["admin_selection_code"] = Id;
            return RedirectToAction("chat","AdminPanel");
        }

        public ActionResult chat()
        {
            
            string userId = Session["admin_selection_code"].ToString();

            var usr = db.Students.SingleOrDefault(u => u.studentId == userId);

            ViewBag.photo = usr.Photo;
            ViewBag.fullName = usr.fullName;
            ViewBag.nickName = usr.nickName;
            ViewBag.studentId = usr.studentId;
            ViewBag.reg = usr.Registration;
            ViewBag.faculty = usr.Faculty;
            ViewBag.session = usr.Session;
            ViewBag.phone = usr.Phone;

            return View();
        }

        public JsonResult getMessage()
        {
            var userId = Session["admin_selection_code"].ToString();
            var message = (from s in db.chats select s).AsEnumerable().Reverse();

            if (userId != null)
            {
                message = message.Where(c => c.selectionCode == userId);
            }

            var jsonData = Json(message, JsonRequestBehavior.AllowGet);
            return jsonData;

        }

        [HttpPost]
        public ActionResult chat(string userName, string message)
        {
            chat ct = new chat();
            ct.userName = "admin";
            ct.selectionCode = Session["admin_selection_code"].ToString();
            ct.message = message;
            ct.timeDate = DateTime.Now.ToString();

            db.chats.Add(ct);
            db.SaveChanges();

            return View();
        }
    }
}