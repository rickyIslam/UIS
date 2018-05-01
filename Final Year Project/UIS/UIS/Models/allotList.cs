using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdmissionDemo.Models
{
    using System;
    using System.Collections.Generic;

    public partial class allotList
    {
        public int id { get; set; }
        public string meritPostion { get; set; }
        public string status { get; set; }
        public string studentId { get; set; }
        public string allotedChoice { get; set; }
        public string allotedHall { get; set; }

    }
}