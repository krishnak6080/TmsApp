using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TmsApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace TmsApp.Controllers
{
    [Authorize]
    public class AllocationController : Controller
    {
        private readonly TMSContext db;

        public AllocationController(TMSContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            List<Allocation> AList = db.Allocations.ToList();
            return View(AList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(db.Employees, "EmployeeId", "EmployeeId");
            ViewData["VehicleNumber"] = new SelectList(db.Vehicles, "VehicleNumber", "VehicleNumber");
            ViewData["RouteNumber"] = new SelectList(db.RoutePaths, "RouteNumber", "RouteNumber");
            
            return View();
        }

        [HttpPost]
        public IActionResult Create(Allocation a)
        {
            
            Vehicle v = new();
            v = db.Vehicles.Find(a.VehicleNumber);
            if ((v.AvailableSeats-1) > 0)
            {
                v.AvailableSeats = v.AvailableSeats - 1;
                db.Vehicles.Update(v);
                db.SaveChanges();
                db.Allocations.Add(a);
                db.SaveChanges();
            }
            else
            {
                v.Operable = false;
                db.Update(v);
                db.SaveChanges();
            }

            ViewData["EmployeeId"] = new SelectList(db.Employees, "EmployeeId", "EmployeeId", a.EmployeeId);
            ViewData["VehicleNumber"] = new SelectList(db.Vehicles, "VehicleNumber", "VehicleNumber",a.VehicleNumber);
            ViewData["RouteNumber"] = new SelectList(db.RoutePaths, "RouteNumber", "RouteNumber",a.RouteNumber);
            
            return RedirectToAction("Index", "Allocation");
        }
        public IActionResult Details(int id)
        {
            Allocation a = db.Allocations.Find(id);
            return View(a);
        }
        public IActionResult Edit(int id)
        {
            Allocation a = db.Allocations.Find(id);
            ViewData["EmployeeId"] = new SelectList(db.Employees, "EmployeeId", "EmployeeId");
            ViewData["VehicleNumber"] = new SelectList(db.Vehicles, "VehicleNumber", "VehicleNumber");
            ViewData["RouteNumber"] = new SelectList(db.RoutePaths, "RouteNumber", "RouteNumber");
            
            return View(a);
        }
        [HttpPost]
        public IActionResult Edit(Allocation a)
        {
            db.Allocations.Update(a);
            db.SaveChanges();
            ViewData["EmployeeId"] = new SelectList(db.Employees, "EmployeeId", "EmployeeId", a.EmployeeId);
            ViewData["VehicleNumber"] = new SelectList(db.Vehicles, "VehicleNumber", "VehicleNumber", a.VehicleNumber);
            ViewData["RouteNumber"] = new SelectList(db.RoutePaths, "RouteNumber", "RouteNumber", a.RouteNumber);
            
            return RedirectToAction("Index", "Allocation");
        }
        public IActionResult Delete(int id)
        {
            Allocation a = db.Allocations.Find(id);
            return View(a);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            //Allocation a = db.Allocations.Find(id);
            //db.Allocations.Remove(a);
            //db.SaveChanges();
            //return RedirectToAction("Index", "Allocation");

            Allocation a = db.Allocations.Find(id);
            Vehicle v = new();
            v = db.Vehicles.Find(a.VehicleNumber);
            if ((v.AvailableSeats + 1) > 0)
            {
                v.AvailableSeats = v.AvailableSeats + 1;
                db.Vehicles.Update(v);
                db.SaveChanges();
                db.Allocations.Remove(a);
                db.SaveChanges();
            }
            else
            {
                v.Operable = true;
                db.Update(v);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Allocation");
        }
    }
}
