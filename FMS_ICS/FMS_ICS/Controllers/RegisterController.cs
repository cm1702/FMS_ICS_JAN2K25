using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMS_ICS.Models;

namespace FMS_ICS.Controllers
{
    public class RegisterController : Controller
    {
        public string GenerateCardNumber()
        {
            // Use a random generator to ensure randomness
            Random random = new Random();

            // Generate a 16-digit card number
            string cardNumber = string.Concat(Enumerable.Range(0, 16).Select(_ => random.Next(0, 10).ToString()));

            // Check if the card number is unique in the database
            while (db.UserCards.Any(c => c.CardNumber == cardNumber))
            {
                cardNumber = string.Concat(Enumerable.Range(0, 16).Select(_ => random.Next(0, 10).ToString()));
            }

            return cardNumber;
        }
        fms_db_icsEntities db = new fms_db_icsEntities();

        // GET: Register
        public ActionResult Index()
        {
            
            var cardTypes = db.CardTypes.ToList();

            ViewBag.CardTypes = new SelectList(cardTypes, "CardTypeID", "CardType1");

            return View();
           
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user, int selectedCardType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check eligibility for the selected card type
                    var cardType = db.CardTypes.FirstOrDefault(c => c.CardTypeID == selectedCardType);
                    if (cardType == null)
                    {
                        ModelState.AddModelError("selectedCardType", "Invalid card type selected.");
                        throw new Exception("Invalid card type.");
                    }

                   // Save user details in the database
                    user.RegistrationDate = DateTime.Now;
                    user.Status = "Pending";
                    db.Users.Add(user);
                    db.SaveChanges();

                    // Create user card entry
                    var userCard = new UserCard
                    {
                        UserID = user.UserID,
                        CardTypeID = selectedCardType,
                        CardNumber = GenerateCardNumber(),
                        RemainingLimit = cardType.LimitAmount,
                        Validity = DateTime.Now.AddYears(5), // Set validity period
                        Status = "Inactive"
                    };
                    db.UserCards.Add(userCard);
                    db.SaveChanges();

                    // Create document verification entry
                    var documentVerification = new DocumentVerification
                    {
                        UserID = user.UserID,
                        DocumentType = "KYC",
                        DocumentStatus = "Pending",
                        Remarks = "Awaiting Verification"
                    };
                    db.DocumentVerifications.Add(documentVerification);
                    db.SaveChanges();

                    // Redirect to success page
                    return RedirectToAction("Success");
                }
            }
            catch (Exception ex)
            {
                // Log exception for debugging
                System.Diagnostics.Debug.WriteLine("Error in registration: " + ex.Message);
            }

            // Repopulate card types dropdown in case of an error
            ViewBag.CardTypes = new SelectList(db.CardTypes, "CardTypeID", "CardType1");
            return View(user);
        }

    }

    public class LoginController : Controller
    {
        fms_db_icsEntities db = new fms_db_icsEntities();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // POST: User Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserLogin(string username, string password)
        {
            try
            {
                var user = db.Users.FirstOrDefault(u =>
                    u.Username == username &&
                    u.Password == password &&
                    u.Status == "Active"
                );

                if (user != null)
                {
                    // Implement session management
                    Session["UserID"] = user.UserID;
                    return RedirectToAction("Dashboard", "User");
                }

                ModelState.AddModelError("", "Invalid login attempt");
            }
            catch
            {
                // Log error and handle exception
            }

            return View("Index");
        }

        // POST: Admin Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(string username, string password)
        {
            try
            {
                var admin = db.Admins.FirstOrDefault(a =>
                    a.Username == username &&
                    a.Password == password
                );

                if (admin != null)
                {
                    // Implement session management
                    Session["AdminID"] = admin.AdminID;
                    return RedirectToAction("Dashboard", "Admin");
                }

                ModelState.AddModelError("", "Invalid login attempt");
            }
            catch
            {
                // Log error and handle exception
            }

            return View("Index");
        }

        // Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

    }
}