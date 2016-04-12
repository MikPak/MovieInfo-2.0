using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Data.SQLite;
using System.Collections.Generic;

namespace MovieInfo
{
    /*
    *   MovieInfo - Simple movie library tool
    *       - Searches media files from user specified path
    *       - Parses file names
    *       - Searches for data from The Open Movie Database (omdbapi.com)
    *       - Parses XML-response and views info in graphical user interface
    *       - Keeps record of data in local SQLite-database
    *
    *       GitHub: https://github.com/MikPak/MovieInfo-2.0
    *       Project Started: 20.3.2016
    *       Author: Mikko Pakkanen
    *       Website: http://mikkopakkanen.com
    */
    public partial class MainWindow : Window
    {
        private FolderBrowserDialog folderBrowserDialog1;
        private bool fileOpened = false;
        private string openFileName;
        private SQLiteConnection dbConnection;
        private List<MovieData> movies = new List<MovieData>();
        private string dbName = "movieInfo.sqlite";

        #region properties
        private MovieData Data { get; set; }
        private SQLiteConnection db { get { return dbConnection; } set { dbConnection = value; } }
        #endregion

        #region constructors
        public MainWindow()
        {
            InitializeComponent();
            AddMainMenu();
            initDB();
        }
        #endregion

        #region methods
        private void AddMainMenu()
        {
            // Make the main menu.
            System.Windows.Controls.Menu mainMenu = new System.Windows.Controls.Menu();
            MovieInfoGrid.Children.Add(mainMenu);
            mainMenu.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            mainMenu.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            mainMenu.Height = 20;
            // Make the File menu.
            System.Windows.Controls.MenuItem fileMenuItem = new System.Windows.Controls.MenuItem();
            fileMenuItem.Header = "_File";
            mainMenu.Items.Add(fileMenuItem);
            // Make the File menu items.
            System.Windows.Controls.MenuItem openMenuItem = new System.Windows.Controls.MenuItem();
            fileMenuItem.Items.Add(openMenuItem);
            openMenuItem.Header = "_Select Folder";
            openMenuItem.Click += openMenuItem_Click;
            System.Windows.Controls.ToolTip openToolTip = new System.Windows.Controls.ToolTip();
            openMenuItem.ToolTip = openToolTip;
            openToolTip.Content = "Select folder containing movies";

            System.Windows.Controls.MenuItem exitMenuItem = new System.Windows.Controls.MenuItem();
            fileMenuItem.Items.Add(exitMenuItem);
            exitMenuItem.Header = "_Exit";
            exitMenuItem.Click += exitMenuItem_Click;
            System.Windows.Controls.ToolTip exitToolTip = new System.Windows.Controls.ToolTip();
            exitMenuItem.ToolTip = exitToolTip;
            exitToolTip.Content = "End the program";
        }

        private void initDB()
        {
            if (!File.Exists(dbName))
            {
                SQLiteConnection.CreateFile(dbName);
                this.db = new SQLiteConnection("Data Source=" + dbName + "; Version=3;");
                this.db.Open();

                string sql = "CREATE TABLE movieInfo (title varchar(100), year int, rated varchar(20), released TIMESTAMP, runtime varchar(20), genre varchar(150), director varchar(150), writer varchar(250), actors varchar(250), plot varchar(300), language varchar(50), country varchar(50), awards varchar(150), poster varchar(300), metascore varchar(20), imdbrating varchar(20), imdbvotes varchar(20), imdbid varchar(20), type varchar(20), response varchar(20), parsedName varchar(50))";
                SQLiteCommand command = new SQLiteCommand(sql, this.db);
                command.ExecuteNonQuery();
            }
            else
            {
                this.db = new SQLiteConnection("Data Source=" + dbName + "; Version=3;");
                this.db.Open();

                // Initialize ListBox with movies found from local DB
                string selectSQL = "SELECT * FROM movieInfo";
                SQLiteCommand command = new SQLiteCommand(selectSQL, db);
                try
                {
                    using (SQLiteDataReader rdr = command.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            defaultViewLabel.Visibility = Visibility.Hidden; // Hide label at the middle of content
                            MovieData md = new MovieData();
                            md.Title = rdr["title"].ToString();
                            md.Year = rdr["year"].ToString();
                            md.Rated = rdr["rated"].ToString();
                            md.Runtime = rdr["runtime"].ToString();
                            md.Genre = rdr["genre"].ToString();
                            md.Director = rdr["director"].ToString();
                            md.Writer = rdr["writer"].ToString();
                            md.Actors = rdr["actors"].ToString();
                            md.Plot = rdr["plot"].ToString();
                            md.Language = rdr["language"].ToString();
                            md.Country = rdr["country"].ToString();
                            md.Awards = rdr["awards"].ToString();
                            md.Poster = rdr["poster"].ToString();
                            md.Metascore = rdr["metascore"].ToString();
                            md.imdbRating = rdr["imdbrating"].ToString();
                            md.imdbVotes = rdr["imdbvotes"].ToString();
                            md.imdbID = rdr["imdbid"].ToString();
                            md.Type = rdr["type"].ToString();
                            md.Response = rdr["response"].ToString();

                            movies.Add(md);
                            movieListBox.Items.Add(md);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void openMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Create instance of FolderBrowserDialog
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            // Set the help text description for the FolderBrowserDialog
            this.folderBrowserDialog1.Description =
                "Select directory containing movies";
            // Do not allow the user to create new files via the FolderBrowserDialog
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // Default to the Desktop folder
            this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
            // Display the openFile dialog
            DialogResult result = folderBrowserDialog1.ShowDialog();

            // OK button was pressed.
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Make a search with parsed file names to OMDB-Database
                // Returned object MovieData data contains property List<MovieData> Movies,
                // which contains List of MovieData-objects serialized from JSON-response from OMDB
                try
                {
                    defaultViewLabel.Visibility = Visibility.Hidden; // Hide label at the middle of content
                    //System.Windows.MessageBox.Show(openFileName);
                    openFileName = folderBrowserDialog1.SelectedPath;
                    fileOpened = true;
                    MovieList ml = new MovieList(openFileName);
                    MovieData data = new MovieData(ml.movies, movieListBox, db, movies);
                }
                catch (Exception exp)
                {
                    System.Windows.MessageBox.Show("An error occurred while attempting to load the file. The error is:"
                                    + System.Environment.NewLine + exp.ToString() + System.Environment.NewLine);
                    fileOpened = false;
                }
            }

            // Cancel button was pressed.
            else if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void movieListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            MovieData md = (MovieData)movieListBox.SelectedItem;
            movieNameLabel.Content = md.Title;
            movieReleasedLabel.Content = md.Released + " (" + md.Runtime + ")";
            movieGenreLabel.Content = md.Genre;
            MoviePlotLabel.Text = md.Plot;
            moviePoster.Source = new BitmapImage(new Uri(md.Poster, UriKind.Absolute));
            movieDirectorLabel.Text = "Director: " + md.Director;
            movieWritersLabel.Text = "Writers: " + md.Writer;
            movieStarsLabel.Text = "Actors: " + md.Actors;

            //Console.Write(md.Poster);

            /*
            foreach (MovieData md in this.Data.Movies)
            {
                Console.Write(md.Title);
            }
            */
        }
        #endregion
    }
}
