using NUnit.Framework;
using SourceCodeWaterMark;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCodeWatermarkUnitTests.API
{
    public class FolderToWatermarkTest
    {
        private Watermark mark = new Watermark(Environment.CurrentDirectory + "\\..\\..\\Ressources\\" + Watermark.WATERMARK_FILE_NAME, "3.4.6");

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "Folder path is empty")]
        public void FolderIsEmpty()
        {
            CodeCommentSymbols fileSettings = new CodeCommentSymbols(String.Empty);
            FolderToWatermark folderToMark = new FolderToWatermark(String.Empty, fileSettings, mark);
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "Folder path does not exist")]
        public void FolderDoesNotExist()
        {
            CodeCommentSymbols fileSettings = new CodeCommentSymbols(String.Empty);
            FolderToWatermark folderToMark = new FolderToWatermark("Folder does not exist", fileSettings, mark);
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "No code comment symbols are available")]
        public void FileSettingsIsNull()
        {
            CodeCommentSymbols fileSettings = null;
            FolderToWatermark folderToMark = new FolderToWatermark(Environment.CurrentDirectory + "\\..\\..\\Ressources\\FolderToWaterMark\\", fileSettings, mark);
        }

        [Test]
        public void NoFilesInFolder()
        {
            CodeCommentSymbols fileSettings = new CodeCommentSymbols(Environment.CurrentDirectory + "\\..\\..\\Ressources\\FolderToWaterMark\\CommentSymbols.txt");
            FolderToWatermark folderToMark = new FolderToWatermark(Environment.CurrentDirectory + "\\..\\..\\Ressources\\FolderToWaterMark\\FolderIsEmpty", fileSettings, mark);

            Assert.AreEqual(0, folderToMark.FilesToProcessCount);
            Assert.AreEqual(0, folderToMark.ThreadsUsedToProcessFiles);
            Assert.AreEqual(0, folderToMark.FilesProcessedCount);
        }

        [Test]
        public void SomeFilesInFolder()
        {
            CodeCommentSymbols fileSettings = new CodeCommentSymbols(Environment.CurrentDirectory + "\\..\\..\\Ressources\\FolderToWaterMark\\CommentSymbols.txt");
            FolderToWatermark folderToMark = new FolderToWatermark(Environment.CurrentDirectory + "\\..\\..\\Ressources\\FolderToWaterMark\\SomeFilesToRead", fileSettings, mark);

            Assert.AreEqual(3, folderToMark.FilesToProcessCount);
            Assert.AreEqual(0, folderToMark.FilesProcessedCount);
        }

        [Test]
        public void WatermarkValidFiles()
        {
            // Copy non watermarked files over
            foreach (var file in Directory.GetFiles(Environment.CurrentDirectory + "\\..\\..\\Ressources\\FolderToWaterMark\\SomeFilesToRead"))
                File.Copy(file, Path.Combine(Environment.CurrentDirectory + "\\..\\..\\Ressources\\FolderToWaterMark\\SomeFilesToWatermark", Path.GetFileName(file)), true);

            CodeCommentSymbols fileSettings = new CodeCommentSymbols(Environment.CurrentDirectory + "\\..\\..\\Ressources\\FolderToWaterMark\\CommentSymbols.txt");
            FolderToWatermark folderToMark = new FolderToWatermark(Environment.CurrentDirectory + "\\..\\..\\Ressources\\FolderToWaterMark\\SomeFilesToWatermark", fileSettings, mark);

            Assert.AreEqual(3, folderToMark.FilesToProcessCount);
            Assert.AreEqual(0, folderToMark.FilesProcessedCount);

            folderToMark.WaterMarkFiles();

            //Assert.AreEqual(0, folderToMark.FilesToProcessCount);
            //Assert.AreEqual(3, folderToMark.FilesProcessedCount);
            //Assert.AreNotEqual(0, folderToMark.ThreadsUsedToProcessFiles);

            // Delete watermarked files
            //foreach (var file in Directory.GetFiles(Environment.CurrentDirectory + "\\..\\..\\Ressources\\FolderToWaterMark\\SomeFilesToWatermark"))
            //    File.Delete(file);
        }
    }
}
