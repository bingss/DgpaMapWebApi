using DgpaMapWebApi.Interface;
using DgpaMapWebApi.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DgpaMapWebApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class UpdateDateController : ControllerBase
    {
        private readonly IUpdateDateService _updateDateService;
        public UpdateDateController(IUpdateDateService updateDateService)
        {
            _updateDateService = updateDateService;
        }

        // GET: api/<UpdateDateController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_updateDateService.GetLastestDate() );
        }

    }
}
