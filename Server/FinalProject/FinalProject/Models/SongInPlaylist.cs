namespace FinalProject.Models
{
    public class SongInPlaylist
    {
        // Used to insert a song to playlist. Efficient because we don't use many properties of the regular Song class.
        // (and we don't have playlist id there. Not effienct to add just for this.)
        private int playlistID;
        private int songID;

        public SongInPlaylist(int playlistID, int songID)
        {
            PlaylistID = playlistID;
            SongID = songID;
        }

        public int PlaylistID { get => playlistID; set => playlistID = value; }
        public int SongID { get => songID; set => songID = value; }
    }
}
