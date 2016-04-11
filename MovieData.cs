using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Data.SQLite;

namespace MovieInfo
{
    /*
    *   Class: MovieData
    *       - Main purpose is to store data from OMDB
    *       Constructors:
    *           public MovieData(List<string> filenames)
    *               - Makes HTTP-requests to OMDB, parameter should contain List of parsed movie names
    *               - Initializes default view when starting the app.
    *               - Properties of objects are matching data of JSON-response
    *               - Populates public List<MovieData> Movies with MovieData-objects that are serialized from JSON-response
    *       Functions:
    *           ---
    *               ---
    */
    public class MovieData
    {
        #region properties
        public string Title { get; set; }
        public string Year { get; set; }
        public string Rated { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public string Poster { get; set; }
        public string Metascore { get; set; }
        public string imdbRating { get; set; }
        public string imdbVotes { get; set; }
        public string imdbID { get; set; }
        public string Type { get; set; }
        public string Response { get; set; }

        public List<MovieData> Movies = new List<MovieData>();
        #endregion
        #region constructors
        /* 
        * public MovieData(List<string> filenames)
        * - Makes HTTP-requests to OMDB, given List should contain List of parsed movie names
        * - Initializes default view when starting the app.
        * - Converts JSON-response to .NET-object (http://www.newtonsoft.com/json/help/html/SerializingJSON.htm)
        */
        public MovieData(List<string> filenames, System.Windows.Controls.ListBox listbox, SQLiteConnection db, List<MovieData> movies)
        {
            string url = "http://www.omdbapi.com/?t="; // See www.omdbapi.com/#parameters for full variable definition list
            //int i = 0;
            foreach (string movie in filenames)
            {
                //System.Console.WriteLine("#" + i + " " + movie);
                //i++;
                // First we want to check if there are rows in local database that already contain file with same parsed name..
                string selectSQL = "SELECT * FROM movieInfo WHERE parsedName = '" + movie + "'";
                SQLiteCommand command = new SQLiteCommand(selectSQL, db);
                try
                {
                    using(SQLiteDataReader rdr = command.ExecuteReader())
                    {
                        // If no movie found locally, fetch it's data from OMBD..
                        if (!rdr.Read())
                        {
                            string request = url + WebUtility.UrlEncode(movie);
                            WebRequest webRequest = WebRequest.Create(request);
                            webRequest.Method = WebRequestMethods.Http.Get;
                            webRequest.ContentType = "application/x-www-form-urlencoded";
                            webRequest.Proxy = null;

                            using (var response = webRequest.GetResponse())
                            {
                                Stream dataStream = response.GetResponseStream(); // Get the stream containing content returned by the server.
                                StreamReader reader = new StreamReader(dataStream); // Open the stream using a StreamReader for easy access.
                                string responseFromServer = reader.ReadToEnd(); // Read the content.
                                string errorStr = @"{""Response"":""False"",""Error"":""Movie not found!""}";

                                //Console.WriteLine(((HttpWebResponse)response).StatusDescription); // Display HTTP-status returned by server.
                                //Console.WriteLine(errorStr);
                                //Console.WriteLine(responseFromServer); // Display the content.

                                // If movie found, serialize it and add to property and database
                                if (!responseFromServer.Equals(errorStr))
                                {
                                    MovieData mv = new MovieData();
                                    mv = JsonConvert.DeserializeObject<MovieData>(responseFromServer);
                                    //Movies.Add(mv);
                                    movies.Add(mv); // Add to List<MovieData>

                                    // Add to Database
                                    string insertSQL = "INSERT INTO movieInfo (title, year, rated, released, runtime, genre, director, writer, actors, plot, language, country, awards, poster, metascore, imdbrating, imdbvotes, imdbid, type, response, parsedName) values (@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10,@param11,@param12,@param13,@param14,@param15,@param16,@param17,@param18,@param19,@param20,@param21)";
                                    Console.WriteLine(insertSQL);
                                    command = new SQLiteCommand(insertSQL, db);

                                    // Add parameters
                                    command.Parameters.Add(new SQLiteParameter("@param1", mv.Title));
                                    command.Parameters.Add(new SQLiteParameter("@param2", mv.Year));
                                    command.Parameters.Add(new SQLiteParameter("@param3", mv.Rated));
                                    command.Parameters.Add(new SQLiteParameter("@param4", mv.Released));
                                    command.Parameters.Add(new SQLiteParameter("@param5", mv.Runtime));
                                    command.Parameters.Add(new SQLiteParameter("@param6", mv.Genre));
                                    command.Parameters.Add(new SQLiteParameter("@param7", mv.Director));
                                    command.Parameters.Add(new SQLiteParameter("@param8", mv.Writer));
                                    command.Parameters.Add(new SQLiteParameter("@param9", mv.Actors));
                                    command.Parameters.Add(new SQLiteParameter("@param10", mv.Plot));
                                    command.Parameters.Add(new SQLiteParameter("@param11", mv.Language));
                                    command.Parameters.Add(new SQLiteParameter("@param12", mv.Country));
                                    command.Parameters.Add(new SQLiteParameter("@param13", mv.Awards));
                                    command.Parameters.Add(new SQLiteParameter("@param14", mv.Poster));
                                    command.Parameters.Add(new SQLiteParameter("@param15", mv.Metascore));
                                    command.Parameters.Add(new SQLiteParameter("@param16", mv.imdbRating));
                                    command.Parameters.Add(new SQLiteParameter("@param17", mv.imdbVotes));
                                    command.Parameters.Add(new SQLiteParameter("@param18", mv.imdbID));
                                    command.Parameters.Add(new SQLiteParameter("@param19", mv.Type));
                                    command.Parameters.Add(new SQLiteParameter("@param20", mv.Response));
                                    command.Parameters.Add(new SQLiteParameter("@param21", movie));

                                    // Execute
                                    try
                                    {
                                        command.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {

                                        throw ex;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            listbox.Items.Clear();
            listbox.ItemsSource = movies; // Bind List<MovieData> as ItemsSource for our ListBox
            db.Close(); // Close database connection
        }

        public MovieData()
        {

        }
        #endregion
        }
}
