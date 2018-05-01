using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UIS.Models;

namespace UIS.Controllers
{
    public class CourseRoutineController : Controller
    {
        //
        // GET: /ExamRoutine/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult allCourses()
        {
            string faculty = Session["userFaculty"].ToString();
            UisContext db = new UisContext();
            if(faculty == "CSE")
            {
                return View(db.cse_courseLayout.ToList());
            }




            return View();
        }

      
	}
}