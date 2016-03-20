// The following example displays an application that provides the ability to 
// open rich text files (rtf) into the RichTextBox. The example demonstrates 
// using the FolderBrowserDialog to set the default directory for opening files.
// The OpenFileDialog class is used to open the file.
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

public class MovieList : System.Windows.Forms.ListBox
{
    public MovieList(string path, System.Windows.Controls.ListBox listbox)
    {
        List<string> _items = new List<string>(); // <-- Add this
        _items.Add("One"); // <-- Add these
        _items.Add("Two");
        _items.Add("Three");
        listbox.ItemsSource = _items;
    }
}