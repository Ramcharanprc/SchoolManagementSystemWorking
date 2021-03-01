using SchoolManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SchoolManagementSystem.ViewModels;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace SchoolManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Student(string data)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                StudentViewModels model = new StudentViewModels();
                if (CheckLoginStatus())
                {
                    if (data == null || data == "")
                    {
                        data = db.Students.Max(student => student.ClassId);
                    }
                    model.GetStudent = db.Students.Where(Student => Student.ClassId.Equals(data) || data == null).ToList();
                    model.GetStudentClasses = db.Students.GroupBy(Class => Class.ClassId).Select(Class => Class.Key).ToList();
                    model.GetClassDetails = db.ClassDetails.ToList().OrderByDescending(Class => Convert.ToInt32(Class.ClassId));

                    return View(model);
                }
                else
                {
                    return ReturnToLogin();
                }
            }

        }

        public ActionResult Index()
        {
            if (CheckLoginStatus())
            {
                return View();
            }
            else
            {
                return ReturnToLogin();
            }
        }

        public ActionResult Classes(String data)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                StudentViewModels model = new StudentViewModels();
                if (CheckLoginStatus())
                {
                    if (data == null)
                    {
                        data = "10";
                    }
                    model.GetClass = db.Classes.Include("Employee").Include("Subject").Include("ClassDetail").ToList().OrderByDescending(Class
                        => Convert.ToInt32(Class.ClassId)).Where(Class => Class.ClassId == data);

                    model.GetClassDetails = db.ClassDetails.ToList().OrderByDescending(Class => Convert.ToInt32(Class.ClassId));
                    return View(model);
                }
                else
                {
                    return ReturnToLogin();
                }
            }
        }
        public ActionResult TimeTable(string selectedClass)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                if (CheckLoginStatus())
                {
                    //HI
                    if (selectedClass == null)
                    {
                        selectedClass = db.TimeTables.Max(timetable => timetable.ClassId);
                    }
                    if (Session["success"] != null && Session["success"].Equals("TimetableDeleted"))
                    {
                        ViewBag.Successful = "Table deleted successfully.";
                        Session["success"] = null;
                    }
                    else if (Session["error"] != null)
                    {
                        if (Session["error"].Equals("DatabaseError"))
                        {
                            ViewBag.Error = "It caused an error while deleting the Timetable.";
                            Session["error"] = null;
                        }
                        else if (Session["error"].Equals("NoTimetable"))
                        {
                            ViewBag.Error = "There is no table created for this class.";
                            Session["error"] = null;
                        }
                    }
                    StudentViewModels model = new StudentViewModels();
                    model.GetTimeTablePivot = db.TimetablePivot(selectedClass).ToList();
                    db.ClassDetails.ToList().OrderByDescending(Classs => Convert.ToInt32(Classs.ClassId));
                    model.GetExistingClasses = db.TimeTables.GroupBy(ClassList => ClassList.ClassId).Select(ClassList => ClassList.Key).ToList();
                    model.GetClass = db.Classes.Include("ClassDetail").Where(Class => Class.ClassId == selectedClass).ToList();
                    model.GetClassNamesAndIds = db.Classes.Include("ClassDetail").OrderByDescending(Classs => Classs.ClassId).ToList();
                    model.GetDay = db.Days.ToList();
                    return View(model);
                }
                {
                    return ReturnToLogin();
                }
            }
        }
        public ActionResult Teachers()
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                if (CheckLoginStatus())
                {

                    var detail = db.Employees.Where(Employee => (Employee.UserType.Equals("Teacher") || Employee.UserType.Equals("admin")) && Employee.Status == "A").ToList();
                    return View(detail);
                }
                else
                {
                    return ReturnToLogin();
                }
            }
        }
        public ActionResult Administration()
        {
            if (CheckLoginStatus())
            {
                return View();
            }
            else
            {
                return ReturnToLogin();
            }
        }
        public ActionResult Notifications()
        {
            if (CheckLoginStatus())
            {
                using(SchoolEntities db = new SchoolEntities())
                {
                    var notification = db.Notifications.OrderByDescending(sort => sort.id).ToList();
                    if(Session["success"] != null && Session["Success"].Equals("Notification deleted"))
                    {
                        ViewBag.Successful = "Notification deleted successfully.";
                        Session["success"] = null;
                    }
                    else if(Session["error"] != null && Session["error"].Equals("error"))
                    {
                        ViewBag.Error = "There is an issue while deleteing the Notification.";
                        Session["error"] = null;
                    }
                    return View(notification);
                }

            }
            else
            {
                return ReturnToLogin();
            }
        }
        public ActionResult ChangePassword()
        {
            if (CheckLoginStatus())
            {
                if (Session["success"] != null && Session["success"].Equals("Password changed"))
                {
                    ViewBag.Successful = "Password Changed successfully";
                    Session["success"] = null;
                }
                if (Session["error"] != null && Session["error"].Equals("newMismatch"))
                {
                    ViewBag.Error = "New password and conform password mismatch. Please check the password you entered.";
                    Session["error"] = null;
                }
                else if (Session["error"] != null && Session["error"].Equals("oldMismatch"))
                {
                    ViewBag.Error = "Old password you entered is wrong. Please check the password you entered.";
                    Session["error"] = null;
                }
                else if(Session["error"] != null && Session["error"].Equals("Old and New passwords are same"))
                {
                    ViewBag.Error = "Old password and New password cannot be the same. Please try different password.";
                    Session["error"] = null;
                }
                return View();
            }
            else
            {
                return ReturnToLogin();
            }
        }
        public ActionResult alterPassword(string oldPassword, string newPassword, string confirmPassword)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                if (Session["Password"].Equals(oldPassword))
                {
                    if (oldPassword.Equals(newPassword))
                    {
                        Session["error"] = "Old and New passwords are same";
                    }
                    else if (newPassword.Equals(confirmPassword))
                    {
                        int id = (int)(Session["EmployeeId"]);
                        var employeeDetails = db.Employees.Single(Employee => Employee.EmployeeId == id);
                        employeeDetails.Password = newPassword;
                        db.SaveChanges();
                        Session["success"] = "Password changed";
                        Session["Password"] = newPassword;
                    }
                    else
                    {
                        Session["error"] = "newMismatch";
                    }
                }
                else
                {
                    Session["error"] = "oldMismatch";
                }
            }
            return RedirectToAction("ChangePassword");
        }

        public bool CheckLoginStatus()
        {
            if (Session["EmployeeId"] != null)
            {
                return true;
            }
            else
                return false;
        }
        public ActionResult ReturnToLogin()
        {
            return RedirectToAction("LoginView", "login");

        }
    }
}
