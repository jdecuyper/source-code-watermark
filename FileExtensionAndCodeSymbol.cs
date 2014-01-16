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
        private const string fileWithExtensionsName = "ExtensionsAndCodeSymbols.txt";
        private bool fileWasLoaded = true;

        public FileExtensionAndCodeSymbol()
        {
            ReadSettingFile();
        }

        private void ReadSettingFile(){
            // load hashtable with extensions and code symbols
            string absPath = Environment.CurrentDirectory + "\\" + fileWithExtensionsName;
            if(!File.Exists(absPath)){
                fileWasLoaded = false;
                return;
            }

            using (FileStream fs = new FileStream(absPath,
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();
                        if (line.Contains(' ')) {
                            List<string> words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            
                            if(extensions.ContainsKey(words[0]))
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
                        //Console.WriteLine(sr.ReadLine());
                    }
                }
            }
        }

        public Dictionary<string, Tuple<string, string>> Extensions{
            get {
                return extensions;    
            }  
        }
    }
}
