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
    
    public partial class Attendence
    {
        public string RegistrationNumber { get; set; }
        public string ClassId { get; set; }
        public int MonthId { get; set; }
        public int Date { get; set; }
        public string Year { get; set; }
        public string Status { get; set; }
    
        public virtual ClassDetail ClassDetail { get; set; }
        public virtual Month Month { get; set; }
        public virtual Student Student { get; set; }
    }
}
