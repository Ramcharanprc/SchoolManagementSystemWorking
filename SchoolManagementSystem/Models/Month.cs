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
    
    public partial class Month
    {
        public Month()
        {
            this.Attendences = new HashSet<Attendence>();
            this.Salaries = new HashSet<Salary>();
        }
    
        public int MonthId { get; set; }
        public string Month1 { get; set; }
    
        public virtual ICollection<Attendence> Attendences { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
    }
}
