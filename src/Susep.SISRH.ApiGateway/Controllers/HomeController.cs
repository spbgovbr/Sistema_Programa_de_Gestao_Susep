using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Susep.SISRH.ApiGateway.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : Controller
    {
        [HttpGet()]
        public string Get()
        {
            return "Bem-vindo ao API Gateway do sistema de SISRH.";
        }
    }
}
