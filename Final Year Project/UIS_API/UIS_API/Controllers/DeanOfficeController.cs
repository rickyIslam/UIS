using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UIS_API.Models;
namespace UIS_API.Controllers
{
    public class DeanOfficeController : ApiController
    {

        private BankContext db = new BankContext();

        public IHttpActionResult Get()
        {

            var deanOfficeTran = from s in db.deanOffices select s;
            return Ok(deanOfficeTran);
           // return Ok("Hiiii .... Ricky Islam");
        }

        public IHttpActionResult Get(string id)
        {
            var deanOfficeTran = from s in db.deanOffices select s;
            deanOfficeTran = deanOfficeTran.Where(c => c.studentId == id);
            //semesterFeeInfo = semesterFeeInfo.Where(c => c.payForSemester == semester);
            return Ok(deanOfficeTran);

        }

    }
}
