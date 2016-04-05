using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

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
        public MovieData(List<string> filenames, System.Windows.Controls.ListBox listbox)
        {
            string url = "http://www.omdbapi.com/?t="; // See www.omdbapi.com/#parameters for full variable definition list
            foreach (string movie in filenames)
            {
                string request = url + WebUtility.UrlEncode(movie);
                //System.Console.WriteLine(request);

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

                    Console.WriteLine(((HttpWebResponse)response).StatusDescription); // Display HTTP-status returned by server.
                    //Console.WriteLine(errorStr);

                    // If movie found, serialize it and add to property.
                    if (!responseFromServer.Equals(errorStr)) {
                        MovieData mv = new MovieData();
                        Console.WriteLine(responseFromServer); // Display the content.
                        mv = JsonConvert.DeserializeObject<MovieData>(responseFromServer);
                        Movies.Add(mv);
                    }
                }
            }
            listbox.ItemsSource = this.Movies; // Bind Movies-property as ItemsSource for our ListBox
        }

        public MovieData()
        {

        }
        #endregion
        }
}
