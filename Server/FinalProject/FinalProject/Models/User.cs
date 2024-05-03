using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;
using Twilio;
using Twilio.Rest.Verify.V2.Service;
using System.Numerics;

namespace FinalProject.Models
{
    public class User
    {
        private int id;
        private string email;
        private string name;
        private string password;
        private bool isVerified;
        private DateTime registrationDate;
        private bool isBanned;
        private string image;

        public User(int id, string email, string name, string password, bool isVerified, DateTime registrationDate)
        {
            Id = id;
            Email = email;
            Name = name;
            Password = password;
            IsVerified = isVerified;
            RegistrationDate = registrationDate;
        }

        public User(int id, string email, string name, string password, bool isVerified, DateTime registrationDate, bool ib, string image)
        {
            Id = id;
            Email = email;
            Name = name;
            Password = password;
            IsVerified = isVerified;
            RegistrationDate = registrationDate;
            IsBanned = ib;
            Image = image;
        }

        public User()
        {

        }

        public int Id { get => id; set => id = value; }
        public string Email { get => email; set => email = value; }
        public string Name { get => name; set => name = value; }
        public string Password { get => password; set => password = value; }
        public bool IsVerified { get => isVerified; set => isVerified = value; }
        public DateTime RegistrationDate { get => registrationDate; set => registrationDate = value; }
        public bool IsBanned { get => isBanned; set => isBanned = value; }
        public string Image { get => image; set => image = value; }

        // Used for user login
        public static User Login(string email, string password)
        {
            if (email == null || password == null || email == "" || password == "")
                throw new Exception("User not found");
            if (email == "admin@gmail.com")
            {
                if (password == "123")
                {
                    User admin = new User();
                    admin.email = email;
                    return admin;
                }
                else throw new ArgumentException("Wrong admin password!");
            }
            DBservices db = new DBservices();
            User user = db.Login(email, password);
            if (user == null)
                throw new Exception("Error");
            return user;
        }
        public static List<object> GetExpoTokens()
        {
            DBservices db = new DBservices();
            return db.GetExpoTokens();
        }
        public static bool UpdateUserExpoToken(int UserID, string NewExpoToken)
        {
            DBservices db = new DBservices();
            return db.UpdateExpoToken(UserID, NewExpoToken) > 0;
        }
        public static User GetUserByEmail(string email)
        {
            DBservices db = new DBservices();
            return db.GetUserByEmail(email);
        }
        public static List<object> GetUserSongHistory(int UserID)
        {
            DBservices db = new DBservices();
            return db.GetUserSongHistory(UserID);
        }
        public static User GetUserByPhone(string phone)
        {
            DBservices db = new DBservices();
            return db.GetUserByPhone(phone);
        }

        // Inserts a new user into the user table. Initiates email verification.
        public bool Insert()
        {
            //Execute().Wait();
            //throw new ArgumentException(GenerateToken());
            //return true;
            Validate();
            IsVerified = false;
            DBservices db = new DBservices();
            string Token = GenerateToken();
            bool tmp = db.Insert(this, Token) > 0;
            Execute(Token).Wait(); // email verification
            return tmp;
        }

