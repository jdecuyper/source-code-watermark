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
            Console.WriteLine("*  file: CodeCommentsSymbols.txt. The comment       *");
            Console.WriteLine("*  symbols can also be edited inside that same      *");
            Console.WriteLine("*  file.                                            *");
            Console.WriteLine("*                                                   *"); 
            Console.WriteLine("*  The watermark format can be edited inside        *");
            Console.WriteLine("*  Watermark.txt                                    *");
            Console.WriteLine("*                                                   *"); 
            Console.WriteLine("*                                                   *");
            Console.WriteLine("*  Author: https://github.com/jdecuyper             *");
            Console.WriteLine("*  Date: 2014-01-09                                 *");
            Console.WriteLine("*                                                   *"); 
            Console.WriteLine("*****************************************************");
            Console.Write("\n\n");

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            // Ask for release number
            bool releaseNumberIsValid = false;
            string releaseNumber = String.Empty;

            while (!releaseNumberIsValid) {
                Console.ForegroundColor = ConsoleColor.White; 
                Console.Write("# Provide release number: ");
                releaseNumber = Console.ReadLine();

                Match releaseNumberMatch = Regex.Match(releaseNumber, @"\d+(?:\.\d+)+", RegexOptions.IgnoreCase);
                
                if (String.IsNullOrEmpty(releaseNumber))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Release number is invalid.");
                    Console.WriteLine("Use a valid format (1.0, 7.45.3,...)");
                    continue;
                }

                if (!releaseNumberMatch.Success)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Release number is invalid.");
                    Console.WriteLine("Use a valid format (1.0, 7.45.3,...)");
                    continue;
                }

                releaseNumberIsValid = true;
            }

            // Ask for folder name
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

            // Start watermarking process
            Console.WriteLine(String.Empty);
            Console.WriteLine("# Start to watermark files with release v." + releaseNumber);
            Console.WriteLine("# Read valid extension list");
            
            string fileSettingsAbsPath = Environment.CurrentDirectory + "\\" + CodeCommentSymbols.SETTINGS_FILE_NAME;
            CodeCommentSymbols codeComments = new CodeCommentSymbols(fileSettingsAbsPath);
            
            Console.WriteLine("# Read watermark");
            Watermark mark = new Watermark(Environment.CurrentDirectory + "\\" + Watermark.WATERMARK_FILE_NAME, releaseNumber);

            if (mark.TextLines.Count > 0)
            {
                FolderToWatermark folderToProcess = new FolderToWatermark(folderPath, codeComments, mark);
                Console.WriteLine("# Files to process: " + folderToProcess.FilesToProcessCount);

                Console.WriteLine("# Start watermarking files...");
                folderToProcess.WaterMarkFiles();
                Console.WriteLine("# Watermarking has finished");

                // Print result
                Console.WriteLine("# Threads used to process files: " + folderToProcess.ThreadsUsedToProcessFiles);
                Console.WriteLine("# Processed files: " + folderToProcess.FilesProcessedCount);

                if (folderToProcess.FilesThatCouldNotBeProcessed.Count > 0)
                {
                    Console.WriteLine("# Files that could not be processed: " + folderToProcess.FilesThatCouldNotBeProcessed.Count);
                    Console.WriteLine("# Check FilesThatCouldNotBeProcessed.txt for list of non processed files");
                }
            }
            else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No watermark could be found.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            Console.WriteLine("# Press any key to exit...");
            Console.ReadLine();            
        }
    }
}
