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
    *               - Properties are matching JSON-response
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
        #endregion

        #region constructors
        /* 
        * public MovieData(List<string> filenames)
        * - Makes HTTP-requests to OMDB, given List should contain List of parsed movie names
        * - Initializes default view when starting the app.
        * - Converts JSON-response to .NET-object (http://www.newtonsoft.com/json/help/html/SerializingJSON.htm)
        */
        public MovieData(List<string> filenames)
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
                    //Console.WriteLine(((HttpWebResponse)response).StatusDescription); // Display HTTP-status returned by server.
                    Stream dataStream = response.GetResponseStream(); // Get the stream containing content returned by the server.
                    StreamReader reader = new StreamReader(dataStream); // Open the stream using a StreamReader for easy access.
                    string responseFromServer = reader.ReadToEnd(); // Read the content.
                    //Console.WriteLine(responseFromServer); // Display the content.
                    MovieData mv = new MovieData();
                    mv = JsonConvert.DeserializeObject<MovieData>(responseFromServer);
                    //Console.WriteLine(mv.Title);
                }
            }
        }

        public MovieData()
        {

        }
        #endregion
        }
}
