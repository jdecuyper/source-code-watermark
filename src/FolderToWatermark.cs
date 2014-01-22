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
        private List<FileInfo> _filesToProcess;
        private List<FileInfo> _filesThatCouldNotBeToProcess = new List<FileInfo>();
        private HashSet<int> _threadIds = new HashSet<int>();
        private Watermark _mark;

        public FolderToWatermark(string folderPath, CodeCommentSymbols codeComments, Watermark mark)
        {
            if (String.IsNullOrEmpty(folderPath))
                throw new Exception("Folder path is empty");

            if (!Directory.Exists(folderPath))
                throw new Exception("Folder path does not exist");

            if (codeComments == null)
                throw new Exception("No code comment symbols are available");

            if (mark == null)
                throw new Exception("No watermark is available");

            _codeComments = codeComments;
            _folderPath = folderPath;
            _mark = mark;

            ReadFilesInsideFolder();
        }

        private void ReadFilesInsideFolder()
        {
            // Grab all files and remove duplicates (*.cs and *.aspx.cs will likely have some results in common)
            DirectoryInfo dirInfo = new DirectoryInfo(_folderPath);
            _filesToProcess = _codeComments.FileExtensions
              .SelectMany(i => dirInfo.GetFiles(i, SearchOption.AllDirectories))
              .Distinct(new FileInfoFullNameComparer())
              .ToList();

            _filesToProcessCount = _filesToProcess.Count();
        }

        /// <summary>
        /// To avoid multiple watermarking of the same files, this method can only run once. 
        /// </summary>
        public void WaterMarkFiles() {

            if (_filesWereWatermarked)
                return;

            if (_filesToProcess != null && _filesToProcess.Count() > 0 && _mark.TextLines.Count > 0)
            {
                // Divide work between multiple threads 
                Parallel.ForEach(_filesToProcess, fileInfo =>
                {
                    // Keep track of the amount of threads involved in the process
                    _threadIds.Add(Thread.CurrentThread.ManagedThreadId);

                    // Get starting and ending comment symbol
                    Tuple<string, string> commentSymbols = _codeComments.CommentSymbols[fileInfo.Extension];

                    Stream watermarkStream = null, output = null;
                    
                    // Create stream containing the water mark
                    byte[] byteArray = Encoding.UTF8.GetBytes(_mark.Generate(commentSymbols));
                    watermarkStream = new MemoryStream(byteArray);
                    int watermarkBufferSize = (int)watermarkStream.Length;
                    
                    try
                    {
                        // Convert file into a stream
                        output = new FileStream(fileInfo.FullName, FileMode.Append, FileAccess.Write, FileShare.None);
                        
                        // Copy watermark to the file
                        watermarkStream.CopyTo(output, watermarkBufferSize);
                    }
                    catch (System.ArgumentNullException argExc)
                    {
                        _filesThatCouldNotBeToProcess.Add(fileInfo);
                    }
                    catch (System.NotSupportedException notSupportedExc)
                    {
                        _filesThatCouldNotBeToProcess.Add(fileInfo);
                    }
                    catch (System.ObjectDisposedException objDisposedExc)
                    {
                        _filesThatCouldNotBeToProcess.Add(fileInfo);
                    }
                    catch (System.IO.IOException ioExc)
                    {
                        _filesThatCouldNotBeToProcess.Add(fileInfo);
                    }
                    finally {
                        if(watermarkStream != null) watermarkStream.Dispose();
                        if(output != null) output.Dispose();
                        
                        _filesProcessedCount++;
                        _filesToProcessCount--;
                    }
                });
            }
            DumpFilesWithErrorToTxtFile();

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

        public List<FileInfo> FilesThatCouldNotBeProcessed
        {
            get
            {
                return _filesThatCouldNotBeToProcess;
            }
        }

        public void DumpFilesWithErrorToTxtFile() {
            if (_filesThatCouldNotBeToProcess.Count > 0)
            {
                StringBuilder filesWithError = new StringBuilder();
                foreach (FileInfo fi in _filesThatCouldNotBeToProcess)
                {
                    filesWithError.Append(fi.FullName + Environment.NewLine);
                }
                File.WriteAllText(Environment.CurrentDirectory + "\\" + "FilesThatCouldNotBeProcessed.txt", filesWithError.ToString());
            }
        }

        public int ThreadsUsedToProcessFiles
        {
            get
            {
                return _threadIds.Count();
            }
        }
    }
}
