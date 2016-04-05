using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace MovieInfo
{
    /*
    *   Class: MovieList
    *       Constructors:
    *           public MovieList(string path, System.Windows.Controls.ListBox listbox)
    *               - Initializes ListBox with parsed names of media files found from a given path
    *       Functions:
    *           private List<string> DirSearch(string sDir)
    *               - Returns a list of found files that matches given file extension.
    */

    public class MovieList : System.Windows.Forms.ListBox
    {
        private List<string> _filenames = new List<string>();

        #region properties
        public List<String> movies
        {
            get { return _filenames; }
        }
        #endregion

        #region constructors
        // Initialize ListBox with files from user defined path
        public MovieList(string path)
        {
            List<string> items = DirSearch(path);

            foreach (string item in items)
            {
                string filename = Path.GetFileNameWithoutExtension(item);
                var sb = new StringBuilder();

                // Parse filename, punctuations will be replaced with whitespace
                foreach (char c in filename)
                {
                    if (!char.IsPunctuation(c))
                        sb.Append(c);
                    else
                        sb.Append(" ");
                }

                //System.Console.WriteLine(sb);
                _filenames.Add(sb.ToString());
            }
        }
        #endregion

        #region methods
        /* 
        * DirSearch(string sDir)
        * Checks given path for files that have particular extension
        * Found files are added to List<string> and returned
        */
        private List<string> DirSearch(string sDir)
        {
            List<string> mediaExtensions = new List<string> { ".avi", ".mp4", ".iso", ".img" };
            List<string> filesFound = new List<string>();

            // For every sub directory..
            foreach (string d in Directory.GetDirectories(sDir))
            {
                foreach(string g in Directory.GetFiles(sDir, "*.*"))
                {
                    // If file extension is..
                    if (mediaExtensions.Contains(Path.GetExtension(g).ToLower()))
                    {
                        filesFound.Add(g);
                    }
                } 
                // For every file in directory..
                foreach (string f in Directory.GetFiles(d, "*.*"))
                {
                    // If file extension is..
                    if (mediaExtensions.Contains(Path.GetExtension(f).ToLower()))
                    {
                        filesFound.Add(f);
                        //System.Console.WriteLine(f);
                    }
                }
                DirSearch(d);
            }
            return filesFound;
        }
        #endregion
    }
}