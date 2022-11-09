using Microsoft.AspNetCore.Mvc;
using TmsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TmsApp.Controllers
{
    [Authorize]
    public class VehiclesController : Controller
    {
        private readonly TMSContext db;

        public VehiclesController(TMSContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            
            List<Vehicle> VList = db.Vehicles.ToList();
            return View(VList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Vehicle v)
        {
            
            if (db.Vehicles.Find(v.VehicleNumber) == null)
            {
                db.Vehicles.Add(v);
                db.SaveChanges();
            }
            else
            {
                ViewBag.verr = "Vehicle already exists!";
                return View(v);
            }
            return RedirectToAction("Index", "Vehicles");
        }
        public IActionResult Details(int id)
        {
            Vehicle v = db.Vehicles.Find(id);
            return View(v);
        }
        public IActionResult Edit(int id)
        {
            Vehicle v = db.Vehicles.Find(id);
            return View(v);
        }
        [HttpPost]
        public IActionResult Edit(Vehicle v)
        {
            db.Vehicles.Update(v);
            db.SaveChanges();
            return RedirectToAction("Index","Vehicles");
        }
        public IActionResult Delete(int id)
        {
            Vehicle v = db.Vehicles.Find(id);
            return View(v);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            Vehicle v = db.Vehicles.Find(id);
            var vt = (from i in db.RoutePaths
                      where i.VehicleNumber == v.VehicleNumber
                      select i).ToList();
            if (vt.Count <= 0)
            {
                db.Vehicles.Remove(v);
                db.SaveChanges();
                return RedirectToAction("Index", "Vehicles");
            }
            else
            {
                ViewBag.delerrv = "Allocated route cannot be deleted";
                return View(v);
            }
        }
    }
}
