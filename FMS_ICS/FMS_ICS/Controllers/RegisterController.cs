using FMS_ICS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMS_ICS.Controllers
{
    public class RegisterController : Controller
    {
        private readonly fms_db_icsEntities entities = new fms_db_icsEntities();
        // GET: Register
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(User user)
        {
            if(ModelState.IsValid)
            {
                entities.Users.Add(user);
                entities.SaveChanges();
                return RedirectToAction("Login", "Login");
            }

            return View(user);
        }
    }
}