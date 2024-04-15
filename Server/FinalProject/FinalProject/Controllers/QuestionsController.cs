using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        // GET: api/<QuestionsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<QuestionsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<QuestionsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // Updates user answer to the question (solo quiz)
        // PUT api/<QuestionsController>/5
        [HttpPut("UpdateUserAnswer/QuestionID/{QuestionID}/Answer/{Answer}")]
        public IActionResult Put(int QuestionID, int Answer)
        {
            try
            {
                return Ok(Question.PutUserAnswer(QuestionID, Answer));
            }
            catch (Exception e)
            {
                return BadRequest("ERROR " + e.Message);
            }
        }

        // DELETE api/<QuestionsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
