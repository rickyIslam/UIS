//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UIS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SemesterFee
    {
        public int ID { get; set; }
        public string studentId { get; set; }
        public string faculty { get; set; }
        public string dateTime { get; set; }
        public string payForSemester { get; set; }
        public string receiptId { get; set; }
        public Nullable<double> amount { get; set; }
        public string hallClearance { get; set; }
        public string status { get; set; }
    
        public virtual Student Student { get; set; }
    }
}
