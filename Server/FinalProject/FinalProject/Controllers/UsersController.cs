﻿using Microsoft.AspNetCore.Mvc;
using FinalProject.Models;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Runtime.CompilerServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            // return Models.User.Zibi();
            return new string[] { "value1", "value2" };
        }
        
        // GET api/<UsersController>/5
        // Returns whether the user is verified or not.
        [HttpGet("IsUserVerified/id/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(Models.User.IsUserVerified(id));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        // Returns user's following list. (performers he follows)
        [HttpGet("GetUserFollowingList/UserID/{UserID}")]
        public IActionResult GetUserFollowingList(int UserID)
        {
            try
            {
                return Ok(Models.User.GetFollowingList(UserID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }
        // Gets user favorites songs.
        [HttpGet("GetUserFavorites/UserID/{UserID}")]
        public IActionResult GetUserFavorites(int UserID)
        {
            try
            {
                return Ok(Models.User.GetUserFavorites(UserID));
            }
            catch (Exception e)
            {
                return BadRequest(new {message = "Server error " + e.Message });
            }
        }
        [HttpGet("GetNumberOfLikedSongs/UserID/{UserID}")]
        public IActionResult GetNumberOfLikedSongs(int UserID)
        {
            try
            {
                return Ok(Models.User.GetTotalFavorites(UserID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        [HttpGet("GetNumberOfRecentlyPlayed/UserID/{UserID}")]
        public IActionResult GetNumberOfRecentlyPlayed(int UserID)
        {
            try
            {
                return Ok(Models.User.GetNumberOfRecentlyPlayed(UserID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        // Returns user information for the admin report.
        [HttpGet("LoadUserInformation")]
        public IActionResult LoadUserInformation()
        {
            try
            {
                return Ok(Models.User.LoadUserInformation());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        // Generates the general admin report and returns the data.
        [HttpGet("GetAdminReport")]
        public IActionResult GetAdminReport()
        {
            try
            {
                return Ok(Models.User.GetAdminReport());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        // Gets user registration date by his id.
        [HttpGet("GetUserRegistrationDate/UserID/{UserID}")]
        public IActionResult GetUserRegistrationDate(int UserID)
        {
            try
            {
                return Ok(Models.User.GetRegistrationDate(UserID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        // Gets user XP by his id.
        [HttpGet("GetUserXP/UserID/{UserID}")]
        public IActionResult GetUserXP(int UserID)
        {
            try
            {
                return Ok(Models.User.GetXP(UserID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        [HttpGet("GetExpoTokens")]
        public IActionResult GetExpoTokens()
        {
            try
            {
                return Ok(Models.User.GetExpoTokens());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        // POST api/<UsersController>
        [HttpPost]
        // Posts a new user into the user table - Register
        public IActionResult Post([FromBody] User u)
        {
            try
            {
                return u.Insert() ? Ok(new { message = "Registered!" }) : BadRequest(new { message = "ERROR!" });
            }
            catch (Exception e)
            {
                if (e.Message.Contains("UNIQUE KEY constraint")) // If the email is taken
                    return BadRequest(new { message = "This email is already taken" });
                return BadRequest(new { message = e.Message });
            }
        } 
        // User Login to our site.
        [HttpPost("Login")]
        public IActionResult Post(string email, string password)
        {
            try
            {
                return Ok(Models.User.Login(email, password));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpPost("SendCode/UserID/{UserID}")]
        public IActionResult SendCode(int UserID)
        {
            try
            {
                return Ok(Models.User.SendCode(UserID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpPost("PhoneLogin/Phone/{Phone}")]
        public IActionResult PhoneLogin(string Phone)
        {
            try
            {
                if (!Phone.Contains('+'))
                    Phone = "+" + Phone;
                return Ok(Models.User.PhoneLogin(Phone));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpGet("VerifyCode/Phone/{Phone}/Code/{Code}")]
        public IActionResult VerifyCode(string Phone, string Code)
        {
            try
            {
                if (!Phone.Contains('+'))
                    Phone = "+" + Phone;
                return Ok(Models.User.VerifyCode(Phone, Code));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpGet("GetUserByEmail/email/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            try
            {
                return Ok(Models.User.GetUserByEmail(email));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        [HttpGet("GetUserByPhone/Phone/{Phone}")]
        public IActionResult GetUserByPhone(string Phone)
        {
            try
            {
                if (!Phone.Contains("+"))
                {
                    Phone = "+" + Phone;
                }
                return Ok(Models.User.GetUserByPhone(Phone));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Server error " + e.Message });
            }
        }
        // Adds a song to user favorites.
        [HttpPost("PostUserFavorite/UserID/{UserID}/SongID/{SongID}")]
        public IActionResult PostUserFavorite(int UserID, int SongID)
        {
            try
            {
                return Ok(Models.User.PostUserFavorite(UserID, SongID));
            }
            catch (Exception e)
            {
                if (e.Message.Contains("PRIMARY KEY"))
                    return Ok(false);
                if (e.Message.Contains("FOREIGN KEY"))
                {
                    if (e.Message.Contains("UserID"))
                        return BadRequest(new { message = "This user doesn't exist" });
                    if (e.Message.Contains("SongID"))
                        return BadRequest(new { message = "This song doesn't exist" });
                }
                return BadRequest(new { message = e.Message });
            }
        }
        // Adds a follower to the user following list.
        [HttpPost("FollowArtist/UserID/{UserID}/PerformerID/{PerformerID}")]
        public IActionResult FollowArtist(int UserID, int PerformerID)
        {
            try
            {
                return Ok(Models.User.FollowArtist(UserID, PerformerID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }
        // Deletes a follower from the following list of the user.
        [HttpDelete("UnfollowArtist/UserID/{UserID}/PerformerID/{PerformerID}")]
        public IActionResult UnfollowArtist(int UserID, int PerformerID)
        {
            try
            {
                return Ok(Models.User.UnfollowArtist(UserID, PerformerID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }
        // Deletes a song from user favorites.
        [HttpDelete("DeleteUserFavorite/UserID/{UserID}/SongID/{SongID}")]
        public IActionResult DeleteUserFavorite(int UserID, int SongID)
        {
            try
            {
                return Ok(Models.User.DeleteFromFavorite(UserID, SongID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        // User update details
        // PUT api/<UsersController>/5
        [HttpPut("UpdateUserDetails")]
        public IActionResult Put(User u, string oldEmail)
        {
            try
            {
                return u.Update(oldEmail) ? Ok(new {message = "Updated"}) : BadRequest(new { message = "Server error, please try again later" });
            }
            catch (Exception e)
            {
                if (e.Message.Contains("UNIQUE KEY constraint"))
                    return BadRequest(new { message = "This email is already taken" });
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpPut("UpdateUserPositionInSong/UserID/{UserID}/SongID/{SongID}/Position/{Position}")]
        public IActionResult UpdateUserPositionInSong(int UserID, int SongID, int Position)
        {
            try
            {
                return Models.User.UpdateUserPositionInSong(UserID, SongID, Position) ? Ok(new { message = "Updated" }) : BadRequest(new { message = "Server error, please try again later" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpPut("UpdateUserPhone/UserID/{UserID}/Phone/{Phone}")]
        public IActionResult Put(int UserID, string Phone)
        {
            try
            {
                if (Phone == "-1")
                    return Ok(new { message = "ERROR: Couldn't Update!" });
                if (!Phone.Contains('+'))
                    Phone = "+" + Phone;
                return Models.User.UpdateUserPhoneNumber(UserID, Phone) ? Ok(new { message = "Updated" }) : BadRequest(new { message = "Server error, please try again later" });
            }
            catch (Exception e)
            {
                if (e.Message.Contains("duplicate"))
                    return BadRequest(new { message = "This phone is already taken" });
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpPost("PostUserRecentlyPlayed/UserID/{UserID}/SongID/{SongID}")]
        public IActionResult PostUserRecentlyPlayed(int UserID, int SongID)
        {
            try
            {
                return Models.User.PostUserRecentlyPlayed(UserID, SongID) ? Ok(new { message = "Updated" }) : BadRequest(new { message = "Server error, please try again later" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpPut("UpdateUserExpoToken/UserID/{UserID}/ExpoToken/{ExpoToken}")]
        public IActionResult UpdateUserExpoToken(int UserID, string ExpoToken)
        {
            try
            {
                return Models.User.UpdateUserExpoToken(UserID, ExpoToken) ? Ok(new { message = "Updated" }) : BadRequest(new { message = "Server error, please try again later" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        // Used for admins to ban users
        [HttpPut("BanUser")]
        public IActionResult BanUser(int UserID)
        {
            try
            {
                return Models.User.BanUser(UserID) ? Ok(new { message = "Banned" }) : BadRequest(new { message = "Server error, please try again later" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        // Used for admins to unban users
        [HttpPut("UnbanUser")]
        public IActionResult UnbanUser(int UserID)
        {
            try
            {
                return Models.User.UnbanUser(UserID) ? Ok(new { message = "Banned" }) : BadRequest(new { message = "Server error, please try again later" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        // Adds XP to the user. Used when getting questions right on quizzes.
        [HttpPut("AddUserXP/UserID/{UserID}/XP/{XP}")]
        public IActionResult Put(int UserID, int XP)
        {
            try
            {
                return Models.User.AddUserXP(UserID, XP) ? Ok(new { message = "Added" }) : BadRequest(new { message = "Server error, please try again later" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }
        // Gets the users on the leaderboard.
        [HttpGet("GetLeaderboard")]
        public IActionResult GetLeaderboard()
        {
            try
            {
                return Ok(Models.User.GetLeaderboard());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }
        [HttpGet("GetLastPositionInSong/UserID/{UserID}/SongID/{SongID}")]
        public IActionResult GetLastPositionInSong(int UserID, int SongID)
        {
            try
            {
                ulong PositionInSong = Models.User.GetLastPositionInSong(UserID, SongID);
                return Ok(new { Position = PositionInSong });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }
        [HttpGet("GetUserSongHistory/UserID/{UserID}")]
        public IActionResult GetUserSongHistory(int UserID)
        {
            try
            {
                return Ok(Models.User.GetUserSongHistory(UserID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpGet("GetUserPhoneNumber/UserID/{UserID}")]
        public IActionResult GetUserPhoneNumber(int UserID)
        {
            try
            {
                return Ok(Models.User.GetUserPhoneNumber(UserID));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "SERVER ERROR " + e.Message });
            }
        }
        // Initiates a new email validation. Needed when the user wants to verify his email, but his old request timed out.
        [HttpPut("InitiateNewValidation")]
        public IActionResult Put(User u)
        {
            try
            {
                return u.InitiateNewValidation() ? Ok(new {message = "Check your email to verify the account"}) : BadRequest(new {message = "Server error, please try again later"});
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpPut("UserForgotPassword")]
        public IActionResult UserForgotPassword(User u)
        {
            try
            {
                return Ok(u.UserForgotPassword());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpGet("VerifyResetPasswordCode/UserEmail/{UserEmail}/Code/{Code}")]
        public IActionResult VerifyResetPasswordCode(string UserEmail, int Code)
        {
            try
            {
                return Ok(Models.User.VerifyCode(UserEmail, Code));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpPut("UpdateUserPasswordByEmail/UserEmail/{UserEmail}/Password/{Password}")]
        public IActionResult UpdateUserPasswordByEmail(string UserEmail, string Password)
        {
            try
            {
                return Models.User.UpdateUserPasswordByEmail(UserEmail, Password) ? Ok(new { message = "Updated!" }) : BadRequest(new { message = "This user doesn't exist!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpPut("InitiateNewValidationIfOldInvalid")]
        public IActionResult InitiateNewValidationIfOldInvalid(User u)
        {
            try
            {
                return u.InitiateNewValidationIfOldInvalid() ? Ok(new { sent = true }) : Ok(new { sent = false });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        // User clicked verify on his email
        // PUT api/<UsersController>/5
        [HttpPut("ValidateEmail")]
        public IActionResult Put(string email, string token)
        {
            try
            {
                // Don't forget to change the link of the verification and hear buttons in SendGrid whe you upload the project to Ruppin's server
                return Models.User.ValidateUser(email, token) ? Ok(new { message = "You're now verified" }) : BadRequest(new { message = "Server error, please try again later" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
