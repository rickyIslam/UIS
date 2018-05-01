using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UIS.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace UIS.Controllers
{
    public class c_unitController : Controller
    {
        int sb1 = 3;
        int sb2 = 3;
        int kah = 3;
        int bbh = 3;
        int skh = 3;
        int fnh = 3;

        int cse_seat = 6;

        AdmissionContext adb = new AdmissionContext();

        public ActionResult Notification()
        {
            return View();
        }

        public ActionResult result_table_check()
        {


            return View();

        }
        public ActionResult c_unit_result()
        {
            return View();
        }
        [HttpPost]
        public ActionResult c_unit_result(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.noti = "Something Went Wrong......";
                return View("Notification");
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/Content/Excel/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelfile.SaveAs(path);

                    //Read Data from excel File
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    List<result_upload_prop> listRolls = new List<result_upload_prop>();
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        result_upload_prop rup = new result_upload_prop();
                        rup.userRoll = ((Excel.Range)range.Cells[row, 1]).Text;
                        rup.merit_position = ((Excel.Range)range.Cells[row, 2]).Text;
                        rup.status = ((Excel.Range)range.Cells[row, 3]).Text;
                        listRolls.Add(rup);
                    }

                    foreach (var item in listRolls)
                    {

                        c_choice ac = new c_choice();
                        ac.userRoll = item.userRoll;
                        ac.merit_position = Int32.Parse(item.merit_position);
                        ac.status = item.status;

                        adb.c_choice.Add(ac);
                        adb.SaveChanges();
                    }

                    ViewBag.noti = "Uploaded Successfully......";
                    return View("Notification");

                }
                else
                {
                    ViewBag.noti = "Something Went Wrong......";
                    return View("Notification");
                }
            }

            return View();
        }


        public ActionResult allotList()
        {

            List<allotedList> completeAllotedList = new List<allotedList>();

            string allotedFaculty = "CSE";
            string allotedHall = null;
            var subChose = from s in adb.c_choice orderby s.merit_position ascending select s;

            // Subject selection for students
            foreach (var item in subChose)
            {


                string[] hallArray = new string[4];
                hallArray[0] = item.first_hall;
                hallArray[1] = item.second_hall;
                hallArray[2] = item.third_hall;
                hallArray[3] = item.forth_hall;


                if (hallArray[0] != null)
                {
                    if (cse_seat == 0) //লজিকে সন্দেহ আছে
                    {


                        allotedList af = new allotedList();
                        af.studentId = item.userRoll;
                        af.meritPostion = Convert.ToInt32(item.merit_position); ;
                        af.status = item.status;
                        af.allotedChoice = "Waiting";
                        af.allotedHall = "Waiting";
                        completeAllotedList.Add(af);
                    }

                    else
                    {
                        cse_seat--;

                        for (int i = 0; i < 4; i++)
                        {
                            string hall = hallArray[i];
                            int hallSeat = hallAvailableFunc(hall);
                            if (hallSeat > 0)
                            {
                                hallSeatAllot(hall);
                                allotedHall = hallArray[i];
                                break;
                            }

                        }

                        allotedList af = new allotedList();
                        af.studentId = item.userRoll;
                        af.meritPostion = Convert.ToInt32(item.merit_position); ;
                        af.status = item.status;
                        af.allotedChoice = allotedFaculty;
                        if (allotedHall == null)
                        {
                            af.allotedHall = "------";
                        }
                        else
                        {
                            af.allotedHall = allotedHall;
                        }

                        completeAllotedList.Add(af);

                        allotedHall = null;


                    }
                }



            }


            List<allotedList> a_completeAllotedList = (List<allotedList>)Session["a_completeAllotedList"];
            Session["c_completeAllotedList"] = completeAllotedList;
            TempData["c_completeAllotedList"] = completeAllotedList;
            return RedirectToAction("uploadAllotedList");
        }

        public ActionResult uploadAllotedList()
        {
            ViewBag.completeAllotedList = TempData["c_completeAllotedList"];
            return View();
        }

        [HttpPost]
        public ActionResult uploadAllotedList(string demo)
        {

            var usr = adb.c_alloted.SingleOrDefault();

            if (usr == null)
            {

                foreach (var item in Session["c_completeAllotedList"] as IEnumerable<UIS.Models.allotedList>)
                {
                    c_alloted cAlloted = new c_alloted();
                    cAlloted.studentId = item.studentId;
                    cAlloted.allotedFaculty = item.allotedChoice;
                    cAlloted.allotedHall = item.allotedHall;
                    cAlloted.status = null;

                    adb.c_alloted.Add(cAlloted);
                    adb.SaveChanges();
                    cAlloted = null;

                }

                Session["b_completeAllotedList"] = null;
                ViewBag.noti = "SuccessFull";
                return View("Notification");
            }
            else
            {

            }

            return View();
        }


        // Admission From MErit List --------------------------------------------------------------------------------
        public ActionResult c_unit()
        {
            var cse_remain = TotalSeat.cseSeat - (from s in adb.allotCounts where s.allotedFaculty == "CSE" select s).Count();
            ViewBag.cse_seat = TotalSeat.cseSeat + "/" + cse_remain;


            var items = (from s in adb.c_alloted orderby s.c_choice.merit_position ascending select s).ToList();

            if (items != null)
            {
                items = items.Where(c => c.allotedFaculty != "Waiting" && c.status != "done").ToList();
            }

            return View(items);
        }

        // Admission From Waiting ..=------------------------------------------------------------------------------
        public ActionResult c_unit_waiting()
        {
            var cse_remain = TotalSeat.cseSeat - (from s in adb.allotCounts where s.allotedFaculty == "CSE" select s).Count();
            ViewBag.cse_seat = TotalSeat.cseSeat + "/" + cse_remain;

            // retrive data from allotd Table 
            var items = (from s in adb.c_alloted orderby s.c_choice.merit_position ascending select s).ToList();

            if (items != null)
            {
                items = items.Where(c => c.allotedFaculty == "Waiting" && c.status != "done").ToList();
            }

            return View(items);
        }

        // Allocation --------------------------------------

        public ActionResult c_check_post(string Id, string allotedFaculty, string allotedHall)
        {

            string[] hallArray = new string[4];
            string waiting_alloted_faculty = "CSE";
            string waiting_alloted_hall = null;


            if (allotedFaculty == "Waiting" && allotedHall == "Waiting")
            {

                var choices = from s in adb.c_choice where s.userRoll == Id select s;

                // ------------------------ Subject Iterating ---------------------
                foreach (var item in choices)
                {
                    //hallssssssss
                    hallArray[0] = item.first_hall;
                    hallArray[1] = item.second_hall;
                    hallArray[2] = item.third_hall;
                    hallArray[3] = item.forth_hall;
                    // }



                    // ------------------------ Hall Iterating ---------------------

                    for (int i = 0; i < 4; i++)
                    {
                        string hall = hallArray[i];
                        int hallCounter = hallRemain(hall);
                        if (hallCounter > 0)
                        {
                            waiting_alloted_hall = hallArray[i];
                            break;
                        }

                    }


                }


                var usr = adb.c_alloted.SingleOrDefault(u => u.studentId == Id);
                if (usr != null)
                {
                    TempData["admissionRoll"] = usr.studentDb.studentRoll;
                    TempData["name"] = usr.studentDb.name;
                    TempData["sex"] = usr.studentDb.sex;
                    TempData["father"] = usr.studentDb.father;
                    TempData["sscRoll"] = usr.studentDb.ssc_roll;
                    TempData["sscReg"] = usr.studentDb.ssc_reg;
                    TempData["hscRoll"] = usr.studentDb.hsc_roll;
                    TempData["hscReg"] = usr.studentDb.hsc_reg;
                    TempData["photo"] = usr.studentDb.photoLink;
                    TempData["faculty"] = waiting_alloted_faculty;
                    TempData["hall"] = waiting_alloted_hall;

                }

                // ---------------- alloteCount Insertion -------------------

                if (ModelState.IsValid)
                {

                    var selectedStudent = (from p in adb.c_alloted where (p.studentId == Id) select p).FirstOrDefault();
                    selectedStudent.status = "done";
                    adb.Entry(selectedStudent).State = EntityState.Modified;
                    adb.SaveChanges();
                    // return RedirectToAction("HallInfoList");
                }

                allotCount ac = new allotCount();
                ac.studentId = usr.studentDb.studentRoll;
                ac.allotedFaculty = waiting_alloted_faculty;
                ac.allotedHall = waiting_alloted_hall;

                adb.allotCounts.Add(ac);
                adb.SaveChanges();



                return RedirectToAction("RegisterStudent", "AdminPanel");


            } //--------------End-------------------------

//----------------------------------------------Merit Students _______________________________________________
            else
            {

                var usr = adb.c_alloted.SingleOrDefault(u => u.studentId == Id);
                if (usr != null)
                {
                    TempData["admissionRoll"] = usr.studentDb.studentRoll;
                    TempData["name"] = usr.studentDb.name;
                    TempData["sex"] = usr.studentDb.sex;
                    TempData["father"] = usr.studentDb.father;
                    TempData["sscRoll"] = usr.studentDb.ssc_roll;
                    TempData["sscReg"] = usr.studentDb.ssc_reg;
                    TempData["hscRoll"] = usr.studentDb.hsc_roll;
                    TempData["hscReg"] = usr.studentDb.hsc_reg;
                    TempData["photo"] = usr.studentDb.photoLink;
                    TempData["faculty"] = allotedFaculty;
                    TempData["hall"] = allotedHall;

                }

                if (ModelState.IsValid)
                {

                    var selectedStudent = (from p in adb.c_alloted where (p.studentId == Id) select p).FirstOrDefault();
                    selectedStudent.status = "done";
                    adb.Entry(selectedStudent).State = EntityState.Modified;
                    adb.SaveChanges();
                    // return RedirectToAction("HallInfoList");
                }

                allotCount ac = new allotCount();
                ac.studentId = usr.studentDb.studentRoll;
                ac.allotedFaculty = allotedFaculty;
                ac.allotedHall = allotedHall;

                adb.allotCounts.Add(ac);
                adb.SaveChanges();

                return RedirectToAction("RegisterStudent", "AdminPanel");
            }

        }




        // allotment Functionsssssssssss---------------------------------------------------------------------------------


        int hallAvailableFunc(string hall)
        {
            if (hall == "SB1")
                return sb1;

            else if (hall == "SB2")
                return sb2;

            else if (hall == "KAH")
                return kah;

            else if (hall == "BBH")
                return bbh;

            else if (hall == "SKH")
                return skh;

            else if (hall == "FNH")
                return fnh;

            else
                return 0;

        }

        void hallSeatAllot(string hall)
        {


            if (hall == "SB1")
                sb1 = sb1 - 1;

            else if (hall == "SB2")
                sb2 = sb2 - 1;

            else if (hall == "KAH")
                kah = kah - 1;

            else if (hall == "BBH")
                bbh = bbh - 1;

            else if (hall == "SKH")
                skh = skh - 1;

            else if (hall == "FNH")
                fnh = fnh - 1;

        }

        //---------------- FOr final admisson Process ----------------------------------------------------------- 

        int hallRemain(string hall)
        {
            int sb1_remain;
            int sb2_remain;
            int kah_remain;
            int bbh_remain;
            int skh_remain;
            int fnh_remain;

            var details = from s in adb.allotCounts where (s.allotedFaculty == "BBA") select s;


            if (hall == "SB1")
                return sb1_remain = TotalSeat.cse_sb1 - (details = details.Where(c => c.allotedHall == "SB1")).Count();

            else if (hall == "SB2")
                return sb2_remain = TotalSeat.cse_sb2 - (details = details.Where(c => c.allotedHall == "SB2")).Count();

            else if (hall == "KAH")
                return kah_remain = TotalSeat.cse_kah - (details = details.Where(c => c.allotedHall == "KAH")).Count();

            else if (hall == "BBH")
                return bbh_remain = TotalSeat.cse_bbh - (details = details.Where(c => c.allotedHall == "BBH")).Count();

            else if (hall == "SKH")
                return skh_remain = TotalSeat.cse_skh - (details = details.Where(c => c.allotedHall == "SKH")).Count();

            else if (hall == "FNH")
                return fnh_remain = TotalSeat.cse_fnh - (details = details.Where(c => c.allotedHall == "FNH")).Count();

            else
                return 0;


        }
    }

}