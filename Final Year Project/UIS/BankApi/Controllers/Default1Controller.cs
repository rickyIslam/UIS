using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BankApi.Models;

namespace BankApi.Controllers
{
    public class Default1Controller : ApiController
    {
        private BankContext db = new BankContext();


        public IHttpActionResult Get()
        {
           
            var studentDetails = from s in db.TempBanks select s;
            //studentDetails = studentDetails.Where(c => c.mail == id);
            return Ok(studentDetails);
        }

        public IHttpActionResult Get(string id)
        {
            //Student student = db.Students.Find(id);
            //if (student == null)
            //{
            //    return NotFound();
            //}

            //return Ok(student);

            //var studentDetails = from s in db.Students select s;
            //studentDetails = studentDetails.Where(c => c.studentId == id);
            //return Ok(studentDetails);

        }

    }
}