using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using TmsApp.Models;



namespace Tms.Controllers
{
    public class UserController : Controller
    {
        public static List<UserDatum> adminfo = new List<UserDatum>();

        public async Task<IActionResult> Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Home", "Dashboard");


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:7234/api/User/");
                if (Res.IsSuccessStatusCode)
                {
                    var admres = Res.Content.ReadAsStringAsync().Result;
                    adminfo = JsonConvert.DeserializeObject<List<UserDatum>>(admres);
                }
            }
            return View();
            

        }


        [HttpPost]
        public async Task<IActionResult> Login(UserDatum t)
        {
            //var result = (from i in adminfo
            //              where i.Username == t.Username && i.Passcode == t.Passcode
            //              select i
            //              ).SingleOrDefault();
            //if (result != null)
            //{
            //    HttpContext.Session.SetString("Uname", t.Username);
            //    return RedirectToAction("Home", "Dashboard");
            //}


            //if (t.Username == "admin" &&
            //t.Passcode == "Admin")
             var result = (from i in adminfo
                           where i.Username == t.Username && i.Passcode == t.Passcode
                           select i
                          ).SingleOrDefault();
            if (result != null)
            {
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, t.Username),
                   
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {

                    AllowRefresh = true,
                    IsPersistent = true

                    //Persistent cookies will be saved as files in the browser until they
                    //manually deleted. This will cause the cookie to persist even if you close the browser.
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Home", "Dashboard");
            }
            else
            {
                ViewBag.LoginError = "Incorrect Username or Password.Try again!";
                return View();
            }
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
