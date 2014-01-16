using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceCodeWaterMark
{
    public class FolderToWatermark
    {
        private string _folderPath = String.Empty;
        private bool _folderPathIsValid = true;
        private int _filesToProcess = 0;

        public FolderToWatermark(string folderPath) {
            
            if (String.IsNullOrEmpty(folderPath)) {
                _folderPathIsValid = false;
                return;
            }

            if (!Directory.Exists(folderPath))
            {
                _folderPathIsValid = false;
                return;
            }

            _folderPath = folderPath;
            GetSupportedFiles();
        }

        private void GetSupportedFiles() {
            DirectoryInfo di = new DirectoryInfo(_folderPath);
            FileInfo[] fi = new string[] { "*.ascx", "*.cs" }
                .SelectMany(i => di.GetFiles(i, SearchOption.AllDirectories))
                .Distinct().ToArray();
            _filesToProcess = fi.Count();
        }

        public int FilesToProcess
        {
            get{
                return _filesToProcess;
            }
        }

        public void AddWaterMark() {
            // 
        }
    }
}
