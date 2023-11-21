using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers
{
    public class NGOController : Controller
    {
        // GET: NGO
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Name, string Password)
        {
            using (var db = new Zero_HungerEntities())
            {
                var user = db.NGOs.SingleOrDefault(u => u.Name == Name && u.Password == Password);

                if (user != null)
                {
                    Session["UserID"] = user.ID;
                    Session["UserName"] = user.Name;
                    return RedirectToAction("Index", "NGO");
                }

                ViewBag.Error = "Invalid username or password";
                return View("Login");
            }
        }
        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("login");
        }

    }
}