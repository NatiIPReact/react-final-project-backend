using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        // GET: api/<GenresController>
        // Returns all genres
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(Genre.GetAllGenres());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }
        // Returns genres by plays, for the admin report.
        [HttpGet("AdminGetGenresInformation")]
        public IActionResult AdminGetGenresInformation()
        {
            try
            {
                return Ok(Genre.GetMostPlayedGenres());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }

        // GET api/<GenresController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<GenresController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GenresController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GenresController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
