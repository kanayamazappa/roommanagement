using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RoomManagement.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IHttpContextAccessor _httpContextAcessor;
        public BaseController()
        {
            _httpContextAcessor = new HttpContextAccessor();
        }
    }
}
