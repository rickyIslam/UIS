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
    
    public partial class Student
    {
        public Student()
        {
            this.DeanApprovedEntries = new HashSet<DeanApprovedEntry>();
            this.enrolledStudents = new HashSet<enrolledStudent>();
            this.FiSemesterFees = new HashSet<FiSemesterFee>();
            this.HallFees = new HashSet<HallFee>();
            this.HallTransactions = new HashSet<HallTransaction>();
            this.Posts = new HashSet<Post>();
            this.SemesterFees = new HashSet<SemesterFee>();
            this.FiEnrolledStudents = new HashSet<FiEnrolledStudent>();
        }
    
        public string studentId { get; set; }
        public string fullName { get; set; }
        public string nickName { get; set; }
        public string Dob { get; set; }
        public string Faculty { get; set; }
        public string Registration { get; set; }
        public string Session { get; set; }
        public string Hall { get; set; }
        public string Father { get; set; }
        public string Mother { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Town { get; set; }
        public string Po { get; set; }
        public string Ps { get; set; }
        public string District { get; set; }
        public string Photo { get; set; }
    
        public virtual ICollection<DeanApprovedEntry> DeanApprovedEntries { get; set; }
        public virtual ICollection<enrolledStudent> enrolledStudents { get; set; }
        public virtual ICollection<FiSemesterFee> FiSemesterFees { get; set; }
        public virtual ICollection<HallFee> HallFees { get; set; }
        public virtual ICollection<HallTransaction> HallTransactions { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<SemesterFee> SemesterFees { get; set; }
        public virtual ICollection<FiEnrolledStudent> FiEnrolledStudents { get; set; }
    }
}
