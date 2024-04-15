using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        // GET: api/<CommentsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CommentsController>/5
        // Returns the comments of a specific artist by its id
        [HttpGet("GetArtistsComments/PerformerID/{PerformerID}")]
        public IActionResult GetArtistsComments(int PerformerID)
        {
            try
            {
                return Ok(Models.Comment.GetArtistsComments(PerformerID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }

        // POST api/<CommentsController>
        // adds a comment to the artist page. by artist id and user id (to know who posted the comment)
        [HttpPost]
        public IActionResult Post(Models.Comment CommentToPost)
        {
            try
            {
                return CommentToPost.Insert() ? Ok(new { message = "Success" }) : BadRequest(new { message = "SERVER ERROR " });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }

        // PUT api/<CommentsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CommentsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
