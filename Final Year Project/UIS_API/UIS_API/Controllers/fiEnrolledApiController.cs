using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UIS_API.Models;

namespace UIS_API.Controllers
{
    public class fiEnrolledApiController : ApiController
    {

        UisContext db = new UisContext();

        public IEnumerable<EnrolledProp> Get()
        {
            List<EnrolledProp> epl = new List<EnrolledProp>();
            var itemList = (from s in db.FiEnrolledStudents select s).ToList();//db.enrolledStudents.ToList();

            //if (itemList != null)
            //{
            //    itemList = itemList.Where(c => c.Student.Session == session).ToList();
            //    itemList = itemList.Where(c => c.faculty == faculty).ToList();
            //    itemList = itemList.Where(c => c.payForSemester == semester).ToList();
            //}


            foreach (FiEnrolledStudent item in (List<FiEnrolledStudent>)itemList)
            {
                EnrolledProp ep = new EnrolledProp();
                ep.name = item.Student.fullName.ToString();
                ep.studentId = item.studentId;
                ep.registration = item.Student.Registration.ToString();
                ep.session = item.Student.Session.ToString();
                ep.phone = item.Student.Phone.ToString();
                ep.email = item.Student.Email.ToString();
                epl.Add(ep);
            }

            return epl;
        }


        public IEnumerable<EnrolledProp> Get(string session, string faculty, string semester)
        {
            List<EnrolledProp> epl = new List<EnrolledProp>();
            var itemList = (from s in db.FiEnrolledStudents select s).ToList();//db.enrolledStudents.ToList();

            if (itemList != null)
            {
                itemList = itemList.Where(c => c.Student.Session == session).ToList();
                itemList = itemList.Where(c => c.faculty == faculty).ToList();
                itemList = itemList.Where(c => c.payForSemester == semester).ToList();
            }


            foreach (FiEnrolledStudent item in (List<FiEnrolledStudent>)itemList)
            {
                EnrolledProp ep = new EnrolledProp();
                ep.name = item.Student.fullName.ToString();
                ep.studentId = item.studentId;
                ep.registration = item.Student.Registration.ToString();
                ep.session = item.Student.Session.ToString();
                ep.phone = item.Student.Phone.ToString();
                ep.email = item.Student.Email.ToString();
                epl.Add(ep);
            }

            return epl;
        }
    }
}
