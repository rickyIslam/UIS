using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UIS.Models;

namespace UIS.Controllers
{
    public class NoticeBoardController : Controller
    {
        AdmissionContext adb = new AdmissionContext();
        //
        // GET: /NoticeBoard/
        public ActionResult CreateNotice()
        {
            return View();
        }

        public ActionResult NoticeHome()
        {
            return View();
        }

     
	}
}