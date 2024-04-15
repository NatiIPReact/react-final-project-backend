using System;
using System.Net.NetworkInformation;

namespace FinalProject.Models
{
    public class Question
    {
        private int id;
        private string content;
        private List<string> answers;
        private int correctAnswer;
        private int userAnswer;

        public Question(int id, string content, List<string> answers, int correctAnswer)
        {
            this.id = id;
            this.content = content;
            this.answers = answers;
            this.correctAnswer = correctAnswer;
        }
        public Question(int id, string content, List<string> answers, int correctAnswer, int userAnswer)
        {
            this.id = id;
            this.content = content;
            this.answers = answers;
            this.correctAnswer = correctAnswer;
            this.UserAnswer = userAnswer;
        }

        public int Id { get => id; set => id = value; }
        public string Content { get => content; set => content = value; }
        public List<string> Answers { get => answers; set => answers = value; }
        public int CorrectAnswer { get => correctAnswer; set => correctAnswer = value; }
        public int UserAnswer { get => userAnswer; set => userAnswer = value; }
        // Generates random question with random answers.
        public static Question GetRandomQuestion() {
            Random random = new Random();
            int randomNumber = random.Next(1, 5);
            switch(randomNumber)
            {
                case 1:
                    return GenerateQuestionWhoSang();
                case 2:
                    return GenerateQuestionWhenWasTheSongReleased();
                case 3:
                    return GenerateQuestionWhichGenreIsTheSong();
                case 4:
                    return GenerateQuestionWhichOfTheseIsABand();
                default:
                    return GenerateQuestionWhoSang();
            }
        }
        // יוצר שאלה רנדומלית מהפורמט: מי שר את השיר הבא?
        private static Question GenerateQuestionWhoSang()
        {
            Dictionary<string, object> RS = Song.GetRandomSong();
            List<string> randomArtists = Performer.Get3RandomPerformers(Convert.ToInt32(RS["PerformerID"]));
            randomArtists.Add(RS["PerformerName"].ToString());
            Question q = new Question(-1, "Who sang the song: " + RS["SongName"].ToString() + "?", randomArtists, 3);
            q.ShuffleAnswers();
            return q;
        }
        // Format:
        // מתי שוחרר השיר?
        private static Question GenerateQuestionWhenWasTheSongReleased()
        {
            Dictionary<string, object> RS = Song.GetRandomSong();
            int YearToIgnore = Convert.ToInt32(RS["ReleaseYear"]);
            List<string> randomYears = Song.Get3RandomReleaseYear(YearToIgnore);
            randomYears.Add(RS["ReleaseYear"].ToString());
            Question q = new Question(-1, "When was the next song released: " + RS["SongName"].ToString() + "?", randomYears, 3);
            q.ShuffleAnswers();
            return q;
        }

        // Format:
        // מאיזה ז'אנר השיר הבא?
        private static Question GenerateQuestionWhichGenreIsTheSong()
        {
            Dictionary<string, object> RS = Song.GetRandomSong();
            List<string> randomGenres = Genre.Get3RandomGenres(RS["GenreName"].ToString());
            randomGenres.Add(RS["GenreName"].ToString());
            Question q = new Question(-1, "Which genre is the song: " + RS["SongName"].ToString() + " related to?", randomGenres, 3);
            q.ShuffleAnswers();
            return q;
        }

        // Format:
        // איזה מהבאים הוא להקה?
        private static Question GenerateQuestionWhichOfTheseIsABand()
        {
            string Band = Performer.GetRandomBand();
            List<string> singleArtists = Performer.Get3RandomSingleArtists();
            singleArtists.Add(Band);
            Question q = new Question(-1, "Which of the following artists is a band?", singleArtists, 3);
            q.ShuffleAnswers();
            return q;
        }

        // Shuffles the answers of the question.
        private void ShuffleAnswers()
        {
            Random random = new Random();
            int n = Answers.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                string value = Answers[k];
                Answers[k] = Answers[n];
                Answers[n] = value;

                // Update the CorrectAnswerIndex if the correct answer was shuffled
                if (correctAnswer == k)
                {
                    correctAnswer = n;
                }
                else if (correctAnswer == n)
                {
                    correctAnswer = k;
                }
            }
        }
        // Gets the answers of the question.
        public List<string> getAnswers()
        {
            return answers;
        }
        // Updates user answer to this question.
        public static bool PutUserAnswer(int QID, int answer)
        {
            if (QID < 1)
                throw new ArgumentException("Question doesn't exist");
            if (answer > 3 || answer < 0)
                throw new ArgumentException("Incorrect answer value");
            DBservices db = new DBservices();
            return db.PutUserAnswer(QID, answer) > 0;
        }
    }
}
