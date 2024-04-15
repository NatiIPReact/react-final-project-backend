using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        // GET: api/<MessagesController>
        // Returns all messages
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(Models.Message.GetMessages());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }

        // GET api/<MessagesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // Adds a new message.
        // POST api/<MessagesController>
        [HttpPost]
        public IActionResult Post(Models.Message m)
        {
            try
            {
                return Ok(m.Insert());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }

        // PUT api/<MessagesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MessagesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
