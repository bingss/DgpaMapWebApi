using DgpaMapWebApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DgpaMapWebApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class HistoryJobController : ControllerBase
    {


        private readonly DgpaDbContext _dgpaDb;

        public HistoryJobController(DgpaDbContext dgpaDb)
        {
            _dgpaDb = dgpaDb;
        }

        // GET: api/<HistoryJobController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<HistoryJobController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<HistoryJobController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<HistoryJobController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<HistoryJobController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
