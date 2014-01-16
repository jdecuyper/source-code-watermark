using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;

namespace SourceCodeWaterMark
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkMagenta;

            Console.WriteLine("*****************************************************");
            Console.WriteLine("*                                                   *");
            Console.WriteLine("*            Add watermark to source code           *");
            Console.WriteLine("*                                                   *");
            Console.WriteLine("*  This program will add a release number           *");
            Console.WriteLine("*  at the top of all files inside a provided folder *");
            Console.WriteLine("*  location.                                        *");
            Console.WriteLine("*                                                   *");
            Console.WriteLine("*  The process will only update the file with       *");
            Console.WriteLine("*  the extensions contained inside the following    *");
            Console.WriteLine("*  file: Extensions.txt. The comment symbols can    *");
            Console.WriteLine("*  also be edited inside that file.                 *");
            Console.WriteLine("*                                                   *"); 
            Console.WriteLine("*  The watermark format can be edited inside        *");
            Console.WriteLine("*  Watermark.txt                                    *");
            Console.WriteLine("*                                                   *"); 
            Console.WriteLine("*  Changes cannot be reverted.                      *");
            Console.WriteLine("*                                                   *");
            Console.WriteLine("*  Author: https://github.com/jdecuyper             *");
            Console.WriteLine("*  Date: 2014-01-09                                 *");
            Console.WriteLine("*                                                   *"); 
            Console.WriteLine("*****************************************************");

            Console.Write("\n\n");

            // add a couple of new lines to separate the banner
            // from the rest of the text
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            bool releaseNumberIsValid = false;
            string releaseNumber = String.Empty;

            while (!releaseNumberIsValid) {
                Console.ForegroundColor = ConsoleColor.White; 
                Console.Write("# Provide release number: ");
                releaseNumber = Console.ReadLine();

                Match releaseNumberMatch = Regex.Match(releaseNumber, @"([0-9]+[\.]?)+", RegexOptions.IgnoreCase);
                
                if (String.IsNullOrEmpty(releaseNumber))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Release number is invalid.");
                    Console.WriteLine("Use a valid format (1, 1.2, 1.3.0,...)");
                    continue;
                }

                if (!releaseNumberMatch.Success)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Release number is invalid.");
                    Console.WriteLine("Use a valid format (1, 1.2, 1.3.0,...)");
                    continue;
                }

                releaseNumberIsValid = true;
            }

            bool folderExists = false;
            string folderPath = String.Empty;

            while (!folderExists)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("# Provide folder path: ");
                folderPath = Console.ReadLine();

                if (String.IsNullOrEmpty(folderPath))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Folder path number is invalid.");
                    Console.WriteLine("Folder path can be a relative or absolute (C:\\foo or 1\\bar)");
                    continue;
                }

                if (!Directory.Exists(folderPath))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Folder path is invalid.");
                    Console.WriteLine("Folder path can be a relative or absolute (C:\\foo or 1\\bar)");
                    continue;
                }

                folderExists = true;
            }
            
            // what if the release number already exists inside a file?
            // what if some file raises an error when reading or writing?

            Console.WriteLine(String.Empty);
            Console.WriteLine("# Start to watermark files with release v." + releaseNumber);
            Console.WriteLine("# Read valid extension list");
            FileExtensionAndCodeSymbol fileSettings = new FileExtensionAndCodeSymbol();
            Dictionary<string, Tuple<string, string>> ext = fileSettings.Extensions;

            foreach (KeyValuePair<string, Tuple<string, string>> kvp in ext) {
                Console.WriteLine(kvp.Key + " - " + kvp.Value.Item1 + " - " + kvp.Value.Item2);
            }

            Console.Write("# Press any key to exit...");
            Console.ReadLine();            
        }
    }
}
