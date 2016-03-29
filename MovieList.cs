// The following example displays an application that provides the ability to 
// open rich text files (rtf) into the RichTextBox. The example demonstrates 
// using the FolderBrowserDialog to set the default directory for opening files.
// The OpenFileDialog class is used to open the file.
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Text;

public class MovieList : System.Windows.Forms.ListBox
{

    // Initialize ListBox with files from user defined path
    public MovieList(string path, System.Windows.Controls.ListBox listbox)
    {
        List<string> items = DirSearch(path);
        List<string> filenames = new List<string>();
        foreach (string item in items)
        {
            string filename = Path.GetFileNameWithoutExtension(item);
            var sb = new StringBuilder();
            foreach (char c in filename)
            {
                if (!char.IsPunctuation(c))
                    sb.Append(c);
                else
                    sb.Append(" ");
            }
            //System.Console.WriteLine(sb);
            filenames.Add(sb.ToString());
        }
        listbox.ItemsSource = filenames;
    }

    /* 
    * DirSearch(string sDir)
    * Checks given path for files that have particular extension
    * Found files are added to List<string> and returned
    */
    private List<string> DirSearch(string sDir)
    {
        List<string> mediaExtensions = new List<string> { ".avi", ".mp4" };
        List<string> filesFound = new List<string>();
        foreach (string d in Directory.GetDirectories(sDir))
        {
            foreach (string f in Directory.GetFiles(d, "*.*"))
            {
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

}