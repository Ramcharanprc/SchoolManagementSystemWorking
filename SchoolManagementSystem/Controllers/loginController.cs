using System.Web.Mvc;
using SchoolManagementSystem.Models;
using System;
using System.Linq;

namespace SchoolManagementSystem.Controllers
{
    public class loginController : Controller
    {
        // GET: login
        public ActionResult LoginView()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Aurtherize(SchoolManagementSystem.Models.Employee details)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                var userDetails = db.Employees.Where(x => x.Email == details.Email && x.Password == details.Password).FirstOrDefault();
                if(userDetails == null)
                {
                    details.LoginErrorMessage = "Wrong username or password";
                    return View("LoginView", details);
                }
                else
                {
                    Session["EmployeeId"] = userDetails.EmployeeId;
                    Session["Name"] = userDetails.Name;
                    Session["UserType"] = userDetails.UserType;
                    Session["Password"] = userDetails.Password;
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("LoginView", "login");
        }
    }
}