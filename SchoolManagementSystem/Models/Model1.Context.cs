﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Core.Objects.DataClasses;
    using System.Linq;
    
    public partial class SchoolEntities : DbContext
    {
        public SchoolEntities()
            : base("name=SchoolEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Attendence> Attendences { get; set; }
        public DbSet<ClassDetail> ClassDetails { get; set; }
        public DbSet<Date> Dates { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Hour> Hours { get; set; }
        public DbSet<Month> Months { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
    
    
        public virtual ObjectResult<TimetablePivot_Result> TimetablePivot(string @class)
        {
            var classParameter = @class != null ?
                new ObjectParameter("class", @class) :
                new ObjectParameter("class", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<TimetablePivot_Result>("TimetablePivot", classParameter);
        }
    }
}
