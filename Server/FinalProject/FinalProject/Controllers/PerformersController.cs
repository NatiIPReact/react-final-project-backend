using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformersController : ControllerBase
    {
        // GET: api/<PerformersController>
        // Returns all performers
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(Performer.GetAllPerformers());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }

        // GET api/<PerformersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PerformersController>
        // Adds a new performer
        [HttpPost]
        public IActionResult Post(Performer p)
        {
            try
            {
                return p.Insert() ? Ok(new {message = "Success"}) : BadRequest(new {message = "ERROR"});
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }

        // PUT api/<PerformersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PerformersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        // Gets total streams of a specific performer
        [HttpGet("GetTotalStreamsOfPerformer/PerformerID/{PerformerID}")]
        public IActionResult GetTotalStreamsOfPerformer(int PerformerID)
        {
            try
            {
                return Ok(Performer.GetTotalStreams(PerformerID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }

        // Gets total number a user has added a song of a specific performer to his favorites
        [HttpGet("GetTotalFavoritesOfPerformer/PerformerID/{PerformerID}")]
        public IActionResult GetTotalFavoritesOfPerformer(int PerformerID)
        {
            try
            {
                return Ok(Performer.GetTotalFavorites(PerformerID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        // Gets total followers of a specific performer
        [HttpGet("GetTotalFollowersOfPerformer/PerformerID/{PerformerID}")]
        public IActionResult GetTotalFollowersOfPerformer(int PerformerID)
        {
            try
            {
                return Ok(Performer.GetTotalFollowers(PerformerID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        // Gets the instagram of a specific performer
        [HttpGet("GetPerformerInstagram/PerformerID/{PerformerID}")]
        public IActionResult GetPerformerInstagram(int PerformerID)
        {
            try
            {
                return Ok(Performer.GetInstagram(PerformerID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }
        // Returns the performers data for the admin's report
        [HttpGet("AdminGetPerformersData")]
        public IActionResult AdminGetPerformersData()
        {
            try
            {
                return Ok(Performer.AdminGetPerformersData());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        // Inserts artist by his name, and gets the image from serpapi API (Google Images). Used only on swagger.
        [HttpGet("InsertArtistWithImageAPI/artistName/{artistName}")]
        public async Task<IActionResult> InsertArtistWithImageAPI(string artistName)
        {
            try
            {
                string b = await Performer.InsertArtistWithAPIImage(artistName);
                return b != null ? Ok(b) : BadRequest(new { message = "ERROR" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        // Gets featured artists by # of plays
        [HttpGet("GetFeaturedArtists")]
        public IActionResult GetFeaturedArtists()
        {
            try
            {
                return Ok(Performer.GetFeaturedArtists());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        // Gets all artists for artist.html page
        [HttpGet("GetArtists")]
        public IActionResult GetArtists()
        {
            try
            {
                return Ok(Performer.GetArtists());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
    }
}
