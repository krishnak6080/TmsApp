using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TmsApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Addnew()
        {
            return View();
        }
        
        public IActionResult Modify() { return View(); }

        public IActionResult Status()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
