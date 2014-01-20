using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SourceCodeWaterMark
{
    public class FolderToWatermark
    {
        private string _folderPath = String.Empty;
        private bool _folderPathIsValid = true;
        private int _filesToProcessCount = 0;
        private CodeCommentSymbols _codeComments;
        FileInfo[] _filesToProcess;

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

            ReadFilesInsideFolder();
            AddWaterMarkToFiles();
        }

        private void ReadFilesInsideFolder()
        {
            if (!_folderPathIsValid)
                return;

            DirectoryInfo dirInfo = new DirectoryInfo(_folderPath);
            _filesToProcess = _codeComments.FileExtensions
                .SelectMany(i => dirInfo.GetFiles(i, SearchOption.AllDirectories))
                .Distinct().ToArray();
            _filesToProcessCount = _filesToProcess.Count();
        }

        public int FilesToProcessCount
        {
            get{
                return _filesToProcessCount;
            }
        }

        public void AddWaterMarkToFiles() {

            if (!_folderPathIsValid)
                return;

            // TODO
            // what if the release number already exists inside a file?
            // what if some file raises an error when reading or writing?
            
            if (_filesToProcess != null && _filesToProcess.Count() > 0)
            {
                // Divide work between multiple threads 
                Parallel.ForEach(_filesToProcess, fileInfo =>
                {

                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " - " + fileInfo.Name);
                });
            }
        }
    }
}
