using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using UIS.Models;

namespace UIS.Controllers
{
    public class Admission_adminController : Controller
    {
        int ag_seat = 2;
        int fish_seat = 2;
        int nfs_seat = 2;
        int dm_seat = 2;
        int dvm_seat = 2;
        int ah_seat = 2;
        int lm_seat = 2;

        int sb1 = 3;
        int sb2 = 3;
        int kah = 2;
        int bbh = 2;
        int skh = 2;
        int fnh = 2;


        AdmissionContext adb = new AdmissionContext();

        //
        // GET: /Admission_admin/
        public ActionResult Notification()
        {
            return View();
        }
        public ActionResult eligible_list()
        {
            return View();
        }

        [HttpPost]
        public ActionResult eligible_list(HttpPostedFileBase excelfile)
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
                    List<Admission_eligibleList> listRolls = new List<Admission_eligibleList>();
                    for (int row = 1; row <= range.Rows.Count; row++)
                    {
                        Admission_eligibleList ae = new Admission_eligibleList();
                        ae.roll_no = ((Excel.Range)range.Cells[row, 1]).Text;
                        ae.unit = ((Excel.Range)range.Cells[row, 2]).Text;
                        listRolls.Add(ae);
                    }

                    foreach (var item in listRolls)
                    {

                        check_eligible ce = new check_eligible();
                        ce.roll_no = item.roll_no;
                        ce.unit = item.unit;

                        adb.check_eligible.Add(ce);
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
        ///---------------------------------------------------------------------------
        public ActionResult a_unit_result()
        {
            return View();
        }
        [HttpPost]
        public ActionResult a_unit_result(HttpPostedFileBase excelfile)
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

                        a_choice ac = new a_choice();
                        ac.userRoll = item.userRoll;
                        ac.merit_position = Int32.Parse(item.merit_position);
                        ac.status = item.status;

                        adb.a_choice.Add(ac);
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

            string allotedFaculty = null;
            string allotedHall = null;
            var subChose = from s in adb.a_choice orderby s.merit_position ascending select s;

            // Subject selection for students
            foreach (var item in subChose)
            {
                string[] subArray = new string[7];
                subArray[0] = item.first_choice;
                subArray[1] = item.second_choice;
                subArray[2] = item.third_choice;
                subArray[3] = item.forth_choice;
                subArray[4] = item.fifth_choice;
                subArray[5] = item.sixth_choice;
                subArray[6] = item.seventh_choice;

                string[] hallArray = new string[4];
                hallArray[0] = item.first_hall;
                hallArray[1] = item.second_hall;
                hallArray[2] = item.third_hall;
                hallArray[3] = item.forth_hall;


                if (subArray[0] != null || hallArray[0] != null)
                {
                    if (ag_seat == 0 && fish_seat == 0 && nfs_seat == 0 && dm_seat == 0 && dvm_seat == 0 && ah_seat == 0 && lm_seat == 0) //লজিকে সন্দেহ আছে
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

                        // for loop suru
                        for (int i = 0; i < 7; i++)
                        {
                            string faculty = subArray[i];
                            int seatAvailable = availableFunc(faculty);
                            if (seatAvailable > 0)
                            {
                                seatAllot(faculty);
                                allotedFaculty = subArray[i];
                                break;
                            }

                        }
                        //for loop ses


                        if (allotedFaculty == "dvm" || allotedFaculty == "ah")
                        {
                            allotedHall = externalCampusHall(item.userRoll); ;
                        }
                        else
                        {
                            //==== hall allotment process



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
                        }
                        //hall allocation process ses


                        allotedList af = new allotedList();
                        af.studentId = item.userRoll;
                        af.meritPostion = Convert.ToInt32(item.merit_position); ;
                        af.status = item.status;
                        af.allotedChoice = allotedFaculty;
                        af.allotedHall = allotedHall;
                        completeAllotedList.Add(af);


                    }
                }

            }


            List<allotedList> a_completeAllotedList = (List<allotedList>)Session["a_completeAllotedList"];
            Session["a_completeAllotedList"] = completeAllotedList;
            TempData["a_completeAllotedList"] = completeAllotedList;
            return RedirectToAction("uploadAllotedList");
        }

        public ActionResult uploadAllotedList()
        {
            ViewBag.completeAllotedList = TempData["a_completeAllotedList"];
            return View();
        }

        [HttpPost]
        public ActionResult uploadAllotedList(string demo)
        {

            var usr = adb.a_alloted.SingleOrDefault();

            if (usr == null)
            {
                a_alloted aAlloted = new a_alloted();
                foreach (var item in Session["a_completeAllotedList"] as IEnumerable<UIS.Models.allotedList>)
                {
                    aAlloted.studentId = item.studentId;
                    aAlloted.allotedFaculty = item.allotedChoice;
                    aAlloted.allotedHall = item.allotedHall;
                    aAlloted.status = null;

                    adb.a_alloted.Add(aAlloted);
                    adb.SaveChanges();

                }

                Session["a_completeAllotedList"] = null;
                ViewBag.noti = "SuccessFull";
                return View("Notification");
            }
            else
            {

            }

            return View();
        }


        // subject allotment code
        int availableFunc(string fac)
        {
            if (fac == "ag")
                return ag_seat;

            else if (fac == "fish")
                return fish_seat;

            else if (fac == "nfs")
                return nfs_seat;

            else if (fac == "dm")
                return dm_seat;

            else if (fac == "dvm")
                return dvm_seat;

            else if (fac == "ah")
                return ah_seat;

            else if (fac == "lm")
                return lm_seat;

            else
                return 0;


        }


        void seatAllot(string fac)
        {
            if (fac == "ag")
                ag_seat = ag_seat - 1;

            else if (fac == "fish")
                fish_seat = fish_seat - 1;

            else if (fac == "nfs")
                nfs_seat = nfs_seat - 1;

            else if (fac == "dm")
                dm_seat = dm_seat - 1;

            else if (fac == "dvm")
                dvm_seat = dvm_seat - 1;

            else if (fac == "ah")
                ah_seat = ah_seat - 1;

            else if (fac == "lm")
                lm_seat = lm_seat - 1;


        }

        // hall allotment code

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

        //dvm-ah hall allotment code
        public string externalCampusHall(string studentId)
        {
            string sexVariable = null;
            string ansvmHall = null;
            var query = from s in adb.studentDbs.Where(m => m.studentRoll == studentId) select s;
            var queryList = query.ToList();
            foreach (var element in queryList)
            {
                sexVariable = element.sex;
            }

            if (sexVariable == "m")
            {
                ansvmHall = "MJH-EC";
            }
            else
            {
                ansvmHall = "FNH-EC";
            }

            return ansvmHall;
        }



    }

}
