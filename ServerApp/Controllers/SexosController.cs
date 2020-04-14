using Microsoft.AspNetCore.Mvc;
using ServerApp.Services;

namespace ServerApp.Controllers
{
     [Route("v1/sexos")]
    [ApiController]
    public class SexosController: ControllerBase
    {
        
        private ISexosServices _sexoServices;

        public SexosController(ISexosServices sexoServices)
        {
            _sexoServices = sexoServices;

        }

        [HttpGet]
        public IActionResult GetListUsers()
        {
            var ret = _sexoServices.GetAll();
            return Ok(ret);
        }

    }
}