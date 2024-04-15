namespace FinalProject.Models
{
    public class Message
    {
        private int messageID;
        private string subject;
        private string content;
        private DateTime date;
        private int userID;
        private string userName;
        private string userEmail;

        public int MessageID { get => messageID; set => messageID = value; }
        public string Subject { get => subject; set => subject = value; }
        public string Content { get => content; set => content = value; }
        public DateTime Date { get => date; set => date = value; }
        public int UserID { get => userID; set => userID = value; }
        public string UserName { get => userName; set => userName = value; }
        public string UserEmail { get => userEmail; set => userEmail = value; }

        // Returns all messages
        public static List<Message> GetMessages()
        {
            DBservices db = new DBservices();
            return db.GetAllMessages();
        }
        // Inserts the message to our db
        public object Insert()
        {
            if (UserID < 1)
                throw new ArgumentException("User doesn't exist");
            DBservices db = new DBservices();
            return new
            {
                Success = db.Insert(this) > 0
            };
        }
    }
}
