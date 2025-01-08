using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMS_ICS.Models;

namespace FMS_ICS.Controllers
{
    public class ProductController : Controller
    {
        private readonly fms_db_icsEntities entities = new fms_db_icsEntities();

        // GET: Product
        public ActionResult Index()
        {
            var products = entities.Products.ToList(); 
            return View(products); 
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FMS_ICS.Models.Product product)
        {
            if (ModelState.IsValid)
            {
                entities.Products.Add(product); 
                entities.SaveChanges(); 
                return RedirectToAction("Index"); 
            }
            return View(product); 
        }

        // GET: Product/Edit
        public ActionResult Edit(int id)
        {
            var product = entities.Products.Find(id); 
            if (product == null)
            {
                return HttpNotFound(); 
            }
            return View(product);
        }

        // POST: Product/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                entities.Entry(product).State = System.Data.Entity.EntityState.Modified; 
                entities.SaveChanges(); 
                return RedirectToAction("Index");
            }
            return View(product); 
        }

        // GET: Product/Delete
        public ActionResult Delete(int id)
        {
            var product = entities.Products.Find(id); 
            if (product == null)
            {
                return HttpNotFound(); 
            }
            return View(product);
        }

        // POST: Product/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = entities.Products.Find(id); 
            if (product != null)
            {
                entities.Products.Remove(product); 
                entities.SaveChanges(); 
            }
            return RedirectToAction("Index");
        }
    }
}