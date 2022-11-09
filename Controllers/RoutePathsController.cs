using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TmsApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Dynamic;

namespace TmsApp.Controllers
{
    [Authorize]
    public class RoutePathsController : Controller
    {
        private readonly TMSContext db;

        public RoutePathsController(TMSContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            List<RoutePath> RList = db.RoutePaths.ToList();
            return View(RList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["VehicleNumber"] = new SelectList(db.Vehicles, "VehicleNumber", "VehicleNumber");
            return View();
        }

        [HttpPost]
        public IActionResult Create(RoutePath r)
        {           
            if (db.RoutePaths.Find(r.RouteNumber) == null)
            {
      
                var rt = (from i in db.RoutePaths
                          where i.VehicleStop == r.VehicleStop
                          select i).ToList();
                if (rt.Count<=0)
                {
                    //ViewBag.rterr = "";
                    var rtv = (from i in db.RoutePaths
                               where i.VehicleNumber == r.VehicleNumber
                               select i).ToList();
                    if (rtv.Count <= 0)
                    {
                        db.RoutePaths.Add(r);
                        db.SaveChanges();
                        return RedirectToAction("Index", "RoutePaths");
                    }
                    else
                    {
                        ViewBag.rterr = "The vehicle seems to be serving another route";
                        return View(r);
                    }
                    
                }
                else
                {
                    ViewBag.rterr = "Route already exists";
                    return View(r);
                }
            }


            else
            {
                ViewBag.routeerr = "Route Number already exists!";
                return View(r);
            }         
            return RedirectToAction("Index", "RoutePaths");
        }
        public IActionResult Details(int id)
        {
            RoutePath r = db.RoutePaths.Find(id);
            return View(r);
        }
        public IActionResult Edit(int id)
        {
            RoutePath r = db.RoutePaths.Find(id);
            
            return View(r);
        }
        [HttpPost]
        public IActionResult Edit(RoutePath r)
        {
            db.RoutePaths.Update(r);
            db.SaveChanges();
            
            return RedirectToAction("Index", "RoutePaths");
        }
        public IActionResult Delete(int id)
        {
            RoutePath r = db.RoutePaths.Find(id);
            return View(r);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            RoutePath r = db.RoutePaths.Find(id);
            var rt = (from i in db.Allocations
                      where i.RouteNumber == r.RouteNumber
                      select i).ToList();
            if (rt.Count <= 0)
            {
                db.RoutePaths.Remove(r);
                db.SaveChanges();
                return RedirectToAction("Index", "RoutePaths");
            }
            else
            {
                ViewBag.delerr = "Allocated route cannot be deleted";
                return View(r);
            }
        }
    }
}
