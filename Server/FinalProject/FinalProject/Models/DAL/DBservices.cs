using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Xml.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Components.Web;
using FinalProject;
using Twilio.Rest.Verify.V2.Service;
using Twilio;
using Twilio.TwiML.Voice;
//using SendGrid;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }
    // Update user details, and require email verification if changed. (New token) otherwise, token is set to empty string.
    public int Update(User u, bool isNewEmail, string token)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", u.Id);
        paramDic.Add("@UserName", u.Name);
        paramDic.Add("@UserEmail", u.Email);
        paramDic.Add("@UserPassword", u.Password);
        paramDic.Add("@isNewEmail", isNewEmail ? 1 : 0);
        paramDic.Add("@token", token);
        if (u.Image != null && u.Image != "") paramDic.Add("@image", Convert.FromBase64String(u.Image));



        cmd = CreateCommandWithStoredProcedure(u.Image != null && u.Image != "" ? "Proj_SP_UpdateUserImage" : "Proj_SP_UpdateUser", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    public int UpdateExpoToken(int UserID, string NewExpoToken)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);
        paramDic.Add("@ExpoToken", NewExpoToken);



        cmd = CreateCommandWithStoredProcedure("Proj_SP_StoreExpoToken", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Used for admins to ban users
    public int BanUser(int UserID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);



        cmd = CreateCommandWithStoredProcedure("Proj_SP_BanUser", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Used for admins to unban users
    public int UnbanUser(int UserID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);



        cmd = CreateCommandWithStoredProcedure("Proj_SP_UnbanUser", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Returns true if this user is verified
    public bool IsUserVerified(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", id);


        cmd = CreateCommandWithStoredProcedure("Proj_IsAccountVerified", con, paramDic);             // create the command
        

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                return Convert.ToInt32(dataReader["Result"]) == 1;
            }
            throw new ArgumentException("User doesn't exist");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets how many streams does the artist have (sum of all his songs)
    public object GetTotalStreamsOfArtist(int PerformerID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@PerformerID", PerformerID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetTotalStreamsOfArtist", con, paramDic);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                if (!dataReader.IsDBNull(dataReader.GetOrdinal("TotalPlays")))
                {
                    object res = new
                    {
                        TotalPlays = Convert.ToInt32(dataReader["TotalPlays"])
                    };
                    return res;
                }
                else return new
                {
                    TotalPlays = 0
                };
            }
            throw new ArgumentException("Performer doesn't exist");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets how many followers does the artist have.
    public object GetTotalFavoritesOfArtist(int PerformerID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@PerformerID", PerformerID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetTotalFavoritesOfPerformer", con, paramDic);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                object res = new
                {
                    TotalFavorites = Convert.ToInt32(dataReader["TotalFavorites"])
                };
                return res;
            }
            throw new ArgumentException("Performer doesn't exist");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets user registration date as json
    public object GetUserRegistarationDate(int UserID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetRegistarationDate", con, paramDic);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                object res = new
                {
                    RegistrationDate = dataReader["registrationDate"].ToString()
                };
                return res;
            }
            throw new ArgumentException("User doesn't exist");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets how many followers does the performer have.
    public object GetTotalFollowersOfPerformer(int PerformerID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@PerformerID", PerformerID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetTotalFollowersOfArtist", con, paramDic);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                object res = new
                {
                    TotalFollowers = Convert.ToInt32(dataReader["TotalFollowers"])
                };
                return res;
            }
            throw new ArgumentException("Performer doesn't exist");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets a random song as a dictionary. Used to generate questions.
    public Dictionary<string, object> GetRandomSong()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetRandomSong", con, null);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Dictionary<string, object> res = new Dictionary<string, object>();
                res.Add("SongID", Convert.ToInt32(dataReader["SongID"]));
                res.Add("SongName", dataReader["SongName"].ToString());
                res.Add("SongLyrics", dataReader["SongLyrics"].ToString());
                res.Add("GenreID", Convert.ToInt32(dataReader["GenreID"]));
                res.Add("ReleaseYear", Convert.ToInt32(dataReader["ReleaseYear"]));
                res.Add("PerformerID", Convert.ToInt32(dataReader["PerformerID"]));
                res.Add("NumOfPlays", Convert.ToInt32(dataReader["NumOfPlays"]));
                res.Add("SongLength", dataReader["SongLength"].ToString());
                res.Add("PerformerName", dataReader["PerformerName"].ToString());
                res.Add("GenreName", dataReader["GenreName"].ToString());
                return res;
            }
            throw new ArgumentException("DB ERROR - NOTHING WAS RETURNED!");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // User clicked verify, checks the token and timestamp.
    public int ValidateUser(string email, string token)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserEmail", email);
        paramDic.Add("@UserToken", token);



        cmd = CreateCommandWithStoredProcedure("Proj_SP_Validate_Email", con, paramDic);             // create the command
        var returnParameter = cmd.Parameters.Add("@flag", SqlDbType.Int);
        returnParameter.Direction = ParameterDirection.ReturnValue;

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
                // note that the return value appears only after closing the connection
                int result = (int)returnParameter.Value;
                if (result == 1)
                    throw new ArgumentException("This user doesn't exist");
                if (result == 2)
                    throw new ArgumentException("You're already verified!");
                if (result == 3)
                    throw new ArgumentException("Invalid token");
                if (result == 4)
                    throw new ArgumentException("30 minutes have passed. Try again!");
            }
        }
    }
    // Inserts an artist and gets the image from Google Images, using an API.
    public int InsertArtistAndImageUsingAI(string name, string imageurl)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@PerformerName", name);
        paramDic.Add("@PerformerImage", imageurl);



        cmd = CreateCommandWithStoredProcedure("Proj_SP_InsertArtist", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Inserts a performer from managePortal.html (Only admins can do that!)
    public int AdminInsertPerformer(Performer p)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@PerformerName", p.PerformerName);
        paramDic.Add("@isABand", p.IsABand == 0 ? 0 : 1);
        paramDic.Add("@PerformerImage", p.PerformerImage);
        paramDic.Add("@PerformerInstagram", p.Instagram);



        cmd = CreateCommandWithStoredProcedure("Proj_SP_AdminInsertPerformer", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets top 15 songs to feature. UserID is used to know whether the user has them on his favorites.
    public List<object> GetTop15(int UserID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);

        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetTop15", con, paramDic);             // create the command


        List<object> songs = new List<object>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                int SongID = Convert.ToInt32(dataReader["SongID"]);
                int PerformerID = Convert.ToInt32(dataReader["PerformerID"]);
                string SName = dataReader["SongName"].ToString();
                string PName = dataReader["PerformerName"].ToString();
                string PImage = dataReader["PerformerImage"].ToString();
                int NumOfPlays = Convert.ToInt32(dataReader["NumOfPlays"]);
                string GName = dataReader["GenreName"].ToString();
                string SLength = dataReader["SongLength"].ToString();
                if (SLength != null && SLength.Contains(' '))
                    SLength = SLength.Substring(0, SLength.IndexOf(' '));
                int InFav = Convert.ToInt32(dataReader["InFav"]);
                object s = new { SongID = SongID, SongName = SName, PerformerName = PName, PerformerID = PerformerID,
                PerformerImage = PImage, NumOfPlays = NumOfPlays, GenreName = GName, SongLength = SLength, IsInFav = InFav };
                songs.Add(s);
            }

            return songs;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    public List<object> GetUserSongHistory(int UserID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);

        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetUserRecentlyPlayed", con, paramDic);             // create the command


        List<object> songs = new List<object>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                int SongID = Convert.ToInt32(dataReader["SongID"]);
                int PerformerID = Convert.ToInt32(dataReader["PerformerID"]);
                string SName = dataReader["SongName"].ToString();
                string PName = dataReader["PerformerName"].ToString();
                string PImage = dataReader["PerformerImage"].ToString();
                string GName = dataReader["GenreName"].ToString();
                string SLength = dataReader["SongLength"].ToString();
                if (SLength != null && SLength.Contains(' '))
                    SLength = SLength.Substring(0, SLength.IndexOf(' '));
                int InFav = Convert.ToInt32(dataReader["InFav"]);
                object s = new
                {
                    SongID = SongID,
                    SongName = SName,
                    PerformerName = PName,
                    PerformerID = PerformerID,
                    PerformerImage = PImage,
                    GenreName = GName,
                    Length = SLength,
                    IsInFav = InFav
                };
                songs.Add(s);
            }

            return songs;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    public List<SongRecommendation> GetUserRecommendedSongs(int UserID, int NumberOfSongs = 15)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);

        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetUsersSongs", con, paramDic);             // create the command



        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            RecommendationEngine engine = new RecommendationEngine();
            while (dataReader.Read())
            {
                int SongID = Convert.ToInt32(dataReader["SongID"]);
                int PerformerID = Convert.ToInt32(dataReader["PerformerID"]);
                int TotalLikes = Convert.ToInt32(dataReader["TotalLikes"]);
                string SName = dataReader["SongName"].ToString();
                string PName = dataReader["PerformerName"].ToString();
                string PImage = dataReader["PerformerImage"].ToString();
                int NumOfPlays = Convert.ToInt32(dataReader["NumOfPlays"]);
                int ReleaseYear = Convert.ToInt32(dataReader["ReleaseYear"]);
                int RecentlyPlayed = Convert.ToInt32(dataReader["IsInRecentlyPlayed"]);
                string GName = dataReader["GenreName"].ToString();
                string SLength = dataReader["SongLength"].ToString();
                if (SLength != null && SLength.Contains(' '))
                    SLength = SLength.Substring(0, SLength.IndexOf(' '));
                int InFav = Convert.ToInt32(dataReader["InFav"]);
                int FollowingArtist = Convert.ToInt32(dataReader["FollowingArtist"]);
                int LikesGenre = Convert.ToInt32(dataReader["LikesGenre"]);
                SongRecommendation s = new SongRecommendation(SongID, SName, NumOfPlays,
                    PerformerID, ReleaseYear, InFav, FollowingArtist, LikesGenre, SLength,
                    PName, PImage, GName, RecentlyPlayed, TotalLikes);
                engine.AddSong(s);
            }
            return engine.TurnonEngine(NumberOfSongs);
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    public List<object> GetExpoTokens()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetExpoTokens", con, null);             // create the command


        List<object> songs = new List<object>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                string ExpoToken = dataReader["ExpoToken"].ToString();
                object s = new
                {
                    ExpoToken = ExpoToken
                };
                songs.Add(s);
            }

            return songs;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Gets the chosen song's lyrics.
    public object GetSongLyrics(int SongID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@SongID", SongID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetSongLyrics", con, paramDic);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                string name = dataReader["SongName"].ToString();
                string lyrics = dataReader["SongLyrics"].ToString();
                object res = new {
                    SongName = name,
                    Lyrics = lyrics,
                    PerformerName = dataReader["PerformerName"].ToString()
                };
                return res;
            }
            throw new ArgumentException("Song doesn't exist");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    public DateTime GetLastTokenDate(int UserID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetLastTokenDate", con, paramDic);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                DateTime date = Convert.ToDateTime(dataReader["LastTokenTime"]);
                return date;
            }
            return new DateTime(2000, 1, 1);
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets the user XP by his id.
    public object GetUserXP(int UserID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetUserXP", con, paramDic);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                object res = new
                {
                    UserXP = Convert.ToInt32(dataReader["XP"])
                };
                return res;
            }
            throw new ArgumentException("Song doesn't exist");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets performer instagram handle by his id.
    public object GetPerformerInstagram(int PerformerID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@PerformerID", PerformerID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetPerformerInstagram", con, paramDic);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                if (dataReader["instagramTag"] != null)
                {
                    object res = new
                    {
                        instagram = dataReader["instagramTag"].ToString()
                    };
                    return res;
                }
                else return new
                {
                    instagram = "null"
                };
            }
            throw new ArgumentException("Performer doesn't exist");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets user favorites by his id.
    public List<object> GetUserFavorites(int UserID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetUserFavorites", con, paramDic);             // create the command

        List<object> favorites = new List<object>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                int SID = Convert.ToInt32(dataReader["SongID"]);
                int PID = Convert.ToInt32(dataReader["PerformerID"]);
                string name = dataReader["SongName"].ToString();
                string SLength = dataReader["SongLength"].ToString();
                if (SLength != null && SLength.Contains(' '))
                    SLength = SLength.Substring(0, SLength.IndexOf(' '));
                string PName = dataReader["PerformerName"].ToString();
                string PImage = dataReader["PerformerImage"].ToString();
                string GName = dataReader["GenreName"].ToString();
                object res = new
                {
                    SongID = SID,
                    SongName = name,
                    Length = SLength,
                    PerformerName = PName,
                    PerformerImage = PImage,
                    PerformerID = PID,
                    GenreName = GName,
                    IsInFav = 1
                };
                favorites.Add(res);
            }
            return favorites;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets the songs in a playlist by the id of the playlist.
    // Returns a list of objects rather than songs because we need to handle special data for this request. (such as image,
    // which is not a part of the Song class properties.)
    public List<object> GetPlaylistSongs(int PlaylistID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@PlaylistID", PlaylistID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetSongsInPlaylist", con, paramDic);             // create the command

        List<object> playlistSongs = new List<object>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                int SID = Convert.ToInt32(dataReader["SongID"]);
                string name = dataReader["SongName"].ToString();
                string SLength = dataReader["SongLength"].ToString();
                if (SLength != null && SLength.Contains(' '))
                    SLength = SLength.Substring(0, SLength.IndexOf(' '));
                string PName = dataReader["PerformerName"].ToString();
                string PImage = dataReader["PerformerImage"].ToString();
                int PEID = Convert.ToInt32(dataReader["PerformerID"]);
                int IsSongInFavorites = Convert.ToInt32(dataReader["InFav"]);
                object res = new
                {
                    SongID = SID,
                    SongName = name,
                    Length = SLength,
                    PerformerName = PName,
                    PerformerImage = PImage,
                    PerformerID = PEID,
                    GenreName = dataReader["GenreName"].ToString(),
                    IsInFav = IsSongInFavorites
                };
                playlistSongs.Add(res);
            }
            return playlistSongs;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets a name of a playlist by its id, as a ready-to-cast json object.
    public object GetPlaylistName(int PlaylistID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@PlaylistID", PlaylistID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetPlaylistName", con, paramDic);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                object res = new
                {
                    PlaylistName = dataReader["PlaylistName"].ToString()
                };
                return res;
            }
            throw new Exception("Playlist not found");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    public long GetSongLength(int SongID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@SongID", SongID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetSongLength", con, paramDic);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                string length = dataReader["SongLength"].ToString();
                string[] parts = length.Split(':');
                int minutes = int.Parse(parts[0]);
                int seconds = int.Parse(parts[1]);

                // Convert minutes to seconds and add to total seconds
                int totalSeconds = minutes * 60 + seconds;

                // Convert total seconds to milliseconds
                int duration = totalSeconds * 1000;
                return duration;
            }
            throw new Exception("Song not found");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    public object GetNumberOfLikedSongs(int UserID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetNumberOfLikedSongs", con, paramDic);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                if (!dataReader.IsDBNull(dataReader.GetOrdinal("NumberOfFavoriteSongs")))
                {
                    object res = new
                    {
                        TotalLikedSongs = Convert.ToInt32(dataReader["NumberOfFavoriteSongs"])
                    };
                    return res;
                }
                else return new
                {
                    TotalLikedSongs = 0
                };
            }
            throw new ArgumentException("SERVER ERROR");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    public object GetNumberOfRecentlyPlayed(int UserID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetNumberOfRecentlyPlayed", con, paramDic);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                if (!dataReader.IsDBNull(dataReader.GetOrdinal("TotalRecentlyPlayed")))
                {
                    object res = new
                    {
                        TotalRecentlyPlayed = Convert.ToInt32(dataReader["TotalRecentlyPlayed"])
                    };
                    return res;
                }
                else return new
                {
                    TotalRecentlyPlayed = 0
                };
            }
            throw new ArgumentException("SERVER ERROR");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets user playlists by his id.
    public List<object> GetUserPlaylists(int UserID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetPlaylists", con, paramDic);             // create the command

        List<object> playlists = new List<object>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                object playlist = new
                {
                    id = Convert.ToInt32(dataReader["PlaylistID"]),
                    name = dataReader["PlaylistName"].ToString(),
                    numberOfSongs = Convert.ToInt32(dataReader["SongsInPlaylist"])
                };
                playlists.Add(playlist);
            }
            return playlists;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets all messages
    public List<Message> GetAllMessages()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetMessages", con, null);             // create the command

        List<Message> messages = new List<Message>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Message m = new Message();
                m.Subject = dataReader["subject"].ToString();
                m.UserID = Convert.ToInt32(dataReader["UserID"]);
                m.MessageID = Convert.ToInt32(dataReader["MessageID"]);
                m.Subject = dataReader["subject"].ToString();
                m.Content = dataReader["content"].ToString();
                m.Date = Convert.ToDateTime(dataReader["dateOfMessage"]);
                m.UserName = dataReader["UserName"].ToString();
                m.UserEmail = dataReader["UserEmail"].ToString();
                messages.Add(m);
            }
            return messages;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets most played performer. Used to generate the admin's general report.
    private Dictionary<string, object> AdminReportGetMostPlayedPerformer()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }



        cmd = CreateCommandWithStoredProcedure("Proj_SP_AdminReportGetMostPlayerPerformer", con, null);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Dictionary<string, object> res = new Dictionary<string, object>();
                res.Add("PerformerName", dataReader["PerformerName"].ToString());
                res.Add("TotalListeners", Convert.ToInt32(dataReader["TotalListeners"]));
                return res;
            }
            throw new Exception("DB ERROR");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    // Gets most followed performer. Used to generate the admin's general report.
    private Dictionary<string, object> AdminReportGetMostFollowedPerformer()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }



        cmd = CreateCommandWithStoredProcedure("Proj_SP_AdminReportGetMostFollowedPerformer", con, null);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Dictionary<string, object> res = new Dictionary<string, object>();
                res.Add("PerformerName", dataReader["PerformerName"].ToString());
                res.Add("TotalFollowers", Convert.ToInt32(dataReader["TotalFollowers"]));
                return res;
            }
            throw new Exception("DB ERROR");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    // Gets most played genre. Used to generate the admin's general report.
    private Dictionary<string, object> AdminReportGetMostPlayedGenre()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }



        cmd = CreateCommandWithStoredProcedure("Proj_SP_AdminReportGetMostPlayedGenre", con, null);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Dictionary<string, object> res = new Dictionary<string, object>();
                res.Add("GenreName", dataReader["GenreName"].ToString());
                res.Add("TotalPlays", Convert.ToInt32(dataReader["TotalPlays"]));
                return res;
            }
            throw new Exception("DB ERROR");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    // Gets how many users we have. Used to generate the admin's general report.
    private int AdminReportGetHowManyUsers()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }



        cmd = CreateCommandWithStoredProcedure("Proj_SP_AdminReportHowManyUsers", con, null);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                return Convert.ToInt32(dataReader["TotalUsers"]);
            }
            throw new Exception("DB ERROR");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    // Gets how many solo quizzes users have played. Used to generate the admin's general report.
    private int AdminReportGetHowManyQuizzes()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }



        cmd = CreateCommandWithStoredProcedure("Proj_SP_AdminGetHowManySoloQuizzes", con, null);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                return Convert.ToInt32(dataReader["TotalQuizzes"]);
            }
            throw new Exception("DB ERROR");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    // Builds the actual admin general report using the functions above, and returns as object. The admin can also choose to download
    // as a csv file the report if he'd like to.
    public object BuildReport()
    {
        Dictionary<string, object> MostPlayedPerformer = AdminReportGetMostPlayedPerformer();
        Dictionary<string, object> MostFollowedPerformer = AdminReportGetMostFollowedPerformer();
        Dictionary<string, object> MostPlayedGenre = AdminReportGetMostPlayedGenre();
        int HowManyUsers = AdminReportGetHowManyUsers();
        int SoloQuizzes = AdminReportGetHowManyQuizzes();
        return new
        {
            MostPlayedPerformer = MostPlayedPerformer["PerformerName"],
            NumOfPlaysMostPlayedPerformer = MostPlayedPerformer["TotalListeners"],
            MostFollowedPerformer = MostFollowedPerformer["PerformerName"],
            NumOfFollowersMostFollowedPerformer = MostFollowedPerformer["TotalFollowers"],
            MostPlayedGenre = MostPlayedGenre["GenreName"],
            MostPlayedGenrePlays = MostPlayedGenre["TotalPlays"],
            NumberOfUsers = HowManyUsers,
            SoloQuizzesPlayed = SoloQuizzes
        };
    }
    // Adds user the given XP amount.
    public int AddUserXP(int UserID, int XP)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);
        paramDic.Add("@XPToAdd", XP);

        cmd = CreateCommandWithStoredProcedure("Proj_SP_AddUserXP", con, paramDic);             // create the command


        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Search the query. UserID is used to know whether this user has the songs on his favorites or not.
    public List<object> Search(string query, int UserID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@Query", query);
        paramDic.Add("@UserID", UserID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_Search", con, paramDic);             // create the command

        List<object> searchResults = new List<object>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                int SID = Convert.ToInt32(dataReader["SongID"]);
                string name = dataReader["SongName"].ToString();
                int NOP = Convert.ToInt32(dataReader["NumOfPlays"]);
                string SLength = dataReader["SongLength"].ToString();
                if (SLength != null && SLength.Contains(' '))
                    SLength = SLength.Substring(0, SLength.IndexOf(' '));
                string PName = dataReader["PerformerName"].ToString();
                string PImage = dataReader["PerformerImage"].ToString();
                int PID = Convert.ToInt32(dataReader["PerformerID"]);
                string GName = dataReader["GenreName"].ToString();
                int IQIL = Convert.ToInt32(dataReader["IsQueryInLyrics"]);
                int IsInFav = Convert.ToInt32(dataReader["InFav"]);
                int UF = Convert.ToInt32(dataReader["UserFavorites"]);
                int AF = Convert.ToInt32(dataReader["ArtistFavorites"]);
                object res = new
                {
                    SongID = SID,
                    SongName = name,
                    NumOfPlays = NOP,
                    Length = SLength,
                    PerformerID = PID,
                    PerformerName = PName,
                    PerformerImage = PImage,
                    GenreName = GName,
                    IsQueryInLyrics = IQIL,
                    IsInFavorites = IsInFav,
                    SongFavorites = UF,
                    ArtistFavorites = AF
                };
                searchResults.Add(res);
            }
            return searchResults;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    public List<object> SearchNative(string query, int UserID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@Query", query);
        paramDic.Add("@UserID", UserID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_SearchByQuery", con, paramDic);             // create the command

        List<object> searchResults = new List<object>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                int SID = Convert.ToInt32(dataReader["SongID"]);
                string name = dataReader["SongName"].ToString();
                int NOP = Convert.ToInt32(dataReader["NumOfPlays"]);
                string SLength = dataReader["SongLength"].ToString();
                if (SLength != null && SLength.Contains(' '))
                    SLength = SLength.Substring(0, SLength.IndexOf(' '));
                string PName = dataReader["PerformerName"].ToString();
                string PImage = dataReader["PerformerImage"].ToString();
                int PID = Convert.ToInt32(dataReader["PerformerID"]);
                string GName = dataReader["GenreName"].ToString();
                int IsInFav = Convert.ToInt32(dataReader["InFav"]);
                int UF = Convert.ToInt32(dataReader["UserFavorites"]);
                int AF = Convert.ToInt32(dataReader["ArtistFavorites"]);
                object res = new
                {
                    SongID = SID,
                    SongName = name,
                    NumOfPlays = NOP,
                    Length = SLength,
                    PerformerID = PID,
                    PerformerName = PName,
                    PerformerImage = PImage,
                    GenreName = GName,
                    IsInFav = IsInFav,
                    SongFavorites = UF,
                    ArtistFavorites = AF
                };
                searchResults.Add(res);
            }
            return searchResults;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets featured performers for the homepage.
    public List<Performer> GetFeaturedArtists()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetFeaturedArtists", con, null);             // create the command


        List<Performer> artists = new List<Performer>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Performer p = new Performer();
                p.PerformerID = Convert.ToInt32(dataReader["PerformerID"]);
                p.PerformerName = dataReader["PerformerName"].ToString();
                p.PerformerImage = dataReader["PerformerImage"].ToString();
                artists.Add(p);
            }

            return artists;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Gets the quizzes leaderboard.
    public List<object> GetLeaderboard()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetLeaderboard", con, null);             // create the command


        List<object> users = new List<object>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                string UName = dataReader["UserName"].ToString();
                int XP = Convert.ToInt32(dataReader["XP"]);
                int QGR = Convert.ToInt32(dataReader["SoloQuestionsGotRight"]);
                int SQA = Convert.ToInt32(dataReader["SoloQuestionsAnswered"]);
                int GP = Convert.ToInt32(dataReader["TotalGamesPlayed"]);
                object s = new
                {
                    UserName = UName,
                    XP = XP,
                    GamesPlayed = GP,
                    SoloQuestionsGotRight = QGR,
                    SoloQuestionsAnswered = SQA,
                    SoloAverage = SQA == 0 ? 0.0f : (QGR / (float)SQA) * 100,
                    test1 = QGR,
                    test2 = SQA,
                    Level = Math.Floor(XP / 100.0f) + 1,
                    image = !dataReader.IsDBNull(dataReader.GetOrdinal("Image")) ? Convert.ToBase64String((byte[])dataReader["Image"]) : null,
                    UserID = Convert.ToInt32(dataReader["UserID"])
                };
                users.Add(s);
            }
            return users;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Gets all artists
    public List<Performer> GetArtists()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetArtist", con, null);             // create the command


        List<Performer> artists = new List<Performer>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Performer p = new Performer();
                p.PerformerID = Convert.ToInt32(dataReader["PerformerID"]);
                p.PerformerName = dataReader["PerformerName"].ToString();
                p.PerformerImage = dataReader["PerformerImage"].ToString();
                artists.Add(p);
            }

            return artists;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Gets artists sorted by # of plays for the admin's report. Admin can also download the report later.
    public List<object> AdminGetPerformersData()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("Proj_SP_AdminGetPerformerData", con, null);             // create the command


        List<object> performers = new List<object>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                int PerformerID = Convert.ToInt32(dataReader["PerformerID"]);
                string PName = dataReader["PerformerName"].ToString();
                string PInstagram = dataReader["instagramTag"].ToString();
                bool ISB = Convert.ToInt32(dataReader["isABand"]) == 1;
                int TPlays = Convert.ToInt32(dataReader["TotalPlays"]);
                int TFollowers = Convert.ToInt32(dataReader["TotalFollowers"]);
                int NOUF = Convert.ToInt32(dataReader["NumOfUserFavorites"]);
                object s = new
                {
                    PerformerID = PerformerID,
                    PerformerName = PName,
                    isABand = ISB,
                    PerformerInstagram = PInstagram,
                    TotalPlays = TPlays,
                    TotalFollowers = TFollowers,
                    NumOfUserFavorites = NOUF
                };
                performers.Add(s);
            }

            return performers;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Gets songs sorted by # of plays for the admin's report. Admin can also download the report later.
    public List<object> AdminGetSongsInfo()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetSongsData", con, null);             // create the command


        List<object> songs = new List<object>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                int SID = Convert.ToInt32(dataReader["SongID"]);
                string SName = dataReader["SongName"].ToString();
                int RYear = Convert.ToInt32(dataReader["ReleaseYear"]);
                int NOP = Convert.ToInt32(dataReader["NumOfPlays"]);
                string SLength = dataReader["SongLength"].ToString();
                if (SLength != null && SLength.Contains(' '))
                    SLength = SLength.Substring(0, SLength.IndexOf(' '));
                string PName = dataReader["PerformerName"].ToString();
                string GName = dataReader["GenreName"].ToString();
                int TFav = Convert.ToInt32(dataReader["TotalFavorites"]);
                object s = new
                {
                    SongID = SID,
                    SongName = SName,
                    ReleaseYear = RYear,
                    NumOfPlays = NOP,
                    SongLength = SLength,
                    PerformerName = PName,
                    GenreName = GName,
                    TotalFavorites = TFav
                };
                songs.Add(s);
            }

            return songs;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Used when a user wants to follow an artist.
    public int FollowArtist(int UserID, int PerformerID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);
        paramDic.Add("@PerformerID", PerformerID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_FollowArtist", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            // int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Used when a user wants to unfollow an aritst.
    public int UnfollowArtist(int UserID, int PerformerID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);
        paramDic.Add("@PerformerID", PerformerID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_UnfollowArtist", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            // int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Inserts a new comment to the db. Only following user of this artists can post.
    // This way, we've built a fan club of the artist.
    public int Insert(Comment c)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", c.UserID);
        paramDic.Add("@PerformerID", c.PerformerID);
        paramDic.Add("@Content", c.Content);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_PostComment", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            // int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Inserts a new message
    public int Insert(Message m)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@subject", m.Subject);
        paramDic.Add("@content", m.Content);
        paramDic.Add("@UserID", m.UserID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_PostMessage", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            // int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets songs data of this artist. Returns them as list of object to insert special data for this request.
    public List<object> GetPerformerSongs(int PID, int UserID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@PerformerID", PID);
        paramDic.Add("@UserID", UserID);
        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetPerformerSongs", con, paramDic);             // create the command


        List<object> songs = new List<object>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                int SID = Convert.ToInt32(dataReader["SongID"]);
                string PName = dataReader["PerformerName"].ToString();
                string PImage = dataReader["PerformerImage"].ToString();
                string SName = dataReader["SongName"].ToString();
                string SLength = dataReader["SongLength"].ToString();
                if (SLength != null && SLength.Contains(' '))
                    SLength = SLength.Substring(0, SLength.IndexOf(' '));
                int IsInFav = Convert.ToInt32(dataReader["InFav"]);
                int favorites = Convert.ToInt32(dataReader["SongNumOfFav"]);
                object s = new
                {
                    SongID = SID,
                    PerformerName = PName,
                    PerformerImage = PImage,
                    SongName = SName,
                    SongLength = SLength,
                    IsInFav = IsInFav,
                    SongTotalFavorites = favorites,
                    IsUserFollowingArtist = UserID < 1 ? -1 : Convert.ToInt32(dataReader["IsUserFollowingArtist"])
                };
                songs.Add(s);
            }

            return songs;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Gets songs of a specific genre. Returns them as list of object to insert special data for this request.
    public List<object> GetGenreSongs(int GenreID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@GenreID", GenreID);
        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetGenreSongs", con, paramDic);             // create the command


        List<object> songs = new List<object>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                int SID = Convert.ToInt32(dataReader["SongID"]);
                string PName = dataReader["PerformerName"].ToString();
                string PImage = dataReader["PerformerImage"].ToString();
                string SName = dataReader["SongName"].ToString();
                object s = new
                {
                    SongID = SID,
                    PerformerName = PName,
                    PerformerImage = PImage,
                    SongName = SName
                };
                songs.Add(s);
            }

            return songs;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    public List<object> GetGenreSongsWithUserData(int GenreID, int UserID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@GenreID", GenreID);
        paramDic.Add("@UserID", UserID);
        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetGenreSongsWithUser", con, paramDic);             // create the command


        List<object> songs = new List<object>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                int SID = Convert.ToInt32(dataReader["SongID"]);
                string PName = dataReader["PerformerName"].ToString();
                string PImage = dataReader["PerformerImage"].ToString();
                string SName = dataReader["SongName"].ToString();
                string SLength = dataReader["SongLength"].ToString();
                if (SLength != null && SLength.Contains(' '))
                    SLength = SLength.Substring(0, SLength.IndexOf(' '));
                object s = new
                {
                    SongID = SID,
                    PerformerName = PName,
                    PerformerImage = PImage,
                    SongName = SName,
                    PerformerID = Convert.ToInt32(dataReader["PerformerID"]),
                    Length = SLength,
                    IsInFav = Convert.ToInt32(dataReader["InFav"])
                };
                songs.Add(s);
            }

            return songs;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    // Gets the most played song to feature on our homepage. Returns as an object to insert special data for this request. (such as image)
    public object GetMostPlayedTrack()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetMostPlayedTrack", con, null);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                int SID = Convert.ToInt32(dataReader["SongID"]);
                string PName = dataReader["PerformerName"].ToString();
                string PImage = dataReader["PerformerImage"].ToString();
                string SLength = dataReader["SongLength"].ToString();
                if (SLength != null && SLength.Contains(' '))
                    SLength = SLength.Substring(0, SLength.IndexOf(' '));
                string SName = dataReader["SongName"].ToString();
                int NumOfPlays = Convert.ToInt32(dataReader["NumOfPlays"]);
                object s = new
                {
                    SongID = SID,
                    SongName = SName,
                    SongLength = SLength,
                    PerformerName = PName,
                    NumOfPlays = NumOfPlays,
                    PerformerImage = PImage
                };
                return s;
            }
            throw new ArgumentException("ERROR");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Returns the genres sorted by # of plays. Returns them as list of object to insert special data for this request.
    // For example, NumOfSongs and NumOfPlays.
    public List<object> GetGenresByPlaysDesc()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("Proj_SP_AdminGetMostLovedGenre", con, null);             // create the command
        List<object> res = new List<object>();
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                int GID = Convert.ToInt32(dataReader["GenreID"]);
                string Name = dataReader["GenreName"].ToString();
                int NumOfPlays = Convert.ToInt32(dataReader["SumPlays"]);
                int NOS = Convert.ToInt32(dataReader["NumOfSongs"]);
                object s = new
                {
                    GenreID = GID,
                    GenreName = Name,
                    NumOfSongs = NOS,
                    NumOfPlays = NumOfPlays
                };
                res.Add(s);
            }
            return res;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    // Creates a new email verification request.
    public int InitiateNewValidation(int id, string token)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", id);
        paramDic.Add("@UserToken", token);



        cmd = CreateCommandWithStoredProcedure("Proj_SP_InitiateNewValidation", con, paramDic);             // create the command
        var returnParameter = cmd.Parameters.Add("@flag", SqlDbType.Int);
        returnParameter.Direction = ParameterDirection.ReturnValue;

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
                // note that the return value appears only after closing the connection
                int result = (int)returnParameter.Value;
                if (result == 1)
                    throw new ArgumentException("This user doesn't exist");
                if (result == 2)
                    throw new ArgumentException("You're already verified!");
            }
        }
    }
    // Updates user answer to this question.
    public int PutUserAnswer(int QuestionID, int answer)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@QuestionID", QuestionID);
        paramDic.Add("@answer", answer);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_PostUserAnswer", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            // int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    public int EditPlaylistName(int PlaylistID, string PlaylistName)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@PlaylistID", PlaylistID);
        paramDic.Add("@PlaylistName", PlaylistName);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_EditPlaylistName", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            // int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    public int UpdateUserPositionInSong(int UserID, int SongID, long NewPosition)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);
        paramDic.Add("@SongID", SongID);
        paramDic.Add("@NewPosition", NewPosition);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_UpdatePositionInSong", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            // int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    public int PostUserRecentlyPlayed(int UserID, int SongID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);
        paramDic.Add("@SongID", SongID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_PostUserRecentlyPlayed", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            // int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    public int PutUserPhone(int UserID, string PhoneNumber)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);
        paramDic.Add("@PhoneNumber", PhoneNumber);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_UpdateUserPhoneNumber", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            // int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Inserts the question to this quiz.
    public int Insert(Question q, int quizID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@content", q.Content);
        paramDic.Add("@answer1", q.getAnswers()[0]);
        paramDic.Add("@answer2", q.getAnswers()[1]);
        paramDic.Add("@answer3", q.getAnswers()[2]);
        paramDic.Add("@answer4", q.getAnswers()[3]);
        paramDic.Add("@correctAnswer", q.CorrectAnswer);
        paramDic.Add("@QuizID", quizID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_InsertQuestion", con, paramDic);             // create the command
        int questionID = 0;
        try
        {
            // Add output parameter for QuizID
            SqlParameter questionIdParameter = new SqlParameter("@QuestionID", SqlDbType.Int);
            questionIdParameter.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(questionIdParameter);

            int numEffected = cmd.ExecuteNonQuery(); // execute the command

            // Retrieve the QuizID value from the output parameter
            if (cmd.Parameters["@QuestionID"].Value != DBNull.Value)
            {
                questionID = Convert.ToInt32(cmd.Parameters["@QuestionID"].Value);
            }

            return questionID;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Inserts a playlist. Returns the current identity scope of the playlists table.
    public object Insert(Playlist p)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", p.UserID);
        paramDic.Add("@PlaylistName", p.Name);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_CreateUserPlaylist", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            // int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return GetPlaylistIdentity();
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Returns the current identity scope of the playlist table, as ready-to-cast json object.
    public object GetPlaylistIdentity()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetPlaylistIDidentity", con, null);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                object res = new
                {
                    PlaylistID = Convert.ToInt32(dataReader["CurrentIdentity"])
                };
                return res;
            }
            throw new Exception("SERVER ERROR");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    public ulong GetLastPositionInSong(int UserID, int SongID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);
        paramDic.Add("@SongID", SongID);

        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetLastPositionInSong", con, paramDic);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                return Convert.ToUInt64(dataReader["LastPosition"]);
            }
            return 0;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    public string SendCode(int UserID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);

        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetUserPhone", con, paramDic);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                string Number = dataReader["PhoneNumber"].ToString();
                if (!Number.Contains('-'))
                {
                    TwilioClient.Init(APIKeys.GetTwilioAccountSID(), APIKeys.GetTwilioAuthToken());
                    var varification = VerificationResource.Create(to: Number, channel: "sms", pathServiceSid: APIKeys.GetTwilioPathServiceID());
                    return "Sent!";
                }
            }
            return "ERROR - Cannot send";
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    public object PhoneLogin(string Phone)
    {
        if (DoesPhoneExist(Phone))
        {
            TwilioClient.Init(APIKeys.GetTwilioAccountSID(), APIKeys.GetTwilioAuthToken());
            var varification = VerificationResource.Create(to: Phone, channel: "sms", pathServiceSid: APIKeys.GetTwilioPathServiceID());
            object res2 = new
            {
                message = "Sent!"
            };
            return res2;
        }
        object res = new
        {
            message = "This phone number is not registered to any user!"
        };
        return res;
    }
    public object VerifyCode(string phone, string code)
    {
        TwilioClient.Init(APIKeys.GetTwilioAccountSID(), APIKeys.GetTwilioAuthToken());
        var verificationCheck = VerificationCheckResource.Create(
            to: phone,
            code: code,
            pathServiceSid: APIKeys.GetTwilioPathServiceID()
        );
        object res = new
        {
            message = verificationCheck.Status
        };
        return res;
    }
    public bool DoesPhoneExist(string Phone)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@PhoneNumber", Phone);

        cmd = CreateCommandWithStoredProcedure("Proj_SP_PhoneExists", con, paramDic);
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                int NumberExists = Convert.ToInt32(dataReader["PhoneExists"]);
                return NumberExists > 0;
            }
            throw new Exception("Server Error");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    public object GetUserPhoneNumber(int UserID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);

        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetUserPhone", con, paramDic);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                string Number = dataReader["PhoneNumber"].ToString();
                if (Number.Contains('-'))
                    Number = null;
                object res = new
                {
                    PhoneNumber = Number
                };
                return res;
            }
            throw new Exception("SERVER ERROR");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Inserts a song to the chosen playlist.
    public int InsertSongToPlaylist(SongInPlaylist s)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@PlaylistID", s.PlaylistID);
        paramDic.Add("@SongID", s.SongID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_InsertSongToPlaylist", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            // int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Adds a new song to this user's favorites.
    public object PostUserFavorite(int UserID, int SongID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        SqlCommand cmdRead;

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);
        paramDic.Add("@SongID", SongID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_PostUserFavorite", con, paramDic);             // create the command

        Dictionary<string, object> paramDicRead = new Dictionary<string, object>();
        paramDicRead.Add("@SongID", SongID);
        cmdRead = CreateCommandWithStoredProcedure("Proj_SP_GetSongDataByID", con, paramDicRead);

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            SqlDataReader dataReader = cmdRead.ExecuteReader(CommandBehavior.CloseConnection);
            while (dataReader.Read())
            {
                string sLength = dataReader["SongLength"].ToString();
                if (sLength.Contains(' '))
                {
                    sLength = sLength.Replace(" ", "");
                }
                object res = new
                {
                    GenreName = dataReader["GenreName"].ToString(),
                    length = sLength,
                    PerformerID = Convert.ToInt32(dataReader["PerformerID"]),
                    PerformerImage = dataReader["PerformerImage"].ToString(),
                    PerformerName = dataReader["PerformerName"].ToString(),
                    SongID = Convert.ToInt32(dataReader["SongID"]),
                    SongName = dataReader["SongName"].ToString(),
                    IsInFav = 1
                };
                return res;
            }
            // int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Returns all user information for the admin's report. Admins can download the report later.
    // For security reasons, some information is not returned. Such as passwords.
    public List<User> LoadUserInformation()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("Proj_SP_LoadUserInformation", con, null);             // create the command


        List<User> userList = new List<User>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                User u = new User();
                u.Id = Convert.ToInt32(dataReader["UserID"]);
                u.Email = dataReader["UserEmail"].ToString();
                u.Name = dataReader["UserName"].ToString();
                u.RegistrationDate = Convert.ToDateTime(dataReader["registrationDate"]);
                u.IsBanned = Convert.ToInt32(dataReader["IsBanned"]) == 1;
                userList.Add(u);
            }
            return userList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    // Inserts a new user into the user table, gets a token to initiate an email validation process.
    public int Insert(User u, string Token)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();

        paramDic.Add("@email", u.Email);
        paramDic.Add("@password", u.Password);
        paramDic.Add("@name", u.Name);
        paramDic.Add("@token", Token);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_PostUser", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            // int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Gets 3 random artist names, as a list of strings to generate a question. Cannot include ArtistToNotInclude.
    public List<string> Get3RandomArtists(int ArtistToNotInclude)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@ArtistToNotInclude", ArtistToNotInclude);

        cmd = CreateCommandWithStoredProcedure("Proj_SP_Get3RandomArtists", con, paramDic);             // create the command


        List<string> artists = new List<string>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                artists.Add(dataReader["PerformerName"].ToString());
            }
            return artists;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Gets 3 random single performers (not bands) as a list of strings to generate a question.
    public List<string> Get3RandomSingleArtists()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("Proj_SP_Get3RandomSingleArtists", con, null);             // create the command


        List<string> artists = new List<string>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                artists.Add(dataReader["PerformerName"].ToString());
            }
            return artists;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Gets a random band name to generate a question.
    public string GetRandomBand()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetRandomBand", con, null);             // create the command



        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                return dataReader["PerformerName"].ToString();
            }
            throw new Exception("ERROR");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Returns 3 random release years of songs as a list of strings to generate a question. Cannot include ReleaseYearToIgnore
    public List<string> Get3RandomReleaseYear(int ReleaseYearToIgnore)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@ReleaseYearToIgnore", ReleaseYearToIgnore);

        cmd = CreateCommandWithStoredProcedure("Proj_SP_Get3RandomReleaseYears", con, paramDic);             // create the command


        List<string> years = new List<string>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                years.Add(dataReader["ReleaseYear"].ToString());
            }
            return years;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Gets 3 random genres names as a list of strings to generate a question. Cannot include GenreToIgnore.
    public List<string> Get3RandomGenres(string GenreToIgnore)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@GenreToIgnore", GenreToIgnore);

        cmd = CreateCommandWithStoredProcedure("Proj_SP_Get3RandomGenres", con, paramDic);             // create the command


        List<string> genres = new List<string>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                genres.Add(dataReader["GenreName"].ToString());
            }
            return genres;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    public User GetUserByEmail(string email)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@email", email);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetUserByEmail", con, paramDic);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                User u = new User();
                u.Email = dataReader["UserEmail"].ToString();
                u.Password = dataReader["UserPassword"].ToString();
                u.Name = dataReader["UserName"].ToString();
                u.Id = Convert.ToInt32(dataReader["UserID"]);
                u.IsBanned = Convert.ToInt32(dataReader["IsBanned"]) == 1;
                u.IsVerified = Convert.ToInt32(dataReader["UserIsVerified"]) == 1;
                u.RegistrationDate = Convert.ToDateTime(dataReader["registrationDate"]);
                if (!dataReader.IsDBNull(dataReader.GetOrdinal("Image")))
                {
                    u.Image = Convert.ToBase64String((byte[])dataReader["Image"]);
                }
                else
                {
                    u.Image = null;
                }
                return u;
            }
            throw new ArgumentException("User doesn't exist");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    public User GetUserByPhone(string Phone)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@PhoneNumber", Phone);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetUserByPhone", con, paramDic);             // create the command


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                User u = new User();
                u.Email = dataReader["UserEmail"].ToString();
                u.Password = dataReader["UserPassword"].ToString();
                u.Name = dataReader["UserName"].ToString();
                u.Id = Convert.ToInt32(dataReader["UserID"]);
                u.IsBanned = Convert.ToInt32(dataReader["IsBanned"]) == 1;
                u.IsVerified = Convert.ToInt32(dataReader["UserIsVerified"]) == 1;
                u.RegistrationDate = Convert.ToDateTime(dataReader["registrationDate"]);
                if (!dataReader.IsDBNull(dataReader.GetOrdinal("Image")))
                {
                    u.Image = Convert.ToBase64String((byte[])dataReader["Image"]);
                }
                else
                {
                    u.Image = null;
                }
                return u;
            }
            throw new ArgumentException("User doesn't exist");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // User login
    public User Login(string email, string password)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();

        paramDic.Add("@email", email);
        paramDic.Add("@password", password);

        cmd = CreateCommandWithStoredProcedure("Proj_SP_UserLogin", con, paramDic);             // create the command
        var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);
        returnParameter.Direction = ParameterDirection.ReturnValue;


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dataReader.Read())
            {
                User u = new User();
                u.Email = dataReader["UserEmail"].ToString();
                u.Password = dataReader["UserPassword"].ToString();
                u.Name = dataReader["UserName"].ToString();
                u.Id = Convert.ToInt32(dataReader["UserID"]);
                u.IsBanned = Convert.ToInt32(dataReader["IsBanned"]) == 1;
                u.IsVerified = Convert.ToInt32(dataReader["UserIsVerified"]) == 1;
                u.RegistrationDate = Convert.ToDateTime(dataReader["registrationDate"]);
                if (!dataReader.IsDBNull(dataReader.GetOrdinal("Image")))
                {
                    u.Image = Convert.ToBase64String((byte[])dataReader["Image"]);
                }
                else
                {
                    u.Image = null;
                }
                return u;
            }
            throw new Exception("Server error");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
            var result = returnParameter.Value;
            if ((int)result == 0)
                throw new ArgumentException("This user does not exist");
            else if ((int)result == 1) throw new ArgumentException("Wrong password");
        }
    }
    // TEMP - used for testing and inserts mp3 data as hex to this song id.
    // Saved in SQL as VARBINARY.
    public int InsertFileDataToSongID(int SongID, byte[] fileData)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@SongID", SongID);
        paramDic.Add("@FileData", fileData);



        cmd = CreateCommandWithStoredProcedure("Proj_SP_InsertFileDataToSongID", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }


    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con, Dictionary<string, object> paramDic)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        if(paramDic != null)
            foreach (KeyValuePair<string, object> param in paramDic) {
                cmd.Parameters.AddWithValue(param.Key,param.Value);

            }


        return cmd;
    }
    // Inserts a new quiz to our db.
    public int Insert(Quiz q)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        int quizId = 0; // Variable to store the QuizID value

        try
        {
            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserID", q.UserID);

            cmd = CreateCommandWithStoredProcedure("Proj_SP_InsertQuiz", con, paramDic); // create the command

            // Add output parameter for QuizID
            SqlParameter quizIdParameter = new SqlParameter("@QuizID", SqlDbType.Int);
            quizIdParameter.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(quizIdParameter);

            int numEffected = cmd.ExecuteNonQuery(); // execute the command

            // Retrieve the QuizID value from the output parameter
            if (cmd.Parameters["@QuizID"].Value != DBNull.Value)
            {
                quizId = Convert.ToInt32(cmd.Parameters["@QuizID"].Value);
            }

            return quizId;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    // TEMP - Used for testing purposes!!! to insert songs through the swagger.
    public int InsertSong(Song SongToInsert, byte[] fileData)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@SongName", SongToInsert.Name);
        paramDic.Add("@SongLyrics", SongToInsert.Lyrics);
        paramDic.Add("@ReleaseYear", SongToInsert.ReleaseYear);
        paramDic.Add("@GenreID", SongToInsert.GenreID);
        paramDic.Add("@FileData", fileData);



        cmd = CreateCommandWithStoredProcedure("Proj_SP_InsertSongFileData", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Returns a song by its id
    public FileContentResult ReadSongByID(int SongID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@SongID", SongID);

        cmd = CreateCommandWithStoredProcedure("Proj_ReadSongByID", con, paramDic);             // create the command
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dataReader.Read())
            {
                int fileDataIndex = dataReader.GetOrdinal("FileData");
                // Check if the column value is not DBNull
                if (!dataReader.IsDBNull(fileDataIndex))
                {
                    // Get the size of the file data
                    long fileSize = dataReader.GetBytes(fileDataIndex, 0, null, 0, 0);

                    // Create a byte array to hold the file data
                    byte[] fileData = new byte[fileSize];

                    // Read the file data into the byte array
                    dataReader.GetBytes(fileDataIndex, 0, fileData, 0, (int)fileSize);
                    // Set the Content-Range header
                    FileContentResult SongFile = new FileContentResult(fileData, "audio/mpeg");
                    return SongFile;
                }
            }
            throw new Exception("Song not found!");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Deletes a song from user's favorites.
    public int DeleteFromFavorites(int UserID, int SongID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);
        paramDic.Add("@SongID", SongID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_RemoveFromFavorites", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            // int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Delets a song from the playlist.
    public int DeleteSongFromPlaylist(int PlaylistID, int SongID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@PlaylistID", PlaylistID);
        paramDic.Add("@SongID", SongID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_DeleteSongFromPlaylist", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            // int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Delets a whole playlist.
    public int DeleteUserPlaylist(int UserID, int PlaylistID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@PlaylistID", PlaylistID);
        paramDic.Add("@UserID", UserID);


        cmd = CreateCommandWithStoredProcedure("Proj_SP_DeleteUserPlaylist", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            // int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets user past solo quizes data without the questions, to generate the quizhistory.html page.
    public List<object> GetUserPastQuizDataWithoutQuestions(int UserID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);

        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetUserPastQuizWithoutQuestions", con, paramDic);             // create the command
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<object> quizzes = new List<object>();
            while (dataReader.Read())
            {
                quizzes.Add(new
                {
                    QuizID = Convert.ToInt32(dataReader["QuizID"]),
                    QuizDate = dataReader["QuizDate"].ToString().Substring(0, 10),
                    QuizGrade = Math.Round((Convert.ToInt32(dataReader["QuestionsGotRight"]) / 5.0f) * 100.0f)
                });
            }
            return quizzes;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets a specific quiz questions to watch specific quiz history (also includes info such as grade, date, etc..)
    public Quiz GetQuizQuestions(int QuizID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@QuizID", QuizID);

        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetQuizQuestions", con, paramDic);             // create the command
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<Question> questions = new List<Question>();
            DateTime QuizDate = DateTime.Now;
            while (dataReader.Read())
            {
                List<string> answers = new List<string>();
                answers.Add(dataReader["answer1"].ToString());
                answers.Add(dataReader["answer2"].ToString());
                answers.Add(dataReader["answer3"].ToString());
                answers.Add(dataReader["answer4"].ToString());
                questions.Add(new Question(Convert.ToInt32(dataReader["QuestionID"]), dataReader["content"].ToString(), answers, Convert.ToInt32(dataReader["correctAnswer"]), Convert.ToInt32(dataReader["userAnswer"])));
                QuizDate = Convert.ToDateTime(dataReader["QuizDate"].ToString());
            }
            Quiz q = new Quiz(QuizID, -1, questions, QuizDate);
            return q;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets user past quizzes with the questions
    public List<Quiz> GetUserPastQuizzesAndQuestions(int UserID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);

        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetUserPastQuiz", con, paramDic);             // create the command


        List<Quiz> quizList = new List<Quiz>();
        int qID = -2;

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<Question> quizQuestions = new List<Question>();
            List<string> answers;
            while (dataReader.Read())
            {
                answers = new List<string>();
                answers.Add(dataReader["answer1"].ToString());
                answers.Add(dataReader["answer2"].ToString());
                answers.Add(dataReader["answer3"].ToString());
                answers.Add(dataReader["answer4"].ToString());
                Question q = new Question(Convert.ToInt32(dataReader["QuestionID"]), dataReader["content"].ToString(), answers, Convert.ToInt32(dataReader["correctAnswer"]), Convert.ToInt32(dataReader["userAnswer"]));
                if (qID == -2)
                {
                    qID = Convert.ToInt32(dataReader["QuizID"]);
                    quizQuestions = new List<Question>();
                    quizQuestions.Add(q);
                }
                else if (qID == Convert.ToInt32(dataReader["QuizID"])) // another question on the same quiz
                {
                    quizQuestions.Add(q);
                }
                else // New Quiz
                {
                    Quiz qu = new Quiz(qID, UserID, quizQuestions);
                    quizList.Add(qu);
                    qID = Convert.ToInt32(dataReader["QuizID"]);
                    quizQuestions = new List<Question>();
                    quizQuestions.Add(q);
                }
            }
            return quizList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Returns the comments on a specific performer.
    public List<Comment> ReadComments(int PerformerID)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@PerformerID", PerformerID);

        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetArtistsComments", con, paramDic);             // create the command


        List<Comment> commentsList = new List<Comment>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Comment c = new Comment();
                c.Content = dataReader["CommentContent"].ToString();
                c.UserName = dataReader["UserName"].ToString();
                c.Date = Convert.ToDateTime(dataReader["CommentDate"]);
                if (!dataReader.IsDBNull(dataReader.GetOrdinal("Image")))
                {
                    c.UserImage = Convert.ToBase64String((byte[])dataReader["Image"]);
                }
                else
                {
                    c.UserImage = null;
                }
                commentsList.Add(c);
            }
            return commentsList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // Gets all performers. Not sorted.
    public List<Performer> GetAllPerformers()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("Proj_SP_AdminGetAllArtists", con, null);             // create the command


        List<Performer> performers = new List<Performer>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dataReader.Read())
            {
                Performer p = new Performer();
                p.PerformerID = Convert.ToInt32(dataReader["PerformerID"]);
                p.PerformerName = dataReader["PerformerName"].ToString();
                p.IsABand = Convert.ToInt32(dataReader["isABand"]);
                p.PerformerImage = dataReader["PerformerImage"].ToString();
                p.Instagram = "null";
                performers.Add(p);
            }
            return performers;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets the user's following list. (Each performer he follows)
    public List<Performer> GetUserFollowingList(int UserID)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserID", UserID);

        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetUserFollowingPerformers", con, paramDic);             // create the command


        List<Performer> performers = new List<Performer>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dataReader.Read())
            {
                Performer p = new Performer();
                p.PerformerID = Convert.ToInt32(dataReader["PerformerID"]);
                p.PerformerName = dataReader["PerformerName"].ToString();
                p.PerformerImage = dataReader["PerformerImage"].ToString();
                performers.Add(p);
            }
            return performers;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets all genres. Not sorted.
    public List<Genre> GetAllGenres()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetAllGenres", con, null);             // create the command


        List<Genre> genres = new List<Genre>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dataReader.Read())
            {
                genres.Add(new Genre(Convert.ToInt32(dataReader["GenreID"]), dataReader["GenreName"].ToString()));
            }
            return genres;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Adds a song without the mp3 file data.
    public int PostSongDataWithoutFile(Song song)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        int SongID = GetSongScopeIdentity() + 1;
        paramDic.Add("@SongID", SongID);
        paramDic.Add("@SongName", song.Name);
        paramDic.Add("@SongLyrics", song.Lyrics);
        paramDic.Add("@GenreID", song.GenreID);
        paramDic.Add("@ReleaseYear", song.ReleaseYear);
        paramDic.Add("@PerformerID", song.PerformerID);
        paramDic.Add("@SongLength", song.Length);



        cmd = CreateCommandWithStoredProcedure("Proj_SP_PostSongDataWithoutFile", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return SongID;
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    // Gets the scope identity of the Songs table. Used to insert get the new SongID to insert.
    private int GetSongScopeIdentity()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("FinalProject"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("Proj_SP_GetCurrentSongIdentity", con, null);             // create the command



        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                return Convert.ToInt32(dataReader["CurrentID"]);
            }
            throw new Exception("DB ERROR");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
}