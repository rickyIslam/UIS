using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UIS.Models;
using System.Data.Entity;

namespace UIS.Controllers
{
    public class Admission_finalController : Controller
    {
        AdmissionContext adb = new AdmissionContext();
        //
        // GET: /Admission_final/
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult a_unit()
        {
            var ag_remain = TotalSeat.ag_seat - (from s in adb.allotCounts where s.allotedFaculty == "ag" select s).Count();
            ViewBag.ag_seat = TotalSeat.ag_seat + "/" + ag_remain;

            var fish_remain = TotalSeat.fish_seat - (from s in adb.allotCounts where s.allotedFaculty == "fish" select s).Count();
            ViewBag.fish_seat = TotalSeat.fish_seat + "/" + fish_remain;

            var nfs_remain = TotalSeat.nfs_seat - (from s in adb.allotCounts where s.allotedFaculty == "nfs" select s).Count();
            ViewBag.nfs_seat = TotalSeat.nfs_seat + "/" + nfs_remain;

            var dm_remain = TotalSeat.dm_seat - (from s in adb.allotCounts where s.allotedFaculty == "dm" select s).Count();
            ViewBag.dm_seat = TotalSeat.dm_seat + "/" + dm_remain;

            var dvm_remain = TotalSeat.dvm_seat - (from s in adb.allotCounts where s.allotedFaculty == "dvm" select s).Count();
            ViewBag.dvm_seat = TotalSeat.dvm_seat + "/" + dvm_remain;

            var ah_remain = TotalSeat.ah_seat - (from s in adb.allotCounts where s.allotedFaculty == "ah" select s).Count();
            ViewBag.ah_seat = TotalSeat.ah_seat + "/" + ah_remain;

            var lm_remain = TotalSeat.lm_seat - (from s in adb.allotCounts where s.allotedFaculty == "lm" select s).Count();
            ViewBag.lm_seat = TotalSeat.lm_seat + "/" + lm_remain;


            //var items = (from s in adb.b_alloted orderby s.b_choice.merit_position ascending select s).ToList();

            //if (items != null)
            //{
            //    items = items.Where(c => c.allotedFaculty != "Waiting" && c.status != "done").ToList();
            //}

            //return View(items);

            var items = from s in adb.a_alloted select s;

            if (items != null)
            {
                items = items.Where(c => c.allotedFaculty != "Waiting" && c.status != "done");
            }

            return View(items);
        }

        public ActionResult a_unit_waiting()
        {

            var ag_remain = TotalSeat.ag_seat - (from s in adb.allotCounts where s.allotedFaculty == "ag" select s).Count();
            ViewBag.ag_seat = TotalSeat.ag_seat + "/" + ag_remain;

            var fish_remain = TotalSeat.fish_seat - (from s in adb.allotCounts where s.allotedFaculty == "fish" select s).Count();
            ViewBag.fish_seat = TotalSeat.fish_seat + "/" + fish_remain;

            var nfs_remain = TotalSeat.nfs_seat - (from s in adb.allotCounts where s.allotedFaculty == "nfs" select s).Count();
            ViewBag.nfs_seat = TotalSeat.nfs_seat + "/" + nfs_remain;

            var dm_remain = TotalSeat.dm_seat - (from s in adb.allotCounts where s.allotedFaculty == "dm" select s).Count();
            ViewBag.dm_seat = TotalSeat.dm_seat + "/" + dm_remain;

            var dvm_remain = TotalSeat.dvm_seat - (from s in adb.allotCounts where s.allotedFaculty == "dvm" select s).Count();
            ViewBag.dvm_seat = TotalSeat.dvm_seat + "/" + dvm_remain;

            var ah_remain = TotalSeat.ah_seat - (from s in adb.allotCounts where s.allotedFaculty == "ah" select s).Count();
            ViewBag.ah_seat = TotalSeat.ah_seat + "/" + ah_remain;

            var lm_remain = TotalSeat.lm_seat - (from s in adb.allotCounts where s.allotedFaculty == "lm" select s).Count();
            ViewBag.lm_seat = TotalSeat.lm_seat + "/" + lm_remain;

            // retrive data from allotd Table 
            //var items = from s in adb.a_alloted select s;

            //if (items != null)
            //{
            //    items = items.Where(c => c.allotedFaculty == "Waiting" && c.status !="done");
            //}

            //return View(items);

            var items = (from s in adb.a_alloted orderby s.a_choice.merit_position ascending select s).ToList();

            if (items != null)
            {
                items = items.Where(c => c.allotedFaculty == "Waiting" && c.status != "done").ToList();
            }

            return View(items);
        }

        public JsonResult GetSearchingData(string SearchValue)
        {
            List<a_alloted> StuList = new List<a_alloted>();
            StuList = adb.a_alloted.Where(x => x.studentId == SearchValue).ToList();
            return Json(StuList, JsonRequestBehavior.AllowGet);

        }

