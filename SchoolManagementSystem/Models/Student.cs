//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SchoolManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public partial class Student
    {
        public Student()
        {
            this.Attendences = new HashSet<Attendence>();
            this.Fees = new HashSet<Fee>();
            this.Results = new HashSet<Result>();
            this.Results1 = new HashSet<Result>();
        }
    
        public string RegistrationNumber { get; set; }
        public string StudentName { get; set; }
        public string ClassId { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public System.DateTime YearOfJoining { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string FatherOccupation { get; set; }
        public string BloodGroup { get; set; }
        public string Email { get; set; }
        public string StudentPhoto { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    
        public virtual ICollection<Attendence> Attendences { get; set; }
        public virtual ClassDetail ClassDetail { get; set; }
        public virtual ICollection<Fee> Fees { get; set; }
        public virtual ICollection<Result> Results { get; set; }
        public virtual ICollection<Result> Results1 { get; set; }
    }
}
