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
    public class SemInfoController : ApiController
    {
        private BankContext db = new BankContext();

        public IHttpActionResult Get()
        {

            var semesterFeeInfo = from s in db.TempBanks select s;
            return Ok(semesterFeeInfo);
        }

        public IHttpActionResult Get(string id)
        {
            var semesterFeeInfo = from s in db.TempBanks select s;
            semesterFeeInfo = semesterFeeInfo.Where(c => c.studentId == id);
          //  semesterFeeInfo = semesterFeeInfo.Where(c => c.payForSemester == semester);
            return Ok(semesterFeeInfo);

        }
    }
}