        // User details update, takes the old email as an argument to check whether it's changed. If so, require verification again.
        public bool Update(string oldEmail)
        {
            Validate();
            if (!oldEmail.Equals(Email))
                ValidateEmail(oldEmail);
            DBservices db = new DBservices();
            bool isNewEmail = email != oldEmail;
            string Token = isNewEmail ? GenerateToken() : "";
            bool tmp = db.Update(this, isNewEmail, Token) > 0;
            if (isNewEmail)
                Execute(Token).Wait();
            return tmp;
        }
        public static VerificationResource.StatusEnum SendCode(int UserID)
        {
            DBservices db = new DBservices();
            return db.SendCode(UserID);
        }
        public static bool UpdateUserPhoneNumber(int UserID, string PhoneNumber)
        {
            DBservices db = new DBservices();
            return db.PutUserPhone(UserID, PhoneNumber) > 0;
        }
        public static bool PostUserRecentlyPlayed(int UserID, int SongID)
        {
            DBservices db = new DBservices();
            return db.PostUserRecentlyPlayed(UserID, SongID) > 0;
        }
        public static object GetUserPhoneNumber(int UserID)
        {
            DBservices db = new DBservices();
            return db.GetUserPhoneNumber(UserID);
        }
        // Returns user's following list. (performers he follows)
        public static List<Performer> GetFollowingList(int UserID)
        {
            if (UserID < 1)
                throw new ArgumentException("User doesn't exist");
            DBservices db = new DBservices();
            return db.GetUserFollowingList(UserID);
        }
        // returns true if this user is verified
        public static bool IsUserVerified(int id)
        {
            if (id < 1)
                throw new ArgumentException("User doesn't exist");
            DBservices db = new DBservices();
            return db.IsUserVerified(id);
        }
        // Validates that the user info is correct. Throws an exception otherwise.
        private void Validate()
        {
            if (this == null)
                throw new Exception("Server error, try again later");
            if (name == "" || name == null)
                throw new ArgumentException("Please insert your name");
            if (password == "" || password == null)
                throw new ArgumentException("Please insert your password");
            if (password.Length < 3)
                throw new ArgumentException("Your password must contain atleast 3 characters");
            ValidateEmail(email);
            if (name.Any(char.IsDigit))
                throw new ArgumentException("Your name cannot contain numbers");
        }
        // Get an email, throws an exception if its format is incorrect.
        private static void ValidateEmail(string email)
        {
            if (email == "" || email == null)
                throw new ArgumentException("Please insert your email");
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, emailPattern))
                throw new ArgumentException("Please insert correct email");
        }
        // Sends email verification request.
        async Task Execute(string Token)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("csbgroup22@gmail.com", "Ruppin Music");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress(Email, "User: " + Name);
            var templateId = "d-e7de34c1548845ca862654c5db151472";

            string dynamicName = name;
            if (name.Contains(" "))
                dynamicName = name.Split(' ')[0];

            var dynamicData = new Dictionary<string, dynamic>
            {
              { "name", dynamicName },
              { "token", Token },
              { "email", email }
               // Add other dynamic data fields and their values as needed
            };

            var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, dynamicTemplateData: dynamicData);
            msg.Subject = subject;

            var response = await client.SendEmailAsync(msg);
        }

        // User verified himself using the email sent. Check the credentials and verify. 
        public static bool ValidateUser(string email, string token)
        {
            // Don't forget to change the link of the verification and hear buttons in SendGrid whe you upload the project to Ruppin's server
            DBservices db = new DBservices();
            return db.ValidateUser(email, token) > 0;
        }
        // TEMP - tests, used to search tracks with the Spotify API.
        static async Task<string> SearchTracks(string accessToken)
        {
            // Set the search query
            string searchQuery = "7 Rings";

            // Set the API endpoint for track search
            string searchEndpoint = $"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(searchQuery)}&type=track";

            // Create the HTTP client
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Send the request to search for tracks
            var response = await httpClient.GetAsync(searchEndpoint);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Extract the response content
                var responseContent = await response.Content.ReadAsStringAsync();

                // TODO: Process the response data as per your requirements
                Console.WriteLine("Search Results:");
                Console.WriteLine(responseContent);
                return responseContent;
            }
            else
            {
                // Request failed
                Console.WriteLine("Request failed with status code: " + response.StatusCode);
                return "ERROR";
            }
        }

        // Generates a random token for an email verification
        private static string GenerateToken()
        {
            int length = 16;
            // Create a random number generator.
            var rng = new Random();

            // Create a list of possible characters.
            var characters = new string[]
            {
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
        "abcdefghijklmnopqrstuvwxyz",
        "0123456789",
            };

            // Create a string builder to hold the token.
            var sb = new StringBuilder(length);

            // Loop over the characters and randomly select one for each position in the token.
            for (int i = 0; i < length; i++)
            {
                // Use the same Random object to generate the random index and select the character.
                int charSetIndex = rng.Next(characters.Length);
                string charSet = characters[charSetIndex];
                sb.Append(charSet[rng.Next(charSet.Length)]);
            }

            // Return the token.
            return sb.ToString();
        }
        // Sends an email to the user, to verify himself.
        // Don't forget to update your api key
        public bool InitiateNewValidation()
        {
            if (Id < 1)
                throw new ArgumentException("User doesn't exist");
            DBservices db = new DBservices();
            string Token = GenerateToken();
            bool tmp = db.InitiateNewValidation(Id, Token) > 0;
            Execute(Token).Wait();
            return tmp;
        }
        // Gets user favorites songs. returns List of json objects (needed because special data is used)
        public static List<object> GetUserFavorites(int UserID)
        {
            DBservices db = new DBservices();
            return db.GetUserFavorites(UserID);
        }
        public static object GetTotalFavorites(int UserID)
        {
            DBservices db = new DBservices();
            return db.GetNumberOfLikedSongs(UserID);
        }
        public static object GetNumberOfRecentlyPlayed(int UserID)
        {
            DBservices db = new DBservices();
            return db.GetNumberOfRecentlyPlayed(UserID);
        }
        // Posts a new song to user favorites
        public static object PostUserFavorite(int UserID, int SongID)
        {
            if (UserID < 1)
                throw new ArgumentException("User doesn't exist");
            if (SongID < 1)
                throw new ArgumentException("Song doesn't exist");
            DBservices db = new DBservices();
            return db.PostUserFavorite(UserID, SongID);
        }
        // Deletes a song from user favorites.
        public static bool DeleteFromFavorite(int UserID, int SongID)
        {
            if (UserID < 1)
                throw new ArgumentException("User doesn't exist");
            if (SongID < 1)
                throw new ArgumentException("Song doesn't exist");
            DBservices db = new DBservices();
            return db.DeleteFromFavorites(UserID, SongID) > 0;
        }
        // Returns user information for the admin's report.
        // For security reasons, some information is not returned. Such as passwords.
        public static List<User> LoadUserInformation()
        {
            DBservices db = new DBservices();
            return db.LoadUserInformation();
        }
        // Builds the general admin report.
        public static object GetAdminReport()
        {
            DBservices db = new DBservices();
            return db.BuildReport();
        }
        // Used for the user to follow an artist.
        public static bool FollowArtist(int UserID, int PerformerID)
        {
            if (UserID < 1)
                throw new ArgumentException("User doesn't exist!");
            if (PerformerID < 1)
                throw new ArgumentException("Performer doesn't exist!");
            DBservices db = new DBservices();
            return db.FollowArtist(UserID, PerformerID) >= 0;
        }
        // Used when user wants to unfollow an artist.
        public static bool UnfollowArtist(int UserID, int PerformerID)
        {
            if (UserID < 1)
                throw new ArgumentException("User doesn't exist!");
            if (PerformerID < 1)
                throw new ArgumentException("Performer doesn't exist!");
            DBservices db = new DBservices();
            return db.UnfollowArtist(UserID, PerformerID) >= 0;
        }
        // Gets the registration date of the user as json object. There's no need to return a whole User object because we
        // only use the date field.
        public static object GetRegistrationDate(int UserID)
        {
            if (UserID < 1)
                throw new ArgumentException("User doesn't exist!");
            DBservices db = new DBservices();
            return db.GetUserRegistarationDate(UserID);
        }
        // Gets the XP of the user. There's no need to return a whole User object because we only use the XP field.
        public static object GetXP(int UserID)
        {
            if (UserID < 1)
                throw new ArgumentException("User doesn't exist!");
            DBservices db = new DBservices();
            return db.GetUserXP(UserID);
        }
        // Adds XP to the UserID it recieved.
        public static bool AddUserXP(int UserID, int XP)
        {
            if (UserID < 1)
                throw new ArgumentException("User doesn't exist!");
            DBservices db = new DBservices();
            return db.AddUserXP(UserID, XP) > 0;
        }
        // Gets user data on the leaderboard. Returns List of objects because we don't use many of the User class's fields,
        // and we'd also like to hide some of them for security reasons. (such as passwords!)
        public static List<object> GetLeaderboard()
        {
            DBservices db = new DBservices();
            return db.GetLeaderboard();
        }

        // Used for admins to ban users
        public static bool BanUser(int UserID)
        {
            if (UserID < 1)
                throw new ArgumentException("User doesn't exist");
            DBservices db = new DBservices();
            return db.BanUser(UserID) > 0;
        }

        // Used for admins to unban users
        public static bool UnbanUser(int UserID)
        {
            if (UserID < 1)
                throw new ArgumentException("User doesn't exist");
            DBservices db = new DBservices();
            return db.UnbanUser(UserID) > 0;
        }

        public static object PhoneLogin(string phone)
        {
            DBservices db = new DBservices();
            return db.PhoneLogin(phone);
        }

        public static object VerifyCode(string phone, string code)
        {
            DBservices db = new DBservices();
            return db.VerifyCode(phone, code);
        }
    }
}