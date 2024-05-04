namespace FinalProject.Models
{
    public class Playlist
    {
        private int id;
        private string name;
        private int userID;

        public Playlist(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int UserID { get => userID; set => userID = value; }
        // Inserts a new playlist to the user.
        public object Insert()
        {
            if (this == null)
                throw new ArgumentException("ERROR");
            if (name == null || name == "")
                throw new ArgumentException("Enter playlist name!");
            DBservices db = new DBservices();
            return db.Insert(this);
        }
        // Inserts a new song to the playlist. Returns true if succeeded
        public static bool InsertSongToPlaylist(SongInPlaylist s)
        {
            if (s == null)
                throw new ArgumentException("SERVER ERROR");
            DBservices db = new DBservices();
            return db.InsertSongToPlaylist(s) > 0;
        }
        // Gets all of the users playlist (by his id)
        public static List<object> GetUserPlaylists(int UserID)
        {

            if (UserID < 1)
                throw new ArgumentException("User doesn't exist");
            DBservices db = new DBservices();
            return db.GetUserPlaylists(UserID);
        }
        // Delete the whole chosen playlist
        public static bool DeleteUserPlaylist(int UserID, int PlaylistID)
        {
            if (UserID < 1)
                throw new ArgumentException("This user doesn't exist");
            if (PlaylistID < 1)
                throw new ArgumentException("This playlist doesn't exist");
            DBservices db = new DBservices();
            return db.DeleteUserPlaylist(UserID, PlaylistID) > 0;
        }
        // Deletes a song from the playlist.
        public static bool DeleteSongFromPlaylist(int PlaylistID, int SongID)
        {
            if (PlaylistID < 1)
                throw new ArgumentException("This playlist doesn't exist");
            if (SongID < 1)
                throw new ArgumentException("This song doesn't exist");
            DBservices db = new DBservices();
            return db.DeleteSongFromPlaylist(PlaylistID, SongID) > 0;
        }
        public static bool EditPlaylistName(int PlaylistID, string Name)
        {
            if (PlaylistID < 1)
                throw new ArgumentException("This playlist doesn't exist");
            if (Name == null || Name == "")
                throw new ArgumentException("The playlist's name cannot be empty");
            DBservices db = new DBservices();
            return db.EditPlaylistName(PlaylistID, Name) > 0;
        }
        // Gets all the songs in the playlist. object is used because we'd like to return more data. (such as performer name, image, etc..)
        public static List<object> GetPlaylistSongs(int PlaylistID)
        {
            if (PlaylistID < 1)
                throw new ArgumentException("Playlist doesn't exist");
            DBservices db = new DBservices();
            return db.GetPlaylistSongs(PlaylistID);
        }
        // Gets the name of the playlist. As JSON, { PlaylistName: ${name} }
        public static object GetPlaylistName(int PlaylistID)
        {
            if (PlaylistID < 1)
                throw new ArgumentException("Playlist doesn't exist");
            DBservices db = new DBservices();
            return db.GetPlaylistName(PlaylistID);
        }
    }
}
