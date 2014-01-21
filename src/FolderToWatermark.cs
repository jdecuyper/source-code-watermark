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
        private int _filesProcessedCount = 0;
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
        public void WaterMarkFiles() {

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
                    // Keep track of the amount of threads involved in the process
                    threadIds.Add(Thread.CurrentThread.ManagedThreadId);

                    // Get starting and ending comment symbol
                    Tuple<string, string> commentSymbols = _codeComments.CommentSymbols[fileInfo.Extension];

                    // Create stream containing the water mark
                    byte[] byteArray = Encoding.UTF8.GetBytes(commentSymbols.Item1 + " WATER MARK " + commentSymbols.Item2 + Environment.NewLine + Environment.NewLine);
                    MemoryStream watermarkStream = new MemoryStream(byteArray);
                    int watermarkBufferSize = (int)watermarkStream.Length;

                    // Write stream to file, this avoids to read the whole file in memory
                    using (Stream destStream = File.OpenWrite(fileInfo.FullName))
                    {
                        watermarkStream.CopyTo(destStream, watermarkBufferSize);
                        watermarkStream.Dispose();
                    }

                    _filesProcessedCount++;
                    _filesToProcessCount--;
                });
            }

            _filesWereWatermarked = true;
        }

        void CopyStream(Stream destination, Stream source)
        {
            int count;
            byte[] buffer = new byte[4096];
            while ((count = source.Read(buffer, 0, buffer.Length)) > 0)
                destination.Write(buffer, 0, count);
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
                return _filesProcessedCount;
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
