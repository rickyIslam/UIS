﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class UisContext : DbContext
    {
        public UisContext()
            : base("name=UisContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AccountOfficeAuth> AccountOfficeAuths { get; set; }
        public virtual DbSet<adminStatu> adminStatus { get; set; }
        public virtual DbSet<chat> chats { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<cse_courseLayout> cse_courseLayout { get; set; }
        public virtual DbSet<DeanApprovedEntry> DeanApprovedEntries { get; set; }
        public virtual DbSet<DeanOfficeAuth> DeanOfficeAuths { get; set; }
        public virtual DbSet<enrolledStudent> enrolledStudents { get; set; }
        public virtual DbSet<enrollmentStatu> enrollmentStatus { get; set; }
        public virtual DbSet<FiSemesterFee> FiSemesterFees { get; set; }
        public virtual DbSet<HallFee> HallFees { get; set; }
        public virtual DbSet<HallProvostAuth> HallProvostAuths { get; set; }
        public virtual DbSet<HallTransaction> HallTransactions { get; set; }
        public virtual DbSet<MailAuth> MailAuths { get; set; }
        public virtual DbSet<NoticeBoard> NoticeBoards { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<SemesterFee> SemesterFees { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<TempBank> TempBanks { get; set; }
        public virtual DbSet<FiEnrolledStudent> FiEnrolledStudents { get; set; }
    }
}
