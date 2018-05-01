using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UIS.Models
{
    public class FiEnrollment
    {
        //public string courseCode { get;set;}
        //public string courseTitle { get; set; }
        //public string creditHour { get; set; }

        private cse_courseLayout courses = new cse_courseLayout();

        public cse_courseLayout Courses
        {
            get { return courses; }
            set { courses = value; }
        }

        public FiEnrollment()
        {

        }

        public FiEnrollment(cse_courseLayout courses)
        {
            this.courses = courses;
        }
    }
}