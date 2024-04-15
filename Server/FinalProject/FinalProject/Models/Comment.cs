namespace FinalProject.Models
{
    public class Comment
    {
        private int commentID;
        private int userID;
        private int performerID;
        private string content;
        private DateTime date;
        private string userName;

        public Comment(int commentID, int userID, int performerID, string content)
        {
            CommentID = commentID;
            UserID = userID;
            PerformerID = performerID;
            Content = content;
        }
        public Comment()
        {

        }

        public Comment(int commentID, int userID, int performerID, string content, DateTime date, string userName)
        {
            CommentID = commentID;
            UserID = userID;
            PerformerID = performerID;
            Content = content;
            Date = date;
            UserName = userName;
        }

        public int CommentID { get => commentID; set => commentID = value; }
        public int UserID { get => userID; set => userID = value; }
        public int PerformerID { get => performerID; set => performerID = value; }
        public string Content { get => content; set => content = value; }
        public DateTime Date { get => date; set => date = value; }
        public string UserName { get => userName; set => userName = value; }
        // Inserts a comment to our db
        public bool Insert()
        {
            Validate();
            DBservices db = new DBservices();
            return db.Insert(this) > 0;
        }
        // Checks the data is valid
        private void Validate()
        {
            if (userID < 1)
                throw new ArgumentException("User doesn't exist!");
            if (performerID < 1)
                throw new ArgumentException("Artist doesn't exist!");
        }
        // Gets artist comments by his id.
        public static List<Comment> GetArtistsComments(int PerformerID)
        {
            if (PerformerID < 1)
                throw new ArgumentException("User doesn't exist!");
            DBservices db = new DBservices();
            return db.ReadComments(PerformerID);
        }
    }
}
