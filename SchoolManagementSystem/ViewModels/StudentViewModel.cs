using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.ViewModels
{
    public class StudentViewModels
    {
        public IEnumerable<Student> GetStudent { get; set; }
        public List<string> GetStudentClasses { get; set; }

        public IEnumerable<Class> GetClass { get; set; }
        public IEnumerable<Class> GetClassNamesAndIds { get; set; }
        public IEnumerable<Employee> GetEmployee { get; set; }
        public IEnumerable<Attendence> GetAttendence { get; set; }
        public IEnumerable<Day> GetDay { get; set; }

        public IEnumerable<Fee> GetFee { get; set; }
        public IEnumerable<Hour> GetHour { get; set; }
        public IEnumerable<Month> GetMonth { get; set; }
        public IEnumerable<Result> GetResult { get; set; }
        public IEnumerable<Salary> GetSalary { get; set; }
        public IEnumerable<Subject> GetSubject { get; set; }
        public IEnumerable<Teacher> GetTeacher { get; set; }
        public IEnumerable<TimeTable> GetTimeTable { get; set; }
        public IEnumerable<ClassDetail> GetClassDetails { get; set; }
        public List<string> GetExistingClasses { get; set; }
        public List<TimetablePivot_Result> GetTimeTablePivot { get; set; }


    }
}