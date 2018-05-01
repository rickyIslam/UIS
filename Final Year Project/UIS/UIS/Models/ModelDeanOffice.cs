using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UIS.Models
{
    public class ModelDeanOffice
    {
        public int ID { get; set; }
        public string studentId { get; set; }
        public string faculty { get; set; }
        public string dateTime { get; set; }
        public string purpose { get; set; }
        public string receiptId { get; set; }
        public double amount { get; set; }
    }
}