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
    
    public partial class Class
    {
        public string ClassId { get; set; }
        public int EmployeeId { get; set; }
        public string SubjectId { get; set; }
    
        public virtual ClassDetail ClassDetail { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
