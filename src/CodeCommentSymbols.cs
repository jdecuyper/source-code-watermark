using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SourceCodeWaterMark
{
    /// <summary>
    /// Load all supported file extensions and their associated comment symbols.
    /// For instance, *.css file will use the following comment symbol:
    /// /* watermark */
    /// These settings can be edited inside the following file: CommentSymbols.txt
    /// </summary>
    public class CodeCommentSymbols
    {
        private Dictionary<string, Tuple<string, string>> _commentSymbols = new Dictionary<string, Tuple<string, string>>();
        private bool _settingFileWasLoaded = false;
        private string[] _fileExtensionsRegex;
        public const string SETTINGS_FILE_NAME = "CommentSymbols.txt";

        public CodeCommentSymbols(string fileSettingsPath)
        {
            if (!File.Exists(fileSettingsPath))
            {
                _settingFileWasLoaded = false;
                return;
            }

            ReadSettingFile(fileSettingsPath);
        }

        /// <summary>
        /// Load hashtable with extensions and commment symbols.
        /// </summary>
        /// <param name="fileSettingsPath"></param>
        private void ReadSettingFile(string fileSettingsPath)
        {
            _settingFileWasLoaded = true;

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
                        List<string> words = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Select(p => Regex.Replace(p, @" |\t|\n|\r", "")).ToList();

                        if (words.Count >= 2)
                        {
                            string fileExtension = words[0].StartsWith(".") ? words[0] : String.Format(".{0}", words[0]);
                            string startCommentSymbol = words[1];
                            string endCommentSymbol = words.Count == 2 ? String.Empty : words[2];

                            // If the extension already exists, it is not overwritten
                            if (_commentSymbols.ContainsKey(fileExtension))
                                continue;

                            Tuple<string, string> commentSymbol;
                            commentSymbol = new Tuple<string, string>(startCommentSymbol, endCommentSymbol);
                            _commentSymbols.Add(fileExtension, commentSymbol);
                        }
                    }
                }

                // Keep track of the support file extensions
                _fileExtensionsRegex = _commentSymbols.Keys.Select(ext => String.Format("*{0}", ext)).ToArray();
            }
        }

        public Dictionary<string, Tuple<string, string>> CommentSymbols
        {
            get
            {
                return _commentSymbols;
            }
        }

        public string[] FileExtensions
        {
            get
            {
                return _fileExtensionsRegex;
            }
        }

        public bool SettingFileWasLoaded
        {
            get
            {
                return _settingFileWasLoaded;
            }
        }
    }
}
