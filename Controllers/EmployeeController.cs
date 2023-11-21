using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers
{
    public class EmployeeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new Zero_HungerEntities())
            {
                var employees = db.Employees.ToList();

                return View(employees);
            }

        }

        public ActionResult Add()
        {
            return View();
        }

        // POST: Restaurant/Add
        [HttpPost]
        public ActionResult Add(Employee emp)
        {
            using (var db = new Zero_HungerEntities())
            {
                db.Employees.Add(emp);
                db.SaveChanges();
                TempData["msg"] = "Employee Add Successfull";
                return RedirectToAction("Add");
            }
        }

        public ActionResult Edit(int ID)
        {
            using (var db = new Zero_HungerEntities())
            {
                var restaurant = db.Employees.Find(ID);
                if (restaurant == null)
                {
                    return HttpNotFound();
                }

                ViewBag.Id = restaurant.ID;
                ViewBag.Name = restaurant.Name;
                ViewBag.ContactInfo = restaurant.ContactInfo;

                return View();
            }
        }


        [HttpPost]
        public ActionResult Edit(Employee restaurant)
        {
            using (var db = new Zero_HungerEntities())
            {
                var upd = (from p in db.Employees
                           where p.ID == restaurant.ID
                           select p).SingleOrDefault();
                db.Entry(upd).CurrentValues.SetValues(restaurant);
                db.SaveChanges();
                TempData["msg"] = "Edit Successfull";

                return RedirectToAction("Edit");
            }
        }
        public ActionResult Delete(int id)
        {
            using (var db = new Zero_HungerEntities())
            {
                var request = db.Employees.Find(id);
                if (request == null)
                {
                    return HttpNotFound();
                }
                else
                    db.Employees.Remove(request);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        // GET: Employee
        public ActionResult Assign()
        {
            using (var db = new Zero_HungerEntities())
            {
                var restaurant = db.Restaurants.ToList();

                var requests = db.CollectRequests.Where(r => r.EmployeeId == null).ToList();

                var employees = db.Employees.ToList();


                ViewBag.Requests = requests;
                ViewBag.Employees = employees;

                return View(restaurant);
            }
        }

        [HttpPost]
        public ActionResult Assign(int requestId, int employeeId)
        {
            using (var db = new Zero_HungerEntities())
            {
                var request = db.CollectRequests.Find(requestId);
                var employee = db.Employees.Find(employeeId);

                if (request != null && employee != null)
                {
                    request.EmployeeId = employee.ID;
                    db.Entry(request).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["msg"] = "Employee assigned successfully!";
                }
                else
                {
                    TempData["error"] = "Invalid request or employee!";
                }
            }

            return RedirectToAction("Assign");
        }
    }
}
