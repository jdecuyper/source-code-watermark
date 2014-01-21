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
        private bool _filesWereWatermarked = false;
        private int _filesToProcessCount = 0;
        private CodeCommentSymbols _codeComments;
        FileInfo[] _filesToProcess;
        HashSet<int> threadIds = new HashSet<int>();

        public FolderToWatermark(string folderPath, CodeCommentSymbols codeComments)
        {
            if (String.IsNullOrEmpty(folderPath))
                throw new Exception("Folder path is empty");

            if (!Directory.Exists(folderPath))
                throw new Exception("Folder path does not exist");

            if (codeComments == null || codeComments.CommentSymbols.Count == 0)
                throw new Exception("No code comment symbols are available");
            
            _codeComments = codeComments;
            _folderPath = folderPath;
            
            ReadFilesInsideFolder();
        }

        private void ReadFilesInsideFolder()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(_folderPath);
            _filesToProcess = _codeComments.FileExtensions
                .SelectMany(i => dirInfo.GetFiles(i, SearchOption.AllDirectories))
                .Distinct().ToArray();
            _filesToProcessCount = _filesToProcess.Count();
        }

        /// <summary>
        /// To avoid multiple watermarking of the same files, this method can only run once. 
        /// </summary>
        public void AddWaterMarkToFiles() {

            if (_filesWereWatermarked)
                return;

            // TODO
            // what if the release number already exists inside a file?
            // what if some file raises an error when reading or writing?
            
            if (_filesToProcess != null && _filesToProcess.Count() > 0)
            {
                // Divide work between multiple threads 
                Parallel.ForEach(_filesToProcess, fileInfo =>
                {
                    threadIds.Add(Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " - " + fileInfo.Name);
                });
            }

            _filesWereWatermarked = true;
        }

        public int FilesToProcessCount
        {
            get
            {
                return _filesToProcessCount;
            }
        }

        public int FilesProcessedCount
        {
            get
            {
                return 0;
            }
        }

        public int ThreadsUsedToProcessFiles
        {
            get
            {
                return threadIds.Count();
            }
        }
    }
}
