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
            foreach (DriveInfo d in TotalDrives)
            {
                if(d.IsReady == true)
                {
                    string b = d.Name.ToString();
                    GetAllFiles(b, filename);
                }
            }
        Console.ReadLine();
        }
        
        private static void GetAllFiles(string sDir, string filename)
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
        }
    }
}
