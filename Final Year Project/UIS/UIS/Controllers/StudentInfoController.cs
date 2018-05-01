using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UIS.Models;

namespace UIS.Controllers
{
    public class StudentInfoController : Controller
    {
        private UisContext db = new UisContext();

        //
        // GET: /StudentInfo/
      
        public ActionResult Profile()
        {
            var userId = Session["userId"].ToString();
            var studentDetails = from s in db.Students select s;

            if (userId != null)
            {
                studentDetails = studentDetails.Where(c => c.studentId == userId);
            }
            
            return View(studentDetails);
        }


        public ActionResult chat()
        {
            string userId = Session["userId"].ToString();

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
            var userId = Session["userId"].ToString();
           // var userId = "1302004";
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
            ct.userName = Session["userNick"].ToString();
            ct.selectionCode = Session["userId"].ToString();
            ct.message = message;
            ct.timeDate = DateTime.Now.ToString();

            db.chats.Add(ct);
            db.SaveChanges();

            return View();
        }

     

	}
}