using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        // GET: api/<SongsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SongsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // Gets top 15 songs by # of plays
        [HttpGet("GetTop15")]
        public IActionResult GetTop15(int UserID = -1)
        {
            try
            {
                return Ok(Song.GetTop15Songs(UserID));
            }
            catch(Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        [HttpGet("GetUserRecommendations/UserID/{UserID}")]
        public IActionResult GetUserRecommendations(int UserID)
        {
            try
            {
                return Ok(Song.GetUserRecommendations(UserID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        // Initiates search by query string. UserID is required to know whteher the user has the songs on his favorites.
        // If not logged in, returns -1 and then we know he cannot favorite the songs. (Show popup instead to login first)
        [HttpGet("Search/query/{query}/UserID/{UserID}")]
        public IActionResult Search(string query, int UserID)
        {
            try
            {
                return Ok(Song.Search(query, UserID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        [HttpGet("SearchByQuery/query/{query}/UserID/{UserID}")]
        public IActionResult SearchByQuery(string query, int UserID)
        {
            try
            {
                return Ok(Song.SearchByQuery(query, UserID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        // Gets song lyrics by its id.
        [HttpGet("GetSongLyrics/SongID/{SongID}")]
        public IActionResult GetSongLyrics(int SongID)
        {
            try
            {
                return Ok(Song.GetSongLyrics(SongID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        // Get performer songs by its id. UserID to know whether he has them on his favorites.
        [HttpGet("GetPerformerSongs/PerformerID/{PerformerID}/UserID/{UserID}")]
        public IActionResult GetPerformerSongs(int PerformerID, int UserID)
        {
            try
            {
                return Ok(Song.GetPerformerSongs(PerformerID, UserID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }
        // Gets songs data to generate the admin report on site usage.
        [HttpGet("AdminGetSongsData")]
        public IActionResult AdminGetSongsData()
        {
            try
            {
                return Ok(Song.AdminGetSongsData());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }
        // Gets most played track to feature in index.html
        [HttpGet("GetMostPlayedTrack")]
        public IActionResult GetMostPlayedTrack()
        {
            try
            {
                return Ok(Song.GetMostPlayedTrack());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        // Get songs by their genre.
        [HttpGet("GetGenreSongs/GenreID/{GenreID}")]
        public IActionResult GetGenreSongs(int GenreID)
        {
            try
            {
                return Ok(Song.GetGenreSongs(GenreID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        [HttpGet("GetGenreSongsWithUserData/GenreID/{GenreID}/UserID/{UserID}")]
        public IActionResult GetGenreSongsWithUserData(int GenreID, int UserID)
        {
            try
            {
                return Ok(Song.GetGenreSongsWithUserData(GenreID, UserID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        [HttpGet("GetSongMSDuration/SongID/{SongID}")]
        public IActionResult GetSongMSDuration(int SongID)
        {
            try
            {
                return Ok(new { SongDuration = Song.GetSongLength(SongID) });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        // Posts a song without the actual data file.
        [HttpPost("PostSongDataWithoutFile")]
        public IActionResult PostSongDataWithoutFile(Song s)
        {
            try
            {
                return Ok(s.PostSongDataWithoutFile());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }
        // Post file data to song by its id.
        // POST api/<SongsController>
        [HttpPost("PostFileDataFromJS/SongID/{SongID}")]
        public IActionResult PostFileDataFromJS(int SongID)
        {
            IFormFile file = Request.Form.Files[0];
            try
            {
                bool res = Song.InsertFileDataToSongID(SongID, file);
                return res ? Ok(new { message = "File uploaded successfully" }) : BadRequest(new { message = "Couldn't insert file to SQL db" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }

        // PUT api/<SongsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SongsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        // TEMP UPLOAD SONG, used for testing and uploading song through Swagger.
        [HttpPost("UploadSong")]
        //public IActionResult UploadSong(string SongName, string SongLyrics, DateTime ReleaseDate, int GenreID, IFormFile file)
        public IActionResult UploadSong([FromBody] Song SongToInsert, [FromForm] IFormFile file)
        {
            // return Ok(SongToInsert);
            try
            {
                //Song SongToInsert = new Song(0, SongName, SongLyrics, 0, null, GenreID, ReleaseDate);
                bool res = SongToInsert.Insert(file);
                return res ? Ok(new { message = "File uploaded successfully" }) : BadRequest(new { message = "Couldn't insert file to SQL db" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        // TEMP UPLOAD MP3.
        // Used for testing, inserts mp3 data to song by its id through Swagger.
        [HttpPost("InsertSongHEXData")]
        public IActionResult InsertFileDataToSongID(int SongID, IFormFile file)
        {
            try
            {
                //Song SongToInsert = new Song(0, SongName, SongLyrics, 0, null, GenreID, ReleaseDate);
                bool res = Song.InsertFileDataToSongID(SongID, file);
                return res ? Ok(new { message = "File uploaded successfully" })
                    : BadRequest(new { message = "Couldn't insert file to SQL db" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        // Returns a song file data by its id to play.
        [HttpGet("GetSongByID/SongID/{SongID}")]
        public IActionResult GetSongByID(int SongID)
        {
            try
            {
                //Response.Headers["Content-Disposition"] = $"attachment; filename=\"{name}.mp3\"";
                // Return the audio file as a FileContentResult
                FileContentResult file = Song.ReadSongByID(SongID);
                Response.Headers["Content-Type"] = "audio/mpeg";
                Response.Headers["Access-Control-Allow-Headers"] = "range, accept-encoding";
                Response.Headers["Access-Control-Allow-Origin"] = "*";
                Response.Headers["Age"] = "3999";
                Response.Headers["Alt-Svc"] = "h3=\":443\"; ma=86400";
                Response.Headers["Cache-Control"] = "max-age=14400";
                Response.Headers["Cf-Cache-Status"] = "HIT";
                Response.Headers["Cf-Ray"] = "7e39481e5da77d95-TLV";
                int fileSize = file.FileContents.Length;
                Response.Headers["Content-Length"] = fileSize.ToString();
                Response.Headers["Accept-Ranges"] = "bytes";
                Response.Headers["Content-Range"] = $"bytes 0-{fileSize - 1}/{fileSize}";
                //Response.Headers["Date"] = "Sat, 08 Jul 2023 15:15:16 GMT";
                Response.Headers["Etag"] = "\"cb472-51bd07-4c9e757f2c940\"";
                //Response.Headers["Last-Modified"] = "Mon, 17 Sep 2012 15:22:37 GMT";
                Response.Headers["Nel"] = "{\"success_fraction\":0,\"report_to\":\"cf-nel\",\"max_age\":604800}";
                Response.Headers["Report-To"] = "{\"endpoints\":[{\"url\":\"https://a.nel.cloudflare.com/report/v3?s=IjPaZ4YY5%2BiQZ42KvSZ4q5iPR3%2BXljS69N8lEteBDdjnh0EkMalsQXR%2BtoV41huWqXQT7DAUwNwEcKbrrmnKgg0Oza07zCqSL8OxQIbg68p3JcHGhCcmT6FhAa4TEsOqFqw%3D\"}],\"group\":\"cf-nel\",\"max_age\":604800}";
                Response.Headers["Server"] = "cloudflare";
                Response.Headers["Vary"] = "Accept-Encoding";
                //Response.Headers["MSDuration"] = Song.GetSongLength(SongID).ToString();
                return Song.ReadSongByID(SongID);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
