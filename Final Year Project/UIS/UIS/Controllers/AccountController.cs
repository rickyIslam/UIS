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

namespace UIS.Controllers
{
    public class AccountController : Controller
    {
        private UisContext db = new UisContext();
        
        //স্টুডেন্ট রেজিষ্টার করার জন্য

       
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "studentId,fullName,nickName,Dob,Faculty,Registration,Session,Hall,Father,Mother,Phone,Email,Password,parmanentAddress,Photo")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index","Blog");
            }

            return View(student);
        }


        //ষ্টুডেন্ট লগিন ... গেট
        public ActionResult LoginStudent()
        {
            return View();
        }

        //ষ্টুডেন্ট লগিন ... পোষ্ট
        [HttpPost]
        public ActionResult LoginStudent(string userName, string password, string selector)
        {
            string level = null;
            string semester = null;
           // string strUserName = Convert.ToString(userName);
            string enrollStatus = null;


            if (selector=="student")
            {
             
                var usr = db.Students.SingleOrDefault(u => u.studentId == userName && u.Password == password);


                if (usr != null)
                {
                    Session["userId"] = usr.studentId.ToString();
                    Session["userNick"] = usr.nickName.ToString();
                    Session["userName"] = usr.fullName.ToString();
                    Session["photo"] = usr.Photo.ToString();
                    Session["userFaculty"] = usr.Faculty.ToString();
                    Session["userEmail"] = usr.Email.ToString();

                    //Level Of Student
                    string session = usr.Session.ToString();
                    string startingYear = session.Substring(0, 4);
                    int intStartingYear = Convert.ToInt32(startingYear);
                    int admissionYear = intStartingYear + 1;

                    int currentYear = DateTime.Now.Year;

                    int diff = currentYear - admissionYear;

                    if (diff == 0)
                    {
                        level = "1";
                    }
                    else if (diff == 1)
                    {
                        level = "2";
                    }
                    else if (diff == 2)
                    {
                        level = "3";
                    }
                    else if (diff == 3)
                    {
                        level = "4";
                    }
                    else
                    {
                        level = "Session Completed";
                    }




                    //student semester
                    int sem = DateTime.Now.Month;

                    if (sem >= 1 && sem <= 6)
                    {
                        semester = "1";
                    }
                    else
                    {
                        semester = "2";
                    }

                    string faculty = usr.Faculty.ToString();
                    string CurrentState = faculty + level + semester;

                    Session["level"] = level;
                    Session["semester"] = semester;

                    if (level == "Session Completed")
                    {
                        Session["currentState"] = CurrentState;
                    }
                    else
                    {
                        Session["currentState"] = "completed";
                    }

                   

                    // Enrollment Status Pawar jonno
                    string varFaculty = Session["userFaculty"].ToString();
                    var status = db.enrollmentStatus.SingleOrDefault(a => a.faculty == varFaculty);
                    if (status != null)
                    {
                        enrollStatus = status.status;
                    }

                    Session["enrollmentStatus"] = enrollStatus;


                    return RedirectToAction("Index", "Blog");
                }

                else
                {

                    return RedirectToAction("LoginTemp");


                }

            }

            if(selector=="account")
            {
                var usr = db.AccountOfficeAuths.SingleOrDefault(u => u.Employee_Id == userName && u.Account_Password == password);
                if(usr != null)
                {
                    return RedirectToAction("SemInfoList", "AccountOfficer");
                }

                else
                {

                    return RedirectToAction("LoginTemp");


                }
            }

            if (selector == "dean")
            {
                var usr = db.DeanOfficeAuths.SingleOrDefault(u => u.Employee_Id == userName && u.Account_Password == password);
                if (usr != null)
                {
                    return RedirectToAction("enrollReq", "DeanOffice");  
                }

                else
                {

                    return RedirectToAction("LoginTemp");


                }
            }


            if (selector == "hall")
            {
                var usr = db.HallProvostAuths.SingleOrDefault(u => u.Employee_Id == userName && u.Account_Password == password);
                if (usr != null)
                {
                    return RedirectToAction("HallFeeInfoList", "HallOfficer"); 
                }

                else
                {

                    return RedirectToAction("LoginTemp");


                }
            }





            return View();
        }


        public ActionResult logOut()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("LoginStudent", "Account");

        }

        //লগিন সংক্রান্ত তথ্য ভুল হলে
        public ActionResult LoginTemp()
        {
            return View();
        }

        //মেইল পাঠানোর ফাংশান

        public ActionResult sentMail()
        {
            Random random = new Random();
            int password = random.Next(1000, 100000000);
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("mdrickyislam@gmail.com");
            msg.To.Add("rickygrason@gmail.com");
            msg.Subject = "PSTU Information System- Change Your Password !!!";
            msg.Body = "Dear User !!! Your Security Code is " + password + ". Please Enter This Code For Continue The Next Process.";
            SmtpClient sc = new SmtpClient("smtp.gmail.com");
            sc.Port = 25;
            sc.Credentials = new NetworkCredential("mdrickyislam@gmail.com", "*******");
            sc.EnableSsl = true;
            sc.Send(msg);
            Response.Write("Mail send");

            return View();
        }

        //মেইল অথন্টিকেশন গেট
        public ActionResult MailAuth()
        {

            return View();

        }

        [HttpPost]
        public ActionResult MailAuth(string dummy)
        {
            string myMail = Session["userEmail"].ToString();
            //Session["authMail"] = email;
            MailAuth ma = new MailAuth();

            try
            {
                Random random = new Random();
                int password = random.Next(1000, 100000000);
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("mdrickyislam@gmail.com");
                msg.To.Add(myMail);
                msg.Subject = "PSTU Information System- Update Your Password";
                msg.Body = "Dear User !!! Thanks For Your Interest. Your Security Code is " + password + ". Please Enter This Code For Processing Further Steps.";
                SmtpClient sc = new SmtpClient("smtp.gmail.com");
                sc.Port = 25;
                sc.Credentials = new NetworkCredential("mdrickyislam@gmail.com", "12357890");
                sc.EnableSsl = true;
                sc.Send(msg);

                // TO strore in Database 

                string confirmationCode = Convert.ToString(password);
                ma.mail = myMail;
                ma.code = confirmationCode;

                db.MailAuths.Add(ma);
                db.SaveChanges();


            }
            catch (Exception ex)
            {
                return View("Error: " + ex);
            }

            return RedirectToAction("CodeAuth", "Account");

        }

        //কোড অথন্টিকেশানের জন্য 
        public ActionResult CodeAuth()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CodeAuth(string code)
        {
            string email = Session["userEmail"].ToString();
            var auth = db.MailAuths.SingleOrDefault(u => u.code == code && u.mail == email);

            if (auth != null)
            {
                return RedirectToAction("StudentChangePassword","Account");
            }

            else
            {
                return View();
            }

        }

        public ActionResult StudentChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StudentChangePassword(string newPass)
        {
            string userId = Session["userId"].ToString();
            if (ModelState.IsValid)
            {

                var model = (from p in db.Students where (p.studentId == userId) select p).FirstOrDefault();


                model.Password = newPass;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profile", "StudentInfo");
            }


            return View();
        }


        //একাউন্ট অফিসার লগিন 
        public ActionResult AccountOfficerLogin ()
        {
          
            return View();
        }

        


    }
}
