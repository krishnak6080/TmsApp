using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TmsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace TmsApp.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly TMSContext db;

        public EmployeesController(TMSContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            List<Employee> EList = db.Employees.ToList();
            return View(EList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["VehicleStop"] = new SelectList(db.RoutePaths, "VehicleStop", "VehicleStop");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee e)
        {
            
            if (db.Employees.Find(e.EmployeeId) == null)
            {
                var stopLoc = e.EmpLocation;
                RoutePath? stop = (from i in db.RoutePaths
                            where i.VehicleStop == stopLoc
                            select i).SingleOrDefault();
                ViewData["VehicleStop"] = new SelectList(db.RoutePaths, "VehicleStop", "VehicleStop",stop.VehicleStop);
                db.Employees.Add(e);
                db.SaveChanges();
                
                var id = e.EmployeeId;
                var loc = e.EmpLocation;
                var route = (from i in db.RoutePaths
                             where i.VehicleStop == loc
                             select i).SingleOrDefault();
                Allocation a = new();
                {
                    a.EmployeeId = id;
                    a.RouteNumber = route.RouteNumber;
                    a.VehicleNumber = route.VehicleNumber;
                }
                
                db.Allocations.Add(a);
                db.SaveChanges();
                Vehicle v = new();
                v = db.Vehicles.Find(a.VehicleNumber);
                if ((v.AvailableSeats - 1) > 0)
                {
                    v.AvailableSeats = v.AvailableSeats - 1;
                    db.Vehicles.Update(v);
                    db.SaveChanges();
                }
                else
                {
                    v.Operable = false;
                    db.Update(v);
                    db.SaveChanges();
                }
            }
            else
            {
                ViewBag.err = "Employee Id already exists!";
                return View(e);
            }
            
            return RedirectToAction("Index", "Employees");
        }
        public IActionResult Details(int id)
        {
            Employee e = db.Employees.Find(id);
            return View(e);
        }
        public IActionResult Edit(int id)
        {
            Employee E = db.Employees.Find(id);
            
            return View(E);
        }
        [HttpPost]
        public IActionResult Edit(Employee e)
        {
            db.Employees.Update(e);
            db.SaveChanges();
            return RedirectToAction("Index", "Employees");
        }
        public IActionResult Delete(int id)
        {
            Employee e = db.Employees.Find(id);
            return View(e);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            Employee e = db.Employees.Find(id);
            Allocation a = (from i in db.Allocations
                            where i.EmployeeId == id
                            select i).SingleOrDefault();
            Vehicle v = new();
            v = db.Vehicles.Find(a.VehicleNumber);
            if ((v.AvailableSeats + 1) > 0)
            {
                v.AvailableSeats = v.AvailableSeats + 1;
                db.Vehicles.Update(v);
                db.SaveChanges();
            }
            if (a != null)
            { db.Allocations.Remove(a); }
            db.Employees.Remove(e);
            db.SaveChanges();
            return RedirectToAction("Index", "Employees");
        }
    }
}
