using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;

namespace SearchEngine_WPFApp_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker bgworker = new BackgroundWorker();
        private FileSearcher searcher;
        public MainWindow()
        {
            InitializeComponent();
            this.searcher = new FileSearcher(null, null);
            this.searcher.OnFileFound += FileFound;
            bgworker.DoWork += WorkInBackground;
            bgworker.RunWorkerCompleted += WorkerCompleted;
        }

        private void FileFound(string path)
        {
            lbFileFound.Dispatcher.BeginInvoke((Action)delegate()
            {
                lbFileFound.Items.Add(path);
            });
        }

        private void WorkInBackground(object sender, DoWorkEventArgs args)
        {
            searcher.Search();
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            if (lbFileFound.Items.Count == 0)
            {
                MessageBox.Show("Not Found !!");
                lbFileFound.Items.CopyTo(searcher.History, 0);
                return;
            }
            searcher.History = new string[lbFileFound.Items.Count];
            lbFileFound.Items.CopyTo(searcher.History, 0);
            searcher.WriteHistory();
            MessageBox.Show("Done!");
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //int c = 0;
            DriveInfo[] TotalDrives = DriveInfo.GetDrives();
            if (bgworker.IsBusy)
            {
                bgworker.CancelAsync();
                MessageBox.Show("Busy");
                return;
            }

            this.searcher.Filename = Filename.Text.ToLower();

            lbDrives.Items.Clear();
            foreach (DriveInfo drvinfo in TotalDrives)
            {
                if (drvinfo.IsReady == true)
                {
                    lbDrives.Items.Add($"Drive{drvinfo.Name} : Active");
                }
                else
                    lbDrives.Items.Add($"Drive{drvinfo.Name} : Inactive");
                //c++;
            }
            this.searcher.ReadHistory();
            if (bgworker.IsBusy)
            {
                bgworker.CancelAsync();
                MessageBox.Show("Busy");
                return;
            }

            if (searcher.IsHistory() && searcher.Filename != "")
            {
                lbFileFound.Items.Clear();
                foreach (var path in searcher.History)
                    if (path.ToLower().Contains(searcher.Filename.ToLower()))
                    {
                        lbFileFound.Items.Add(path);
                    }

                MessageBox.Show("History Found!!");
                return;
            }

            lbFileFound.Items.Clear();
            bgworker.RunWorkerAsync();
        }
    }
}
