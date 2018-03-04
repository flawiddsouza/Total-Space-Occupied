using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace TotalSpaceOccupied
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string TextFileName = System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".txt";

        public MainWindow()
        {
            InitializeComponent();

            PathsTextBox.TextChanged += PathsTextBox_TextChanged;

            PathsTextBox.Text = Helpers.TextFileToString(TextFileName); // read input from text file
        }

        private void PathsTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Helpers.StringToTextFile(PathsTextBox.Text, TextFileName); // save input to text file

            if (!String.IsNullOrEmpty(PathsTextBox.Text))
            {
                string[] paths = Regex.Split(PathsTextBox.Text, "\r\n");
                long size = 0;
                foreach (string path in paths)
                {
                    if (Directory.Exists(path))
                    {
                        Task<long> task = Task.Factory.StartNew(() => Helpers.GetDirectorySize(new DirectoryInfo(path)));
                        task.ContinueWith(t =>
                        {
                            size += (long)t.Result;
                            TotalSpaceOccupied.Text = Helpers.StrFormatByteSize(size);
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                    else
                    {
                        TotalSpaceOccupied.Text = "0 bytes";
                    }
                }
            }
            else
            {
                TotalSpaceOccupied.Text = "0 bytes";
            }
        }
    }
}