using Microsoft.AspNetCore.Mvc;

namespace Extra.EventPresences.API.Controllers
{
    [Route("v1/Ping/[action]")]
    [ApiController]
    public class PingController : Controller
    {
        [HttpGet]
        public string Ping()
        {
            return DateTime.UtcNow.ToString();
        }

        //[HttpGet]
        //[ServiceFilter(typeof(BasicAuthenticationAttribute))]
        //public string PingAuth()
        //{
        //    return DateTime.UtcNow.ToString();
        //}
    }
}
