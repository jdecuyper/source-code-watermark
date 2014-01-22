using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceCodeWaterMark
{
    public class Watermark
    {
        public const string WATERMARK_FILE_NAME = "Watermark.txt";
        private List<string> _textLines = new List<string>();
        private string _releaseNumber = String.Empty;

        public Watermark(string fileSettingsPath, string releaseNumber)
        {
            if (String.IsNullOrEmpty(fileSettingsPath))
                throw new Exception("Watermark file path is not valid");
            
            if (!File.Exists(fileSettingsPath))
                throw new Exception("Watermark setting file does not exist");

            _releaseNumber = releaseNumber;

            ReadSettingFile(fileSettingsPath);
        }

        private void ReadSettingFile(string fileSettingsPath)
        {
            string watermark = String.Empty;

            using (FileStream fs = new FileStream(fileSettingsPath,
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (sr.Peek() >= 0)
                    {
                        string currentLine = sr.ReadLine();
                        if (currentLine.Contains("RELEASE_NUMBER"))
                            currentLine = currentLine.Replace("RELEASE_NUMBER", _releaseNumber);
                        
                        if (currentLine.Contains("TIMESTAMP"))
                            currentLine = currentLine.Replace("TIMESTAMP", DateTime.Now.ToString());

                        _textLines.Add(currentLine);
                    }
                }
            }
        }

        public List<string> TextLines
        {
            get {
                return _textLines;
            }
        }

        public string Generate(Tuple<string, string> commentSymbols)
        {
            if (_textLines.Count == 0)
                return String.Empty;

            // Combine watermark with comment symbols
            StringBuilder watermark = new StringBuilder();
            watermark.Append(Environment.NewLine);
            
            if (commentSymbols.Item2 == String.Empty)
            {
                // If only one comment symbol is available then add it at the beginning of each line
                // Example: #, //
                foreach (string text in _textLines)
                {
                    watermark.Append(commentSymbols.Item1 + " " + text + " " + commentSymbols.Item2);
                    watermark.Append(Environment.NewLine);
                }
            }
            else
            {
                // If two comment symbols are available then add it only before and after the watermark
                // Example: /* and */
                watermark.Append(commentSymbols.Item1 + Environment.NewLine);
                foreach (string text in _textLines)
                {
                    watermark.Append(text);
                    watermark.Append(Environment.NewLine);
                }
                watermark.Append(commentSymbols.Item2);
            }
            return watermark.ToString();
        }
    }
}
