using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UIS.Models;
using System.IO;
using Rotativa;
using System.Data.Entity;

namespace UIS.Controllers
{
    public class Admission_studentController : Controller
    {
        AdmissionContext adb = new AdmissionContext();

        //
        // GET: /Admission_student/
        public ActionResult Notification()
        {
            return View();
        }
        public ActionResult check()
        {
            return View();
        }

        [HttpPost]
        public ActionResult check(string unit, string userRoll)
        {
            if (unit=="A")
            {
                var usr_check = adb.check_eligible.SingleOrDefault(u => u.roll_no == userRoll && u.unit == unit);
                var choice_check = adb.a_choice.SingleOrDefault(u => u.userRoll == userRoll);
                var studentDb_check = adb.studentDbs.SingleOrDefault(m => m.studentRoll == userRoll);

                if (usr_check != null)
                {
                    if (studentDb_check != null)
                    {
                        Session["admission_roll"] = userRoll;
                        Session["admission_unit"] = unit;
                        return RedirectToAction("admit_stopage", "Admission_student");
                    }
                    else
                    {
                        Session["admission_roll"] = userRoll;
                        Session["admission_unit"] = unit;
                        return RedirectToAction("admitCart", "Admission_student"); // a_choice
                    }
                }
                else
                {
                    ViewBag.noti = "No detail found... ";
                    return View("Notification");
                }
            }

            if (unit == "B")
            {
                var usr_check = adb.check_eligible.SingleOrDefault(u => u.roll_no == userRoll && u.unit == unit);
                var choice_check = adb.b_choice.SingleOrDefault(u => u.userRoll == userRoll);
                var studentDb_check = adb.studentDbs.SingleOrDefault(m => m.studentRoll == userRoll);

                if (usr_check != null)
                {
                    if (studentDb_check !=null)
                    {
                        Session["admission_roll"] = userRoll;
                        Session["admission_unit"] = unit;
                        return RedirectToAction("admit_stopage", "Admission_student");
                    }
                    else
                    {
                        Session["admission_roll"] = userRoll;
                        Session["admission_unit"] = unit;
                        return RedirectToAction("admitCart", "Admission_student"); // a_choice
                    }
                }
                else
                {
                    ViewBag.noti = "No detail found... ";
                    return View("Notification");
                }
            }

            if (unit == "C")
            {
                var usr_check = adb.check_eligible.SingleOrDefault(u => u.roll_no == userRoll && u.unit == unit);
               // var choice_check = adb.b_choice.SingleOrDefault(u => u.userRoll == userRoll);
                var studentDb_check = adb.studentDbs.SingleOrDefault(m => m.studentRoll == userRoll);

                if (usr_check != null)
                {
                    if (studentDb_check != null)
                    {
                        Session["admission_roll"] = userRoll;
                        Session["admission_unit"] = unit;
                        return RedirectToAction("admit_stopage", "Admission_student");
                    }
                    else
                    {
                        Session["admission_roll"] = userRoll;
                        Session["admission_unit"] = unit;
                        return RedirectToAction("admitCart", "Admission_student"); // a_choice
                    }
                }
                else
                {
                    ViewBag.noti = "No detail found... ";
                    return View("Notification");
                }
            }

            return View();
     
        }

        public ActionResult admitCart()
        {
            return View();

        }
        [HttpPost]
        public ActionResult admitCart(string studentName, string fatherName, string hscRoll, string sscRoll, string hscReg, string sscReg, HttpPostedFileBase file, string sex)
        {

            var path = "";
            var photoPath = "";
            var aa = Path.GetExtension(file.FileName).ToLower();

            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    if (Path.GetExtension(file.FileName).ToLower() == ".jpg"
                        || Path.GetExtension(file.FileName).ToLower() == ".png"
                              || Path.GetExtension(file.FileName).ToLower() == ".gif"
                                    || Path.GetExtension(file.FileName).ToLower() == ".jpeg")
                    {

                        System.Drawing.Image myImage = System.Drawing.Image.FromStream(file.InputStream);
                        if (myImage.Width == 300 && myImage.Height == 300)
                        {
                            path = Path.Combine(Server.MapPath("~/AdmissionPhoto"), file.FileName);
                            file.SaveAs(path);
                            photoPath = file.FileName;

                        }
                        else
                        {
                            ViewBag.noti = "Picture dimention Not matched !!!";
                            return View("");
                        }

                    }
                    else
                    {
                        ViewBag.noti = "Upload valid Image File";
                        return View("Notification");
                    }
                }

            }

            studentDb sdb = new studentDb();
            sdb.name = studentName;
            sdb.father = fatherName;
            sdb.hsc_roll = hscRoll;
            sdb.hsc_reg = hscReg;
            sdb.ssc_reg = sscReg;
            sdb.ssc_roll = sscRoll;
            sdb.photoLink = photoPath;
            sdb.sex = sex;
            sdb.studentRoll = Session["admission_roll"].ToString();
            sdb.unit = Session["admission_unit"].ToString();
            Session["sex"] = sex;

            adb.studentDbs.Add(sdb);
            adb.SaveChanges();

            return RedirectToAction("admit_stopage", "Admission_student");

