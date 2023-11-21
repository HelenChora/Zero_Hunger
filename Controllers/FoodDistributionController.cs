using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ZeroHunger.EF;


namespace ZeroHunger.Controllers
{
    public class FoodDistributionController : Controller
    {
        // GET: FoodDistribution
        public ActionResult Index()
        {
            using (var db = new Zero_HungerEntities())
            {
                var Food = (from f in db.FoodDistributions from e in db.Employees
                            where e.ID == f.EmployeeID select f).ToList();

                var employees = db.Employees.ToList();

                var collection = db.CollectRequests.ToList();


                ViewBag.Employees = employees;
                ViewBag.Collections = collection;

                return View(Food);
            }
        }
        [HttpPost]
        public ActionResult Index(FoodDistribution food, int EmployeeID, int CollectRequestID)
        {
            using (var db = new Zero_HungerEntities())
            {
                food.EmployeeID = EmployeeID;
                food.CollectRequestID = CollectRequestID;
                food.RequestType = "Pending";
                db.FoodDistributions.Add(food);
                db.SaveChanges();
                TempData["msg"] = "Employee Add Successfull";
                return RedirectToAction("Index");
            }
        }

        public ActionResult Complete(int id)
        {
            using (var db = new Zero_HungerEntities())
            {
                var request = db.CollectRequests.Find(id);

                var food = (from f in db.FoodDistributions where f.CollectRequestID == id select f).SingleOrDefault();

                if (food != null)
                {
                    food.RequestType = "Complete";
                    db.Entry(food).State = EntityState.Modified;
                    db.SaveChanges();

                    
                }
                else
                {
                    TempData["error"] = "Invalid request!";
                }

                if (request != null)
                {
                    request.Status = "Complete";
                    db.Entry(request).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["msg"] = "Request marked as completed successfully!";
                }
                else
                {
                    TempData["error"] = "Invalid request!";
                }
            }

            return RedirectToAction("Index");
        }
    }
}