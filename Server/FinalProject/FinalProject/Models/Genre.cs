namespace FinalProject.Models
{
    public class Genre
    {
        private int genreID;
        private string genreName;

        public Genre(int genreID, string genreName)
        {
            this.genreID = genreID;
            this.genreName = genreName;
        }

        public int GenreID { get => genreID; set => genreID = value; }
        public string GenreName { get => genreName; set => genreName = value; }
        // Gets random 3 genres (used for quizzes). Cannot include genre to ignore.
        public static List<string> Get3RandomGenres(string GenreToIgnore)
        {
            DBservices db = new DBservices();
            return db.Get3RandomGenres(GenreToIgnore);
        }
        // Gets all genres.
        public static List<Genre> GetAllGenres()
        {
            DBservices db = new DBservices();
            return db.GetAllGenres();
        }
        // Gets most played genre.
        public static object GetMostPlayedGenres()
        {
            DBservices db = new DBservices();
            return db.GetGenresByPlaysDesc();
        }
    }
}
