using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomManagement.Control;
using RoomManagement.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RoomManagement.Controllers
{
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> Login(Login login)
        {
            ResponseAPI<ResponseLogin> _response = API.Execute<ResponseLogin>("http://localhost:56011/api/login", null, null, login, RestSharp.Method.POST);

            if (_response.Object.authenticated)
            {
                var _claims = new List<Claim>() {
                            new Claim("Root", "root"),
                            new Claim(ClaimTypes.Name, _response.Object.user.Name),
                            new Claim("IdUser", _response.Object.user.IdUser.ToString()),
                            new Claim(ClaimTypes.Email, _response.Object.user.Email),
                            new Claim("AccessToken", _response.Object.accessToken)
                        };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(_claims, "login");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await _httpContextAcessor.HttpContext.SignInAsync(
                        claimsPrincipal,
                        new AuthenticationProperties()
                        {
                            IsPersistent = true
                        });
            }
            return Json(_response.Object);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
