using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UIS.Models
{
    public class TempBankModel
    {
        public int ID { get; set; }
        public string studentId { get; set; }
        public string faculty { get; set; }
        public string dateTime { get; set; }
        public string payForSemester { get; set; }
        public string receiptId { get; set; }
        public double amount { get; set; }
    }
}