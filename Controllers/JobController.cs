
using Azure.Core;
using DgpaMapWebApi.Dto;
using DgpaMapWebApi.Dtos;
using DgpaMapWebApi.Interface;
using DgpaMapWebApi.Models;
using DgpaMapWebApi.Parameter;
using DgpaMapWebApi.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;



namespace DgpaMapWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        
        private readonly IJobService _jobService;
        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        //public DgpaContr,
        // GET: api/<DgpaController>
        //取得全部
        [HttpGet]
        public async Task<IActionResult> Get()
        //public IActionResult Get()
        {
            try
            {
                var result = await _jobService.GetAllDataAsync();
                if (result == null)
                {
                    return NotFound("找不到資料");
                }
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest($"錯誤:{ex}");
            }
        }

        //資料總筆數
        [HttpGet("count")]
        public async Task<IActionResult> GetDataCount()
        {
            try
            {
                var result = await _jobService.GetCountAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤:{ex}");
            }
        }

        // GET api/<DgpaController>
        [HttpGet("query")]
        public async Task<IActionResult> GetQuery([FromBody] JobSelectParameter queryParas)
        {
            try
            {
                var result = await _jobService.GetQueryDataAsync(queryParas);
                if (result == null)
                {
                    return NotFound("找不到資料");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤:{ex}");
            }
        }

        [HttpGet("geo")]
        public async Task<IActionResult> GetGeojson()
        {
            try
            {
                var result = await _jobService.GetGeojsonDataAsync();
                if (result == null)
                {
                    return NotFound("找不到資料");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤:{ex}");
            }
        }

        // GET api/<DgpaController>
        [HttpGet("geo/query")]
        public async Task<IActionResult> GetQueryGeojson([FromBody] JobSelectParameter queryParas)
        {
            try
            {
                var result = await _jobService.GetQueryGeojsonDataAsync(queryParas);
                if (result == null)
                {
                    return NotFound("找不到資料");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"錯誤:{ex}");
            }
        }

        //// POST api/<DgpaController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<DgpaController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<DgpaController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

    }
}
