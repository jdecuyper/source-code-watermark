using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceCodeWaterMark
{
    /// <summary>
    /// Load all supported file extensions and their associated code symbols.
    /// For instance, *.cs file will use the following code symbol right before 
    /// the watermark: //
    /// These settings can be edited inside the following file: ExtensionsAndCodeSymbols.txt
    /// </summary>
    public class FileExtensionAndCodeSymbol
    {
        private Dictionary<string, Tuple<string, string>> extensions = new Dictionary<string, Tuple<string, string>>();
        private bool fileWasLoaded = true;

        public FileExtensionAndCodeSymbol(string fileSettingsPath)
        {
            if (!File.Exists(fileSettingsPath))
            {
                fileWasLoaded = false;
                return;
            }

            ReadSettingFile(fileSettingsPath);
        }

        /// <summary>
        /// Load hashtable with extensions and code symbols.
        /// </summary>
        /// <param name="fileSettingsPath"></param>
        private void ReadSettingFile(string fileSettingsPath)
        {
            using (FileStream fs = new FileStream(fileSettingsPath,
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();
                        if (line.Contains(' '))
                        {
                            List<string> words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                            // If the extension already exists, it is not overwritten
                            if (extensions.ContainsKey(words[0]))
                                continue;

                            Tuple<string, string> commentSymbol;
                            if (words.Count == 2)
                                commentSymbol = new Tuple<string, string>(words[1], String.Empty);
                            else if (words.Count == 3)
                                commentSymbol = new Tuple<string, string>(words[1], words[2]);
                            else
                                commentSymbol = new Tuple<string, string>(String.Empty, String.Empty);

                            extensions.Add(words[0], commentSymbol);
                        }
                    }
                }
            }
        }

        public Dictionary<string, Tuple<string, string>> Extensions
        {
            get
            {
                return extensions;
            }
        }

        public bool SettingFileWasLoaded
        {
            get
            {
                return fileWasLoaded;
            }
        }
    }
}
