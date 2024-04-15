using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace FinalProject.Models
{
    public class Performer
    {
        protected int performerID;
        protected string performerName;
        protected int isABand;
        protected string performerImage;
        protected string instagram;

        public int PerformerID { get => performerID; set => performerID = value; }
        public string PerformerName { get => performerName; set => performerName = value; }
        public int IsABand { get => isABand; set => isABand = value; }
        public string PerformerImage { get => performerImage; set => performerImage = value; }
        public string Instagram { get => instagram; set => instagram = value; }
        // Gets featured artists
        public static List<Performer> GetFeaturedArtists()
        {
            DBservices db = new DBservices();
            return db.GetFeaturedArtists();
        }
        // Get all artists
        public static List<Performer> GetArtists()
        {
            DBservices db = new DBservices();
            return db.GetArtists();
        }
        // gets total streams of an artist
        public static object GetTotalStreams(int PerformerID)
        {
            if (PerformerID < 1)
                throw new ArgumentException("Performer doesn't exist!");
            DBservices db = new DBservices();
            return db.GetTotalStreamsOfArtist(PerformerID);
        }
        // gets total favorites of an artist (# of times one of his songs was favorited by users)
        public static object GetTotalFavorites(int PerformerID)
        {
            if (PerformerID < 1)
                throw new ArgumentException("Performer doesn't exist!");
            DBservices db = new DBservices();
            return db.GetTotalFavoritesOfArtist(PerformerID);
        }
        // Gets total followers of the artist.
        public static object GetTotalFollowers(int PerformerID)
        {
            if (PerformerID < 1)
                throw new ArgumentException("Performer doesn't exist!");
            DBservices db = new DBservices();
            return db.GetTotalFollowersOfPerformer(PerformerID);
        }
        // Gets 3 random performers, cannot include PerformerIDToIgnore. (Used to generate questions for quizzes)
        public static List<string> Get3RandomPerformers(int PerformerIDToIgnore)
        {
            DBservices db = new DBservices();
            return db.Get3RandomArtists(PerformerIDToIgnore);
        }
        // Gets a random band
        public static string GetRandomBand()
        {
            DBservices db = new DBservices();
            return db.GetRandomBand();
        }
        // Gets 3 single artists (not bands)
        public static List<string> Get3RandomSingleArtists()
        {
            DBservices db = new DBservices();
            return db.Get3RandomSingleArtists();
        }
        // Inserts a new performer
        public bool Insert()
        {
            DBservices db = new DBservices();
            return db.AdminInsertPerformer(this) > 0;
        }
        // Gets all performers
        public static List<Performer> GetAllPerformers()
        {
            DBservices db = new DBservices();
            return db.GetAllPerformers();
        }
        // Gets the instagram handle of a performer
        public static object GetInstagram(int PerformerID)
        {
            if (PerformerID < 1)
                throw new ArgumentException("Performer doesn't exist!");
            DBservices db = new DBservices();
            return db.GetPerformerInstagram(PerformerID);
        }
        // Gets performers data for the admin's report.
        public static List<object> AdminGetPerformersData()
        {
            DBservices db = new DBservices();
            return db.AdminGetPerformersData();
        }
        // inserts an artist and gets the image from serpapi API. (Google Images)
        // Used only on swagger. # of requests is limited.
        public async static Task<string> InsertArtistWithAPIImage(string artistName)
        {
            string apiKey = "76b3f536a183539247991ad6bcd171924ea6741a420b32c3e5093893ace3faa6";

            using (HttpClient httpClient = new HttpClient())
            {
                // https://serpapi.com/search.json?engine=google_images&q=Coffee&google_domain=google.com&gl=us&hl=en&api_key=76b3f536a183539247991ad6bcd171924ea6741a420b32c3e5093893ace3faa6
                string url = $"https://serpapi.com/search.json?engine=google_images&q={artistName}&gl=us&hl=en&api_key=76b3f536a183539247991ad6bcd171924ea6741a420b32c3e5093893ace3faa6";
                httpClient.DefaultRequestHeaders.Add("API-Key", apiKey);

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(jsonString);

                string imageUrl = jsonObject["images_results"]?.FirstOrDefault()?["original"].ToString();
                DBservices db = new DBservices();
                if (imageUrl == null)
                    throw new Exception("Server error");
                if (db.InsertArtistAndImageUsingAI(artistName, imageUrl) == 0)
                    throw new Exception("DB error");
                return imageUrl;
            }
        }
    }
}
