using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FinalProject.Models
{
    public class Song
    {
        private int id;
        private string name;
        private string lyrics;
        private int numOfPlays;
        //private FileContentResult fileData;
        private int performerID;
        private int genreID;
        private int releaseYear;
        private string length;

        public Song(int id, string name, string lyrics, int numOfPlays, /*FileContentResult fileData,*/ int genreID, int releaseYear)
        {
            Id = id;
            Name = name;
            Lyrics = lyrics;
            NumOfPlays = numOfPlays;
            //FileData = fileData;
            GenreID = genreID;
            ReleaseYear = releaseYear;
        }
        /*public Song(string name, string lyrics, FileContentResult fileData, int genreID, int releaseYear, int performerID, string length)
        {
            Id = 0;
            Name = name;
            Lyrics = lyrics;
            NumOfPlays = 0;
            FileData = fileData;
            GenreID = genreID;
            ReleaseYear = releaseYear;
            PerformerID = performerID;
            Length = length;
        }*/

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Lyrics { get => lyrics; set => lyrics = value; }
        public int NumOfPlays { get => numOfPlays; set => numOfPlays = value; }
        //public FileContentResult FileData { get => fileData; set => fileData = value; }
        public int GenreID { get => genreID; set => genreID = value; }
        public int ReleaseYear { get => releaseYear; set => releaseYear = value; }
        public int PerformerID { get => performerID; set => performerID = value; }
        public string Length { get => length; set => length = value; }

        // Inserts a song and its mp3 file to our db
        public bool Insert(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file provided");
            // Process the uploaded file
            byte[] fileData;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                fileData = memoryStream.ToArray();
            }
            DBservices db = new DBservices();
            return db.InsertSong(this, fileData) > 0;
        }
        // Reads a song by its id
        public static FileContentResult ReadSongByID(int SongID)
        {
            if (SongID < 1)
                throw new ArgumentException("Song doesn't exist");
            DBservices db = new DBservices();
            return db.ReadSongByID(SongID);
        }
        // Inserts file data (in hex) for this SongID, after translating the file to hex.
        public static bool InsertFileDataToSongID(int SongID, IFormFile file)
        {
            if (SongID < 1)
                throw new ArgumentException("This song doesn't exist");
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file provided");
            // Process the uploaded file
            byte[] fileData;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                fileData = memoryStream.ToArray();
            }
            DBservices db = new DBservices();
            return db.InsertFileDataToSongID(SongID, fileData) > 0;
        }
        public static long GetSongLength(int SongID)
        {
            if (SongID < 1)
                throw new ArgumentException("This song doesn't exist");
            DBservices db = new DBservices();
            return db.GetSongLength(SongID);
        }
        // Gets top 15 songs
        public static List<object> GetTop15Songs(int UserID)
        {
            DBservices db = new DBservices();
            return db.GetTop15(UserID);
        }
        public static List<SongRecommendation> GetUserRecommendations(int UserID)
        {
            DBservices db = new DBservices();
            return db.GetUserRecommendedSongs(UserID);
        }
        // Gets the songs of a specific performer by its id
        public static List<object> GetPerformerSongs(int PID, int UserID)
        {
            DBservices db = new DBservices();
            return db.GetPerformerSongs(PID, UserID);
        }
        // Gets songs data for the admin's report.
        public static List<object> AdminGetSongsData()
        {
            DBservices db = new DBservices();
            return db.AdminGetSongsInfo();
        }
        // Gets song lyrics by its id.
        public static string GetSongLyrics(int SID)
        {
            if (SID < 1)
                throw new ArgumentException("Song doesn't exist");
            DBservices db = new DBservices();
            object res = db.GetSongLyrics(SID);
            string json = JsonSerializer.Serialize(res);
            return json;
        }
        // Gets most played track to feature in index.html. Returns object because we need special data.
        public static object GetMostPlayedTrack()
        {
            DBservices db = new DBservices();
            return db.GetMostPlayedTrack();
        }
        // Gets genre songs by the gerne's id. returns them as object because we need special json objects with special
        // data rather than Song class's properties.
        public static List<object> GetGenreSongs(int GID)
        {
            DBservices db = new DBservices();
            return db.GetGenreSongs(GID);
        }
        // Initiates the search query. object because we a special json with more data.
        public static List<object> Search(string query, int UserID)
        {
            if (query.Length > 100)
                throw new ArgumentException("MAX CHARACTERS: 100");
            DBservices db = new DBservices();
            return db.Search(query, UserID);
        }
        public static List<object> SearchByQuery(string query, int UserID)
        {
            if (query.Length > 100)
                throw new ArgumentException("MAX CHARACTERS: 100");
            DBservices db = new DBservices();
            return db.SearchNative(query, UserID);
        }
        // Gets a random song. As dictionary because we need more data. we can later extract using dic[keyName]
        public static Dictionary<string, object> GetRandomSong()
        {
            DBservices db = new DBservices();
            return db.GetRandomSong();
        }
        // Gets 3 random years (as strings, for answers), without ReleaseYearToIgnore
        public static List<string> Get3RandomReleaseYear(int ReleaseYearToIgnore)
        {
            DBservices db = new DBservices();
            return db.Get3RandomReleaseYear(ReleaseYearToIgnore);
        }
        // Posts song data without the actual file.
        public object PostSongDataWithoutFile()
        {
            DBservices db = new DBservices();
            return new { SongID = db.PostSongDataWithoutFile(this) };
        }
    }
}
