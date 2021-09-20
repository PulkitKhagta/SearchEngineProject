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

            //Level 2
            /*foreach (DriveInfo d in TotalDrives)
            {
                if(d.IsReady == true)
                {
                    string b = d.Name.ToString();
                    GetAllFiles(b, filename);
                }
            }*/
            //Dictionary<string, List<string>> filepaths = Dictionary<string, List<string>>();
            //Level 3
            Parallel.ForEach(TotalDrives, d =>
            {
                if (d.IsReady == true)
                {
                    string b = d.Name.ToString();
                    GetAllFiles(b, filename);
                }
            });
        Console.ReadLine();
        }

        //Function used in Level 2
        /*private static void GetAllFiles(string sDir, string filename)
        {
            foreach(string dir in Directory.GetDirectories(sDir))
            {
                try
                {
                    foreach (string file in Directory.GetFiles(dir, filename))
                    {
                        string filePath = Path.GetFullPath(file);
                        Console.WriteLine(filePath);
                    }
                    //Recursive Search
                    GetAllFiles(dir, filename);
                }
                catch
                {
                    
                }
            }
        }*/

        //Function for Level 3 
        /*private static void GetAllFiles(string sDir, string filename)
        {
            Parallel.ForEach(Directory.GetDirectories(sDir), dir =>
            {
                try
                {
                    foreach (string file in Directory.GetFiles(dir, filename.Contains(".*") ? filename : filename+".*"))
                    {
                        string filePath = Path.GetFullPath(file);
                        Console.WriteLine(filePath);
                    }
                    //Recursive Search
                    GetAllFiles(dir, filename);
                }
                catch(Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                }
            });
        }*/

        private static void GetAllFiles(string sDir, string filename)
        {
            Parallel.ForEach(Directory.GetDirectories(sDir), dir =>
            {
                try
                {
                    Parallel.ForEach(Directory.GetFiles(dir, filename.Contains(".*") ? filename : filename + ".*"), file =>
                    {
                        string filePath = Path.GetFullPath(file);
                        Console.WriteLine(filePath);
                        //filepaths.Add(filePath);
                    });
                    
                    //Recursive Search
                    GetAllFiles(dir, filename);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                }
            });
        }
    }
}
