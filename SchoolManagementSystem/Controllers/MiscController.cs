using Newtonsoft.Json;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Controllers
{
    public class MiscController : Controller
    {
        // GET: Misc
        public ActionResult StudentView(string RegistrationId)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                var StudentDetail = db.Students.Include("ClassDetail").Where(Student => Student.RegistrationNumber.Equals(RegistrationId)).ToList();
                // ViewBag.Message = RegistrationId; // get registration Id from student page
                return View(StudentDetail);
            }

        }
        public ActionResult TeacherView(int EmployeeId)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                var TeacherDetail = db.Employees.Where(x => x.EmployeeId == EmployeeId).ToList();
                // ViewBag.Message = RegistrationId; // get registration Id from student page
                return View(TeacherDetail);
            }
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult AddNewStudent(string StudentName, string FatherName, string MotherName, string YearOfJoining, string Class,
              string Address, string ContactNumber, string DateOfBirth, string Gender, string FatherOccupation, string BloodGroup, string Email, HttpPostedFileBase StudentPhoto)
        {
            try
            {
                string fileName = null;
                if (StudentPhoto != null)
                {
                    fileName = Path.GetFileNameWithoutExtension(StudentPhoto.FileName);
                    string extension = Path.GetExtension(StudentPhoto.FileName);
                    fileName = "~/Photos/" + fileName + DateTime.Now.ToString("yyddhhmm") + extension;
                    StudentPhoto.SaveAs(Server.MapPath(fileName));
                }
                using (SchoolEntities db = new SchoolEntities())
                {
                    if (StudentName != null)
                    {
                        SqlParameter parameter1 = new SqlParameter("@StudentName", StudentName);
                        SqlParameter parameter2 = new SqlParameter("@ClassId", Class);
                        SqlParameter parameter3 = new SqlParameter("@FatherName", FatherName);
                        SqlParameter parameter4 = new SqlParameter("@Mothername", MotherName);
                        SqlParameter parameter5 = new SqlParameter("@YearOfJoining", YearOfJoining);
                        SqlParameter parameter6 = new SqlParameter("@Address", Address);
                        SqlParameter parameter7 = new SqlParameter("@ContactNumber", ContactNumber);
                        SqlParameter parameter8 = new SqlParameter("@DateOfBirth", DateOfBirth);
                        SqlParameter parameter9 = new SqlParameter("@Gender", Gender);
                        SqlParameter parameter10 = new SqlParameter("@FatherOccupation", FatherOccupation);
                        SqlParameter parameter11 = new SqlParameter("@BloodGroup", BloodGroup);
                        SqlParameter parameter12 = new SqlParameter("@Email", Email);
                        SqlParameter parameter13 = new SqlParameter("@StudentPhoto", fileName);
                        var response = db.Database.ExecuteSqlCommand("StudentRegistration @StudentName,@ClassId , @FatherName, @MotherName, @YearOfJoining," +
                            " @Address, @ContactNumber, @DateOfBirth, @Gender, @FatherOccupation, @BloodGroup, @Email, @StudentPhoto",
                            parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7, parameter8, parameter9, parameter10, parameter11, parameter12, parameter13);
                    }
                    var classDetails = db.ClassDetails.ToList().OrderByDescending(Classs => Convert.ToInt32(Classs.ClassId));
                    return View(classDetails);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                return View();
            }

        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult AddNewEmployee(Employee employee)
        {
            try
            {
                if (employee.Name != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName);
                    string extension = Path.GetExtension(employee.ImageFile.FileName);
                    fileName = "~/Photos/" + fileName + DateTime.Now.ToString("yyddhhmm") + extension;
                    employee.Photo = fileName;
                    fileName = Path.Combine(Server.MapPath(fileName));
                    employee.ImageFile.SaveAs(fileName);
                    employee.Status = "A";
                    using (SchoolEntities db = new SchoolEntities())
                    {
                        db.Employees.Add(employee);
                        db.SaveChanges();
                    }
                    ModelState.Clear();
                }
                return View();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                return View();
            }
        }

        public ActionResult EditStudentDetails(String RegistrationNumber)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                if (Session["success"] != null && Session["success"].Equals("Students details edited successful"))
                {
                    ViewBag.Successful = "Student details modified successfully.";
                    Session["success"] = null;
                }
                var details = db.Students.Single(Employee => Employee.RegistrationNumber == RegistrationNumber);
                return View(details);
            }

        }
        [HttpPost]
        public ActionResult EditStudentDetails(String RegistrationNumber, Student student)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                Student studentObj = db.Students.Include("ClassDetail").Single(Student => Student.RegistrationNumber == RegistrationNumber);
                if (student.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(student.ImageFile.FileName);
                    string extension = Path.GetExtension(student.ImageFile.FileName);
                    fileName = "~/Photos/" + fileName + DateTime.Now.ToString("yyddhhmm") + extension;
                    student.StudentPhoto = fileName;
                    fileName = Path.Combine(Server.MapPath(fileName));
                    student.ImageFile.SaveAs(fileName);
                    studentObj.StudentPhoto = student.StudentPhoto;
                }
                string date = student.DateOfBirth.ToString("dd/MM/yyyy");
                if (date.Equals("01/01/0001") == false)
                {
                    studentObj.DateOfBirth = student.DateOfBirth;
                }
                studentObj.StudentName = student.StudentName;
                studentObj.FatherName = student.FatherName;
                studentObj.MotherName = student.MotherName;
                studentObj.Address = student.Address;
                studentObj.Address = student.Address;
                studentObj.BloodGroup = student.BloodGroup;
                studentObj.ContactNumber = student.ContactNumber;
                studentObj.Email = student.Email;
                studentObj.FatherOccupation = student.FatherOccupation;
                studentObj.Gender = student.Gender;
                studentObj.YearOfJoining = student.YearOfJoining;
                db.SaveChanges();
                Session["success"] = "Students details edited successful";
            }
            return RedirectToAction("EditStudentDetails", "Misc", new { @RegistrationNumber = RegistrationNumber });
        }
        public ActionResult EditEmployeeDetails(int employeeId)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                var employee = db.Employees.Single(Employee => Employee.EmployeeId == employeeId);
                return View(employee);
            }
        }
        [HttpPost]
        public ActionResult EditEmployeeDetails(int employeeId, Employee employee)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                var employeeDetails = db.Employees.Single(Employee => Employee.EmployeeId == employeeId);
                if (employee.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName);
                    string extension = Path.GetExtension(employee.ImageFile.FileName);
                    fileName = "~/Photos/" + fileName + DateTime.Now.ToString("yyddhhmm") + extension;
                    employee.Photo = fileName;
                    fileName = Path.Combine(Server.MapPath(fileName));
                    employee.ImageFile.SaveAs(fileName);
                    employeeDetails.Photo = employee.Photo;
                }
                string date = employee.DateOfBirth.ToString("dd/MM/yyyy");
                if (date.Equals("01/01/0001") == false)
                {
                    employeeDetails.DateOfBirth = employee.DateOfBirth;
                }
                employeeDetails.Name = employee.Name;
                employeeDetails.FatherName = employee.FatherName;
                employeeDetails.MotherName = employee.MotherName;
                employeeDetails.Address = employee.Address;
                employeeDetails.Designation = employee.Designation;
                employeeDetails.Email = employee.Email;
                employeeDetails.Experience = employee.Experience;
                employeeDetails.MobileNumber = employee.MobileNumber;
                employeeDetails.UserType = employee.UserType;
                employeeDetails.YearOfJoining = employee.YearOfJoining;
                db.SaveChanges();
            }
            return RedirectToAction("Teachers", "Home");
        }
        [HttpGet]
        public ActionResult AddTimeTable()
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                StudentViewModels models = new StudentViewModels();
                models.GetHour = db.Hours.ToList().OrderBy(Hour => Convert.ToInt32(Hour.HourId));
                models.GetSubject = db.Subjects.ToList();
                models.GetClassDetails = db.ClassDetails.ToList().OrderByDescending(Class => Convert.ToInt32(Class.ClassId));
                models.GetDay = db.Days.ToList();
                models.GetExistingClasses = db.TimeTables.GroupBy(Class => Class.ClassId).Select(Class => Class.Key).ToList();
                if (Session["success"] != null && Session["success"].Equals("Added"))
                {
                    ViewBag.Successful = "TimeTable created succesfully";
                    Session["success"] = null;
                }
                return View(models);
            }
        }
        [HttpPost]
        public ActionResult AddTimeTable(string[] SubjectId, string SelectedClass)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                SortedList<int, string> subjets = new SortedList<int, string>();
                for (int counter = 0; counter < SubjectId.Length; counter++)
                {
                    subjets.Add(counter, SubjectId[counter]);
                }
                string json = JsonConvert.SerializeObject(subjets);
                SqlParameter SubjectsList = new SqlParameter("@SubjectsList", json);
                SqlParameter ClassId = new SqlParameter("@ClassId", SelectedClass);
                var response = db.Database.ExecuteSqlCommand("CreateTimeTable @SubjectsList, @ClassId", SubjectsList, ClassId);
                if (response == SubjectId.Length)
                {
                    Session["Success"] = "Added";
                }
                else
                {
                    Session["Success"] = "false";
                }
            }
            ModelState.Clear();
            return RedirectToAction("AddTimeTable");
        }

        public ActionResult AddClass()
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                StudentViewModels models = new StudentViewModels();
                models.GetEmployee = db.Employees.Where(employee => employee.UserType == "Teacher" || employee.UserType == "admin").ToList();
                models.GetSubject = db.Subjects.ToList();
                if (Session["success"] != null && Session["success"].Equals("true"))
                {
                    ViewBag.Successful = "Class added successfully";
                    Session["success"] = null;
                }
                if (Session["error"] != null && Session["error"].Equals("Insert error"))
                {
                    ViewBag.Error = "There is a class available earlier. Please edit the class in classes page for modifications.";
                    Session["error"] = null;
                }
                return View(models);
            }
        }
        [HttpPost]
        public ActionResult AddClass(string classId, string className, string[] teacherDetails, string[] subjectDetails)
        {
            try
            {
                using (SchoolEntities db = new SchoolEntities())
                {
                    SortedList<int, string> teacherList = new SortedList<int, string>();
                    SortedList<int, string> subjectList = new SortedList<int, string>();
                    if (teacherDetails != null)
                    {
                        for (int counter = 0; counter < teacherDetails.Length; counter++)
                        {
                            if (subjectDetails[counter] != null)
                            {
                                teacherList.Add(counter, teacherDetails[counter]);
                                subjectList.Add(counter, subjectDetails[counter]);
                            }
                        }
                        string teacher = JsonConvert.SerializeObject(teacherList);
                        string subject = JsonConvert.SerializeObject(subjectList);
                        SqlParameter classIdParameter = new SqlParameter("@classId", classId);
                        SqlParameter classNameParameter = new SqlParameter("@className", className);
                        SqlParameter teacherParameter = new SqlParameter("@teacher", teacher);
                        SqlParameter subjectParameter = new SqlParameter("@subject", subject);
                        var response = db.Database.ExecuteSqlCommand("AddClass @classId, @className, @teacher, @subject", classIdParameter, classNameParameter, teacherParameter, subjectParameter);
                        if (response == teacherDetails.Length)
                        {
                            Session["success"] = "true";
                        }
                        else
                        {
                            Session["success"] = "false";
                        }
                    }
                    return RedirectToAction("AddClass");
                }
            }
            catch (SqlException dbEx)
            {
                if (dbEx.Number == 2627)
                {
                    Session["Error"] = "Insert error";

                }
                return RedirectToAction("AddClass");
            }
        }
        public ActionResult DeleteTimetable(string selectedClass)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                var columns = db.TimeTables.Where(Class => Class.ClassId == selectedClass).ToList();
                if (columns.Count == 48)
                {
                    SqlParameter classId = new SqlParameter("@ClassId", selectedClass);
                    var response = db.Database.ExecuteSqlCommand("DeleteTimeTable @ClassId", classId);
                    if (response == 48)
                    {
                        Session["success"] = "TimetableDeleted";
                    }
                    else
                    {
                        Session["error"] = "DatabaseError";
                    }
                }
                else
                {
                    Session["error"] = "NoTimetable";
                }
            }
            return RedirectToAction("TimeTable", "Home");
        }

        public ActionResult EditClass(string ClassId)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                StudentViewModels models = new StudentViewModels();
                models.GetClass = db.Classes.Include("Employee").Include("Subject").Include("ClassDetail").Where(Class => Class.ClassId == ClassId).ToList();
                models.GetClassDetails = db.ClassDetails.ToList().OrderByDescending(Class => Convert.ToInt32(Class.ClassId));
                models.GetEmployee = db.Employees.ToList();
                models.GetSubject = db.Subjects.ToList();
                if (Session["success"] != null && Session["success"].Equals("true"))
                {
                    ViewBag.Successful = "Modifications changed succesfully";
                    Session["success"] = null;
                }
                return View(models);
            }
        }
        [HttpPost]
        public ActionResult EditClass(string[] subjectDetails, string[] teacherDetails, string classId)

        {
            using (SchoolEntities db = new SchoolEntities())
            {
                SortedList<int, string> teacherDetailsList = new SortedList<int, string>();
                SortedList<int, string> subjectDetailsList = new SortedList<int, string>();
                for (int detailsCounter = 0; detailsCounter < teacherDetails.Length; detailsCounter++)
                {
                    teacherDetailsList.Add(detailsCounter, teacherDetails[detailsCounter]);
                    subjectDetailsList.Add(detailsCounter, subjectDetails[detailsCounter]);
                }
                string teacherJson = JsonConvert.SerializeObject(teacherDetailsList);
                string subjectJson = JsonConvert.SerializeObject(subjectDetailsList);
                SqlParameter classIdParameter = new SqlParameter("@classId", classId);
                SqlParameter teacher = new SqlParameter("@teacher", teacherJson);
                SqlParameter subject = new SqlParameter("@subject", subjectJson);
                var response = db.Database.ExecuteSqlCommand("EditClass @classId, @teacher, @subject", classIdParameter, teacher, subject);
                if (response > 0)
                {
                    Session["success"] = "true";
                }
                else
                {
                    Session["success"] = "false";
                }
            }
            return RedirectToAction("EditClass", new { @ClassId = classId });
        }
        public ActionResult EditTimetable(string selectedClass)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                StudentViewModels models = new StudentViewModels();
                if (selectedClass == null)
                {
                    selectedClass = "10";
                }
                models.GetTimeTablePivot = db.TimetablePivot(selectedClass).ToList();
                models.GetClass = db.Classes.Where(Class => Class.ClassId == selectedClass).ToList();
                models.GetHour = db.Hours.ToList();
                models.GetDay = db.Days.ToList();
                models.GetSubject = db.Subjects.ToList();
                if (Session["success"] != null && Session["success"].Equals("Edited"))
                {
                    ViewBag.Successful = "Timetable edited successfully!";
                    Session["success"] = null;
                }
                return View(models);
            }
        }
        [HttpPost]
        public ActionResult EditTimetable(string[] SubjectId, string SelectedClass)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                SortedList<int, string> subjets = new SortedList<int, string>();
                for (int counter = 0; counter < SubjectId.Length; counter++)
                {
                    subjets.Add(counter, SubjectId[counter]);
                }
                string json = JsonConvert.SerializeObject(subjets);
                SqlParameter SubjectsList = new SqlParameter("@SubjectsList", json);
                SqlParameter ClassId = new SqlParameter("@ClassId", SelectedClass);
                var response = db.Database.ExecuteSqlCommand("EditTimeTable @SubjectsList, @ClassId", SubjectsList, ClassId);
                if (response == SubjectId.Length * 2)
                {
                    Session["Success"] = "Edited";
                }
            }
            ModelState.Clear();
            return RedirectToAction("EditTimetable", new { @selectedClass = SelectedClass });
        }
        public ActionResult CreateNotification()
        {
            if (Session["success"] != null && Session["success"].Equals("Notification created"))
            {
                ViewBag.Successful = "Notification created successfully.";
                Session["success"] = null;
            }
            if (Session["error"] != null && Session["error"].Equals("error"))
            {
                ViewBag.Error = "There is a issue while saving the Notification. Please try again";
                Session["error"] = null;
            }
            return View();
        }
        [HttpPost]
        public ActionResult CreateNotification(Notification notification)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                SqlParameter title = new SqlParameter("@title", notification.Title);
                SqlParameter message = new SqlParameter("@message", notification.Message);
                var response = db.Database.ExecuteSqlCommand("createNotification @title, @message", title, message);
                if (response == 1)
                {
                    Session["success"] = "Notification created";
                }
                else
                {
                    Session["error"] = "error";
                }
                return RedirectToAction("CreateNotification");
            }
        }
        public ActionResult DeleteNotification(int id)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                SqlParameter NotificationId = new SqlParameter("@id", id);
                var response = db.Database.ExecuteSqlCommand("DeleteNotification @id", NotificationId);
                if (response == 1)
                {
                    Session["success"] = "Notification deleted";
                }
                else
                {
                    Session["error"] = "error";
                }
                return RedirectToAction("Notifications", "Home");
            }
        }
        public ActionResult ShowEmployee()
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                var list = db.Employees.Where(Employee => Employee.Status == "A").ToList();
                if (Session["success"] != null && Session["success"].Equals("Deleted"))
                {
                    ViewBag.Successful = "Employee details deleted successfully";
                    Session["success"] = null;
                }
                return View(list);
            }
        }
        public ActionResult DeleteEmployee(int employeeId)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                var employee = db.Employees.Single(data => data.EmployeeId == employeeId);
                if (employee.EmployeeId == employeeId)
                {
                    employee.Status = "I";
                    db.SaveChanges();
                    Session["success"] = "Deleted";
                }
            }
            return RedirectToAction("ShowEmployee", "Misc");
        }
        public ActionResult Salary(string month)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                if (month != null && !month.Equals(""))
                {
                    DateTime monthCasted = Convert.ToDateTime(month);
                    string monthSelected = monthCasted.ToString("MMMM");
                    string yearSelected = monthCasted.Year.ToString();
                    var salaryDetails = db.Salaries.Include("Employee").Include("Month").Where(data => data.Month.Month1 == monthSelected && data.Year == yearSelected).ToList();
                    if (salaryDetails.Count == 0)
                    {
                        ViewBag.NoData = "No data found with the month and year you provied.";
                        var SalaryDetails = db.Salaries.Include("Employee").Include("Month").Where(salary => salary.Year == db.Salaries.Max(year => year.Year) && salary.MonthId == db.Salaries.Max(year => year.MonthId)).ToList();
                        return View(SalaryDetails);
                    }
                    return View(salaryDetails);
                }
                else
                {
                    var SalaryDetails = db.Salaries.Include("Employee").Include("Month").Where(salary => salary.Year == db.Salaries.Max(year => year.Year) && salary.MonthId == db.Salaries.Max(year => year.MonthId)).ToList();
                    return View(SalaryDetails);
                }
            }
        }
        public ActionResult CreditSalary()
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                var Employee = db.Employees.ToList();
                if (Session["success"] != null && Session["success"].Equals("Salary entered"))
                {
                    ViewBag.Successful = "Salary Entered Successfully";
                    Session["success"] = null;
                }
                if (Session["error"] != null && Session["error"].Equals("error"))
                {
                    ViewBag.Error = "The Employee Salary for the perticular month is already submitted. Please go to edit section to modify the credentials";
                    Session["error"] = null;
                }
                return View(Employee);
            }
        }
        [HttpPost]
        public ActionResult CreditSalary(int employeeId, DateTime date, int daysAttended, float salary)
        {
            int monthSelected = Convert.ToInt32(date.ToString("MM"));
            string yearSelected = date.Year.ToString();
            using (SchoolEntities db = new SchoolEntities())
            {
                SqlParameter EmpId = new SqlParameter("@employeeId", employeeId);
                SqlParameter month = new SqlParameter("@month", monthSelected);
                SqlParameter year = new SqlParameter("@year", yearSelected);
                SqlParameter Amount = new SqlParameter("@salary", salary);
                SqlParameter days = new SqlParameter("@daysAttended", daysAttended);
                var response = db.Database.ExecuteSqlCommand("SalaryEntry @employeeId, @salary, @daysAttended, @month ,@year", EmpId, Amount, days, month, year);
                if (response == 1)
                {
                    Session["success"] = "Salary entered";
                }
                else
                {
                    Session["error"] = "error";
                }
                return RedirectToAction("CreditSalary", "Misc");
            }

        }
        public ActionResult EditSalary(int EmployeeId, string year, int monthId)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                var salaryData = db.Salaries.Include("Employee").Include("Month").Where(Salary => Salary.EmployeeId == EmployeeId && Salary.Year == year && Salary.MonthId == monthId).ToList();
                return View(salaryData);
            }
        }
        [HttpPost]
        public ActionResult EditSalary(int employeeId, int monthId, string year, int daysAttended, float salary)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                var SalaryDetail = db.Salaries.Single(data => data.EmployeeId == employeeId && data.MonthId == monthId && data.Year == year);
                SalaryDetail.DaysAttended = daysAttended;
                SalaryDetail.Salary1 = Convert.ToDecimal(salary);
                db.SaveChanges();
            }
            return View();
        }
        public ActionResult Attendence(string month, string classId)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                int selectedMonth = 0;
                var selectedYear = "";
                if ((classId == null || classId == "") && (month == null || month == ""))
                {
                    classId = GetMaxClass();
                    selectedMonth = GetMaxMonth();
                    selectedYear = GetMaxYear();
                }
                else if (classId != "" && month == "")
                {
                    selectedMonth = GetMaxMonth();
                    selectedYear = GetMaxYear();
                }
                else if (classId == "" && month != "")
                {
                    classId = GetMaxClass();
                    selectedMonth =Convert.ToInt32(Convert.ToDateTime(month).ToString("MM"));
                    selectedYear = Convert.ToDateTime(month).ToString("yyyy");
                }
                else if (classId != "" && month != "")
                {
                    selectedMonth = Convert.ToInt32(Convert.ToDateTime(month).ToString("MM"));
                    selectedYear = Convert.ToDateTime(month).ToString("yyyy");
                }
                ViewBag.Month = selectedMonth;
                ViewBag.Year = selectedYear;
                StudentViewModels models = new StudentViewModels();
                models.GetClassDetails = db.ClassDetails.ToList().OrderByDescending(Classs => Convert.ToInt32(Classs.ClassId));
                if (db.Attendences.Include("Student").Include("ClassDetail").Include("Month").Where(data => data.MonthId == selectedMonth && data.Year == selectedYear && data.ClassId == classId).OrderBy(data => data.RegistrationNumber).Count() == 0)
                {
                    ViewBag.NoDetails = "Details not found.";
                    classId = GetMaxClass();
                    selectedMonth = GetMaxMonth();
                    selectedYear = GetMaxYear();
                }
                models.GetAttendence = db.Attendences.Include("Student").Include("ClassDetail").Include("Month").Where(data => data.MonthId == selectedMonth && data.Year == selectedYear && data.ClassId == classId).OrderBy(data => data.RegistrationNumber).ToList();
                return View(models);

            }
        }

        public string GetMaxClass()
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                var Classs = db.Attendences.Max(Class => Class.ClassId);
                return Classs;
            }
        }
        public int GetMaxMonth()
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                int month = (db.Attendences.Max(attendence => attendence.MonthId));
                return month;
            }
        }
        public string GetMaxYear()
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                var year = (db.Attendences.Max(attendence => attendence.Year));
                return year;
            }
        }
        public ActionResult RecordAttendence(string classId)
         {
            using(SchoolEntities db = new SchoolEntities())
            {
                    StudentViewModels models = new StudentViewModels();
                    models.GetStudent = db.Students.Where(student => student.ClassId == classId).ToList();
                    models.GetClassDetails = db.ClassDetails.ToList().OrderByDescending(Classs => Convert.ToInt32(Classs.ClassId));
                    models.GetStudentClasses = db.Students.GroupBy(Class => Class.ClassId).Select(Class => Class.Key).ToList();

                if (Session["success"] != null && Session["success"].Equals("Attendence submitted"))
                {
                    ViewBag.Successful = "Attendence submitted successfully";
                    Session["success"] = null;
                }
                else if(Session["error"] != null && Session["error"].Equals("Attendence submission failed"))
                {
                    ViewBag.Error = "There is an issue while submitting the attendence. Please check the values if already existed.";
                    Session["error"] = null;
                }
                    return View(models);
            }

        }
        [HttpPost]
        public ActionResult RecordAttendence(string[] status, string classId, string date)
        {
            int month = Convert.ToInt32(Convert.ToDateTime(date).ToString("MM"));
            int Datedd = Convert.ToInt32(Convert.ToDateTime(date).ToString("dd"));
            var year = Convert.ToDateTime(date).ToString("yyyy");
            SortedList<int, string> RegisteredNumbers = new SortedList<int,string>();
            SortedList<int, string> Status = new SortedList<int, string>();
            using (SchoolEntities db = new SchoolEntities())
            {
                var studentDetail = db.Students.Where(student => student.ClassId == classId).ToList();               
                for (int counter = 0; counter < studentDetail.Count; counter++)
                {
                    RegisteredNumbers.Add(counter, studentDetail[counter].RegistrationNumber);
                    Status.Add(counter, status[counter]);
                }
                string RegisteredIdJson = JsonConvert.SerializeObject(RegisteredNumbers);
                string StatusJson = JsonConvert.SerializeObject(Status);
                SqlParameter Registration = new SqlParameter("@RegistrationNumber", RegisteredIdJson);
                SqlParameter statusParam = new SqlParameter("@status", StatusJson);
                SqlParameter dateParam = new SqlParameter("@date", Datedd);
                SqlParameter monthParam = new SqlParameter("@month", month);
                SqlParameter yearParam = new SqlParameter("@year", year);
                SqlParameter ClassIdParam = new SqlParameter("@ClassId", classId);
                try
                {
                    var response = db.Database.ExecuteSqlCommand("recordAttendence @RegistrationNumber, @ClassId, @status, @month, @date, @year", Registration, ClassIdParam, statusParam, monthParam, dateParam, yearParam);
                    if (response == studentDetail.Count)
                    {
                        Session["Success"] = "Attendence submitted";
                    }
                    else
                    {
                        Session["error"] = "Attendence submission failed";
                    }
                    return RedirectToAction("RecordAttendence", "Misc", new { classId = classId });
                }
                catch (Exception ex)
                {
                    Session["error"] = "Attendence submission failed";
                    return RedirectToAction("RecordAttendence", "Misc", new { classId = classId });

                }
            }

        }

    }
}