          return View();

        }


        public ActionResult admit_stopage()
        {
            return View();
        }

        public ActionResult admitCard(string userRoll)
        {
            var studentDetails = from s in adb.studentDbs select s;

            if (userRoll != null)
            {
                studentDetails = studentDetails.Where(c => c.studentRoll == userRoll);
            }

            return View(studentDetails);
        }


        public ActionResult pdf_admit()
        {
            string userRoll = Session["admission_roll"].ToString();
            var report = new ActionAsPdf("admitCard", new { userRoll = userRoll });
            return report;
        }

        // ============================================================

        public ActionResult give_sub_choice ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult give_sub_choice(string unit, string userRoll)
        {
            string sexVar = null;

            var usr = adb.studentDbs.SingleOrDefault(u => u.studentRoll == userRoll && u.unit==unit);

            if (usr == null)
            {
                ViewBag.noti = "That Roll Is Not Listed....";
                return View("notification");
            }
            else
            {
                sexVar = usr.sex.ToString();

                if (sexVar == "m" || sexVar == "f")
                {
                    Session["sub_unit"] = unit;
                    Session["sub_roll"] = userRoll;
                    Session["sex"] = sexVar;

                    if (unit == "A")
                    {
                        return RedirectToAction("a_choice", "Admission_student");
                    }
                    else if (unit == "B")
                    {
                        return RedirectToAction("b_choice", "Admission_student");
                    }
                    else if (unit == "C")
                    {
                        return RedirectToAction("c_choice", "Admission_student");
                    }

                  

                }
                else
                {
                    ViewBag.noti = "That Roll Is Not Listed....";
                    return View("notification");
                    // return Content(sexVar);
                }
            }
          

            
            
            return View();
        }





        public ActionResult result_check()
        {
            string unit = TempData["unit"].ToString();
            string studentRoll = TempData["studentRoll"].ToString();

            string sexVariable = null;
            if (unit == "A")
            {
                var a_choice = adb.a_choice.SingleOrDefault(m => m.userRoll == studentRoll);
                if (a_choice != null)
                {
                    Session["admission_result_roll"] = studentRoll;
                    var query = from s in adb.studentDbs.Where(m => m.studentRoll == studentRoll) select s;
                    var queryList = query.ToList();
                    foreach (var element in queryList)
                    {
                        sexVariable = element.sex;
                    }
                    ViewBag.sex = sexVariable;
                    return View("a_choice");
                }
                else
                {
                    ViewBag.noti = "Sorry ! Your roll is not listed !";
                    return View("Notification");
                }
            }

            return View();
        }


        [HttpPost]
        public ActionResult result_check(string userRoll, string unit)
        {
            string sexVariable = null;
            if (unit == "A")
            {
                var a_choice = adb.a_choice.SingleOrDefault(m => m.userRoll == userRoll);
                if (a_choice != null)
                {
                    Session["admission_result_roll"] = userRoll;
                    var query = from s in adb.studentDbs.Where(m => m.studentRoll == userRoll) select s;
                    var queryList = query.ToList();
                    foreach (var element in queryList)
                    {
                        sexVariable = element.sex;
                    }
                    ViewBag.sex = sexVariable;
                    return View("a_choice");
                }
                else
                {
                    ViewBag.noti = "Sorry ! Your roll is not listed !";
                    return View("Notification");
                }
            }

            return View();
        }
        public ActionResult a_choice()
        {
            return View();
        }
        [HttpPost]
        public ActionResult a_choice(string first_choice, string second_choice, string third_choice, string forth_choice, string fifth_choice, string sixth_choice, string seventh_choice,string first_hall, string second_hall, string third_hall, string forth_hall)
        {
            if (ModelState.IsValid)
            {
                string userRoll = Session["admission_roll"].ToString();
                var model = (from p in adb.a_choice where (p.userRoll == userRoll) select p).FirstOrDefault();


                model.first_choice = first_choice;
                model.second_choice = second_choice;
                model.third_choice = third_choice;
                model.forth_choice = forth_choice;
                model.fifth_choice = fifth_choice;
                model.sixth_choice = sixth_choice;
                model.seventh_choice = seventh_choice;

                model.first_hall = first_hall;
                model.second_hall = second_hall;
                model.third_hall = third_hall;
                model.forth_hall = forth_hall;

                adb.Entry(model).State = EntityState.Modified;
                adb.SaveChanges();
                return RedirectToAction("admit_stopage", "Admission_student");
            }
            return View();

        }

        // B Unit Starts hereeeeee -------------------------------------------------------------------------------------------------------------------

        public ActionResult b_choice()
        {
            return View();
        }

        [HttpPost]
        public ActionResult b_choice(string first_hall, string second_hall, string third_hall, string forth_hall)
        {
            if (ModelState.IsValid)
            {
                string userRoll = Session["sub_roll"].ToString();
                var model = (from p in adb.b_choice where (p.userRoll == userRoll) select p).FirstOrDefault();


                model.first_hall = first_hall;
                model.second_hall = second_hall;
                model.third_hall = third_hall;
                model.forth_hall = forth_hall;

                adb.Entry(model).State = EntityState.Modified;
                adb.SaveChanges();
                return RedirectToAction("logOut", "Admission_student");
            }
            return View();

        }

        public ActionResult c_choice()
        {
            return View();
        }

        [HttpPost]
        public ActionResult c_choice(string first_hall, string second_hall, string third_hall, string forth_hall)
        {
            if (ModelState.IsValid)
            {
                string userRoll = Session["sub_roll"].ToString();
                var model = (from p in adb.c_choice where (p.userRoll == userRoll) select p).FirstOrDefault();


                model.first_hall = first_hall;
                model.second_hall = second_hall;
                model.third_hall = third_hall;
                model.forth_hall = forth_hall;

                adb.Entry(model).State = EntityState.Modified;
                adb.SaveChanges();
                return RedirectToAction("logOut", "Admission_student");
            }
            return View();

        }

        public ActionResult logOut()
        {
          
            
            return View();
        }

        [HttpPost]
        public ActionResult logOut(string dummy)
        {
            Session["admission_unit"] = null;
            Session["admission_roll"] = null;
            Session["sub_unit"] = null;
            Session["sub_roll"] = null;

           return RedirectToAction("give_sub_choice", "Admission_student");
        }

    }
}