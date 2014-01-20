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
        private CodeCommentSymbols _codeComments;

        public FolderToWatermark(string folderPath, CodeCommentSymbols codeComments)
        {
            
            if (String.IsNullOrEmpty(folderPath)) {
                _folderPathIsValid = false;
                return;
            }

            if (!Directory.Exists(folderPath))
            {
                _folderPathIsValid = false;
                return;
            }

            if (codeComments != null)
                _codeComments = codeComments;

            _folderPath = folderPath;

            GetSupportedFiles();
        }

        private void GetSupportedFiles() {
            DirectoryInfo di = new DirectoryInfo(_folderPath);
            FileInfo[] fi = _codeComments.FileExtensions
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
            // what if the release number already exists inside a file?
            // what if some file raises an error when reading or writing?
        }
    }
}
