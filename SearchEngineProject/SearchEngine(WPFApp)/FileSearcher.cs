using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SearchEngine_WPFApp_
{
    delegate void FileFound(string path);

    class FileSearcher
    {
        private string dir;
        private string name;
        public string[] History { get; set; }

        public event FileFound OnFileFound;
        public FileSearcher(string dir, string filename)
        {
            this.dir = dir;
            this.name = filename;
        }
        public string Dir {
            get
            {
                return dir;
            }
            set
            {
                dir = value;
            }
        }
        public string Filename
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        private void GetAllFiles(string sDir, string filename)
        {
            try
            {
                List<string> allfiles_Folders = new List<string>();
                allfiles_Folders.AddRange(Directory.GetFiles(sDir));
                allfiles_Folders.AddRange(Directory.GetDirectories(sDir));
                Parallel.ForEach(allfiles_Folders, s =>
                {
                    string _s = s.ToLower();
                    string _filename = filename.ToLower();
                    if (Directory.Exists(s) && s != "." && s != "..")
                    {
                        GetAllFiles(s, filename);
                    }
                    if (_s.Contains(_filename))
                    {
                        OnFileFound(s);
                    }
                });
            }
            catch
            {

            }
        }

        public void Search()
        {
            DriveInfo[] TotalDrives = DriveInfo.GetDrives();
            Parallel.ForEach(TotalDrives, d =>
            {
                if (d.IsReady == true)
                {
                    Dir = d.Name.ToString();
                    GetAllFiles(Dir, Filename);
                }
            });
        }

        public void WriteHistory()
        {

            string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            string filename = @"SearchHistory.txt";
            if (File.Exists(Path.Combine(path, filename)))
            {
                File.Delete(Path.Combine(path, filename));
            }
            File.WriteAllLines(Path.Combine(path, filename), History);
        }

        public bool IsHistory()
        {
            if (History == null)
            {
                return false;
            }
            foreach (var file in History)
            {
                //if (file.ToLower().Contains(Path.Combine(dir.ToLower(),term.ToLower())))
                if (file.ToLower().Contains(Filename.ToLower()))
                {
                    return true;
                }

            }
            return false;

        }

        public void ReadHistory()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            string filename = @"SearchHistory.txt";
            if (File.Exists(Path.Combine(path, filename)))
            {
                var lineCount = File.ReadLines(Path.Combine(path, filename));
                History = lineCount.ToArray();

            }

        }
    }
}
