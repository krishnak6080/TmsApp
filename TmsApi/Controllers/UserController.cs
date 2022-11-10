using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TmsApi.Models;


namespace TmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TMSContext _db;
        public UserController(TMSContext db)
        {
            _db = db;
        }

        [HttpGet]
        
        public IActionResult GetUser()
        {
            return Ok(_db.UserData.ToList());
        }

        
    }
}
