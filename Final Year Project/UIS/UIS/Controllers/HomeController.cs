using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UIS.Models;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.IO;

using System.Net.Mail;
using PagedList;
using System.Dynamic;

namespace UIS.Controllers
{
    
    
    public class HomeController : Controller
    {
        private UisContext db = new UisContext();
        //
        // GET: /Home/
        public ActionResult Home()
        {
            return View(db.Posts.ToList());
        }
	}
}