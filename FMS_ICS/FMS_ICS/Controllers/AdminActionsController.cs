using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMS_ICS.Models;

namespace FMS_ICS.Controllers
{
    public class AdminActionsController : Controller
    {
        //private fms_db_ics db = new fms_db_ics();
        fms_db_icsEntities db = new fms_db_icsEntities();
        // GET: AdminActions
        [HttpGet]
        public ActionResult Index()
        {
            //var users = db.Users.Where(u => u.Status == "Pending").ToList();
            var users = db.Users.Where(u => u.Status == "Pending").ToList();
            return View(users);
        }

        // Action to display the details of a specific user
        public ActionResult Verify(int id)
        {
            var user = db.Users.SingleOrDefault(u => u.UserID == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // Action to activate a user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activate(int id)
        {
            var user = db.Users.SingleOrDefault(u => u.UserID == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.Status = "Active";
            db.SaveChanges();

            // Redirect back to the Index view with the updated list of users
            var users = db.Users.Where(u => u.Status == "Pending").ToList();
            return View("Index", users);
        }

    }
}