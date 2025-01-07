using FMS_ICS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMS_ICS.Controllers
{
    public class LoginController : Controller
    {
        fms_db_icsEntities entities = new fms_db_icsEntities();

        //GET
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            //check if the fields are empty
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Username and Password is Required";
                return View();
            }

            var user = entities.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if(user != null)
            {
                //login success
                Session["UserId"] = user.UserID;
                Session["Username"] = user.Username;
                return RedirectToAction("Dashboard", "Home"); //actionmethod followed by controller in which its present
            }
            return RedirectToAction("SignUp", "Register");
        }
    }
}