        public ActionResult a_check_post(string Id, string allotedFaculty, string allotedHall)
        {
            string[] subArray = new string[7];
            string[] hallArray = new string[4];
            string waiting_alloted_faculty = null;
            string waiting_alloted_hall = null;


            if (allotedFaculty == "Waiting" && allotedHall == "Waiting")
            {

                var choices = from s in adb.a_choice where s.userRoll == Id select s;

                // ------------------------ Subject Iterating ---------------------
                foreach (var item in choices)
                {
                    //Subjectssss
                    subArray[0] = item.first_choice;
                    subArray[1] = item.second_choice;
                    subArray[2] = item.third_choice;
                    subArray[3] = item.forth_choice;
                    subArray[4] = item.fifth_choice;
                    subArray[5] = item.sixth_choice;
                    subArray[6] = item.seventh_choice;
                    //hallssssssss
                    hallArray[0] = item.first_hall;
                    hallArray[1] = item.second_hall;
                    hallArray[2] = item.third_hall;
                    hallArray[3] = item.forth_hall;
                    // }

                    for (int i = 0; i < 7; i++)
                    {
                        string faculty = subArray[i];
                        int subCounter = subRemain(faculty);
                        if (subCounter > 0)
                        {
                            waiting_alloted_faculty = subArray[i];
                            break;
                        }

                    }

                    // ------------------------ Hall Iterating ---------------------

                    if (waiting_alloted_faculty == "ah" || waiting_alloted_faculty == "dvm")
                    {
                        waiting_alloted_hall = externalCampusHall(Id);

                    }

                    else
                    {
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

                }


                    var usr = adb.a_alloted.SingleOrDefault(u => u.studentId == Id);
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

                        var selectedStudent = (from p in adb.a_alloted where (p.studentId == Id ) select p).FirstOrDefault();
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

                var usr = adb.a_alloted.SingleOrDefault(u => u.studentId == Id);
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

                    var selectedStudent = (from p in adb.a_alloted where (p.studentId == Id) select p).FirstOrDefault();
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

        // ------------------------------------------------------------------- || User Defined Method || --------------------------------------------

        // subject allotment code

        int subRemain(string fac)
        {
            int ag_remain;
            int fish_remain;
            int nfs_remain;
            int dm_remain;
            int dvm_remain;
            int ah_remain;
            int lm_remain;

            if (fac == "ag")
                return ag_remain = TotalSeat.ag_seat - (from s in adb.allotCounts where s.allotedFaculty == "ag" select s).Count();

            else if (fac == "fish")
                return fish_remain = TotalSeat.fish_seat - (from s in adb.allotCounts where s.allotedFaculty == "fish" select s).Count();

            else if (fac == "nfs")
                return nfs_remain = TotalSeat.nfs_seat - (from s in adb.allotCounts where s.allotedFaculty == "nfs" select s).Count();

            else if (fac == "dm")
                return dm_remain = TotalSeat.dm_seat - (from s in adb.allotCounts where s.allotedFaculty == "dm" select s).Count();

            else if (fac == "dvm")
                return dvm_remain = TotalSeat.dvm_seat - (from s in adb.allotCounts where s.allotedFaculty == "dvm" select s).Count();

            else if (fac == "ah")
                return ah_remain = TotalSeat.ah_seat - (from s in adb.allotCounts where s.allotedFaculty == "ah" select s).Count();

            else if (fac == "lm")
                return lm_remain = TotalSeat.lm_seat - (from s in adb.allotCounts where s.allotedFaculty == "lm" select s).Count();

            else
                return 0;


        }

        // hall allotment code

        int hallRemain(string hall)
        {
            int sb1_remain;
            int sb2_remain;
            int kah_remain;
            int bbh_remain;
            int skh_remain;
            int fnh_remain;

            var details = from s in adb.allotCounts where (s.allotedFaculty == "ag" || s.allotedFaculty == "fish" || s.allotedFaculty == "lm" || s.allotedFaculty == "nfs" || s.allotedFaculty == "dm" || s.allotedFaculty == "dvm" || s.allotedFaculty == "ah") select s;


            if (hall == "SB1")
                return sb1_remain = TotalSeat.sb1 - (details = details.Where(c => c.allotedHall == "SB1")).Count();

            else if (hall == "SB2")
                return sb2_remain = TotalSeat.sb1 - (details = details.Where(c => c.allotedHall == "SB2")).Count();

            else if (hall == "KAH")
                return kah_remain = TotalSeat.sb1 - (details = details.Where(c => c.allotedHall == "KAH")).Count();

            else if (hall == "BBH")
                return bbh_remain = TotalSeat.sb1 - (details = details.Where(c => c.allotedHall == "BBH")).Count();

            else if (hall == "SKH")
                return skh_remain = TotalSeat.sb1 - (details = details.Where(c => c.allotedHall == "SKH")).Count();

            else if (hall == "FNH")
                return fnh_remain = TotalSeat.sb1 - (details = details.Where(c => c.allotedHall == "FNH")).Count();

            else
                return 0;

            //if (hall == "SB1")
            //    return sb1_remain = TotalSeat.sb1 - (from s in adb.allotCounts where s.allotedFaculty == "SB1" select s).Count();

            //else if (hall == "SB2")
            //    return sb2_remain = TotalSeat.sb2 - (from s in adb.allotCounts where s.allotedFaculty == "SB2" select s).Count();

            //else if (hall == "KAH")
            //    return kah_remain = TotalSeat.kah - (from s in adb.allotCounts where s.allotedFaculty == "KAH" select s).Count();

            //else if (hall == "BBH")
            //    return bbh_remain = TotalSeat.bbh - (from s in adb.allotCounts where s.allotedFaculty == "BBH" select s).Count();

            //else if (hall == "SKH")
            //    return skh_remain = TotalSeat.skh - (from s in adb.allotCounts where s.allotedFaculty == "SKH" select s).Count();

            //else if (hall == "FNH")
            //    return fnh_remain = TotalSeat.skh - (from s in adb.allotCounts where s.allotedFaculty == "FNH" select s).Count();

            //else
            //    return 0;

        }


        // dvm-ah hall allotment code
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