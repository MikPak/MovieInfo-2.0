using System;
using System.Windows;
using System.Windows.Forms;

namespace MovieInfo
{
    /*
    *   MovieInfo - Simple movie library tool
    *       - Searches media files from user specified path
    *       - Parses file names
    *       - Searches for data from The Open Movie Database (omdbapi.com)
    *       - Parses XML-response and views info in graphical user interface
    *       - Keeps record of data in local MySQL-database
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

        public MainWindow()
        {
            InitializeComponent();
            AddMainMenu();
        }

        private void AddMainMenu()
        {
            // Make the main menu.
            System.Windows.Controls.Menu mainMenu = new System.Windows.Controls.Menu();
            MovieInfoGrid.Children.Add(mainMenu);
            mainMenu.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            mainMenu.VerticalAlignment = System.Windows.VerticalAlignment.Top;

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

        private void openMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Create instance of FolderBrowserDialog
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();

            // Set the help text description for the FolderBrowserDialog.
            this.folderBrowserDialog1.Description =
                "Select directory containing movies";

            // Do not allow the user to create new files via the FolderBrowserDialog.
            this.folderBrowserDialog1.ShowNewFolderButton = false;

            // Default to the Desktop folder.
            this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;

            // Display the openFile dialog.
            DialogResult result = folderBrowserDialog1.ShowDialog();

            // OK button was pressed.
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    //System.Windows.MessageBox.Show(openFileName);
                    openFileName = folderBrowserDialog1.SelectedPath;
                    fileOpened = true;
                    MovieList ml = new MovieList(openFileName, movieListBox);
                    MovieData md = new MovieData(ml.movies);
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
            System.Console.WriteLine("asd");
        }
    }
}
