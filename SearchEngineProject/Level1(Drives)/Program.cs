using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SearchEngine_ConsoleApp_
{
    class Program
    {
        /*public string[] Histoy { get; set; }
        private string term;

        public string Term
        {
            get { return term; }
            set { term = value; }


        }

        private string dir;

        public string Dir
        {
            get { return dir; }
            set { dir = value; }
        }*/
        static StreamWriter sw { get; set; }
        static void Main(string[] args)
        {
            //Level 1
            int c = 0;
            DriveInfo[] TotalDrives = DriveInfo.GetDrives();
            foreach (DriveInfo drvinfo in TotalDrives)
            {
                Console.WriteLine("Drive{0}", drvinfo.Name);
                Console.WriteLine("  Drive type: {0}", drvinfo.DriveType);
                if (drvinfo.IsReady == true)
                {
                    Console.WriteLine("Drive Is Active");
                }
                else
                    Console.WriteLine("Drive Is Inactive");
                c++;
            }
            Console.WriteLine("Total no of Drives=" + c);
            Console.WriteLine("Enter the filename");
            string filename = Console.ReadLine();
            Console.WriteLine(filename);

            string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            path = Path.Combine(path, "SearchHistory.txt");
            FileStream fileStream = null;
            if (File.Exists(path))
            {
                fileStream = new FileStream(path, FileMode.Append, FileAccess.ReadWrite);
            }
            else
            {
                fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
            }
            //Level 2
            /*foreach (DriveInfo d in TotalDrives)
            {
                if(d.IsReady == true)
                {
                    string b = d.Name.ToString();
                    GetAllFiles(b, filename);
                }
            }*/
            Dictionary<string, List<string>> filedict = new Dictionary<string, List<string>>();
            //file read
            if(!filedict.ContainsKey(filename))  
                filedict.Add(filename, new List<string>());

            sw = new StreamWriter(fileStream);
            //Level 3
            Parallel.ForEach(TotalDrives, d =>
            {
                if (d.IsReady == true)
                {
                    string b = d.Name.ToString();
                    GetAllFiles(b, filename);
                }
            });
            sw.Close();
            sw.Dispose();
            //file write
        Console.ReadLine();
        }

        //Function used in Level 2
        /*private static void GetAllFiles(string sDir, string filename)
        {
            try
            {
                List<string> allfiles_Folders = new List<string>();
                allfiles_Folders.AddRange(Directory.GetFiles(sDir));
                allfiles_Folders.AddRange(Directory.GetDirectories(sDir));
                forEach(string s in allfiles_Folders)
                {
                    string _s = s.ToLower();
                    string _filename = filename.ToLower();
                    if(Directory.Exists(s) && s!= "." && s != "..")
                    {
                        GetAllFiles(s, filename);
                    }
                    if (_s.Contains(_filename))
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            catch
            {

            }
        }*/
        private static void GetAllFiles(string sDir, string filename)
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
                    if(Directory.Exists(s) && s!= "." && s != "..")
                    {
                        GetAllFiles(s, filename);
                    }
                    if (_s.Contains(_filename))
                    {
                        Console.WriteLine(s);
                        sw.WriteLine(s);
                    }
                });
            }
            catch
            {

            }
        }

       
    }
}
