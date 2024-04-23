namespace FinalProject
{
    public class SongRecommendation
    {
        private int songID;
        private string songName;
        private int numOfPlays;
        private int performerID;
        private int releaseYear;
        private int isInFav;
        private int followingArtist;
        private int likesGenre;
        private string songLength;
        private string performerName;
        private string performerImage;
        private string genreName;

        public SongRecommendation(int songID, string songName, int numOfPlays, int performerID, int releaseYear, int inFav, int followingArtist, int likesGenre, string songLength, string performerName, string performerImage, string genreName)
        {
            this.songID = songID;
            this.songName = songName;
            this.numOfPlays = numOfPlays;
            this.performerID = performerID;
            this.releaseYear = releaseYear;
            this.isInFav = inFav;
            this.followingArtist = followingArtist;
            this.likesGenre = likesGenre;
            this.songLength = songLength;
            this.performerName = performerName;
            this.performerImage = performerImage;
            this.genreName = genreName;
        }

        public int SongID { get => songID; set => songID = value; }
        public string SongName { get => songName; set => songName = value; }
        public int NumOfPlays { get => numOfPlays; set => numOfPlays = value; }
        public int PerformerID { get => performerID; set => performerID = value; }
        public int ReleaseYear { get => releaseYear; set => releaseYear = value; }
        public int IsInFav { get => isInFav; set => isInFav = value; }
        public int FollowingArtist { get => followingArtist; set => followingArtist = value; }
        public int LikesGenre { get => likesGenre; set => likesGenre = value; }
        public string SongLength { get => songLength; set => songLength = value; }
        public string PerformerName { get => performerName; set => performerName = value; }
        public string PerformerImage { get => performerImage; set => performerImage = value; }
        public string GenreName { get => genreName; set => genreName = value; }
    }
    public class RecommendationEngine
    {
        private List<SongRecommendation> songs;
        public RecommendationEngine()
        {
            this.songs = new List<SongRecommendation>();
        }
        public void AddSong(SongRecommendation s)
        {
            songs.Add(s);
        }
        public List<SongRecommendation> GetSongs()
        {
            return songs;
        }
        public List<SongRecommendation> TurnonEngine(int numberOfSongs)
        {
            if (songs.Count == 0) return null;
            if (numberOfSongs > songs.Count)
            {
                return songs;
            }
            if (numberOfSongs <= 0)
            {
                numberOfSongs = 1;
            }
            List<SongRecommendation> orderedSongs = songs.OrderByDescending((SongRecommendation song) => CalculateCombinedScore(song)).Take(numberOfSongs).ToList();
            return orderedSongs;
        }
        private double CalculateCombinedScore(SongRecommendation Song)
        {
            int x = 7;
            if (Song.PerformerID == 4)
                x = 8;
            double ReleaseYearWeight = 0.15;
            double NumOfPlaysWeight = 0.2;
            double UserLikesGenreWeight = 0.2;
            double UserFollowsArtistWeight = 0.1;
            double UserLikesSongWeight = 0.35;
            return Song.IsInFav * UserLikesSongWeight + Song.FollowingArtist * UserFollowsArtistWeight
                + Song.LikesGenre * UserLikesGenreWeight
                + CalculateYearScore(Song.ReleaseYear) * ReleaseYearWeight
                + CalculatePlaysScore(Song.NumOfPlays) * NumOfPlaysWeight;
        }
        private double CalculateYearScore(int ReleaseYear)
        {
            int maxReleaseYear = songs.Max(song => song.ReleaseYear);
            int minReleaseYear = songs.Min(song => song.ReleaseYear);
            if (maxReleaseYear - minReleaseYear == 0) return 0;
            return (double)(ReleaseYear - minReleaseYear) / (maxReleaseYear - minReleaseYear);
        }
        private double CalculatePlaysScore(int NumOfPlays)
        {
            int maxPlays = songs.Max(song => song.NumOfPlays);
            int minPlays = songs.Min(song => song.NumOfPlays);
            if (maxPlays - minPlays == 0) return 0;
            return (double)(NumOfPlays - minPlays) / (maxPlays - minPlays);
        }
    }
}
