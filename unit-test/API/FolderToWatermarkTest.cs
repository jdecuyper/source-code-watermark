using NUnit.Framework;
using SourceCodeWaterMark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCodeWatermarkUnitTests.API
{
    public class FolderToWatermarkTest
    {
        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "Folder path is empty")]
        public void FolderIsEmpty()
        {
            CodeCommentSymbols fileSettings = new CodeCommentSymbols(String.Empty);
            FolderToWatermark folderToMark = new FolderToWatermark(String.Empty, fileSettings);
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "Folder path does not exist")]
        public void FolderDoesNotExist()
        {
            CodeCommentSymbols fileSettings = new CodeCommentSymbols(String.Empty);
            FolderToWatermark folderToMark = new FolderToWatermark("Folder does not exist", fileSettings);
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "No code comment symbols are available")]
        public void FileSettingsIsNull()
        {
            CodeCommentSymbols fileSettings = null;
            FolderToWatermark folderToMark = new FolderToWatermark(Environment.CurrentDirectory + "\\..\\..\\Ressources\\FolderToWaterMark\\", fileSettings);
        }

        [Test]
        public void NoFilesInFolder()
        {
            CodeCommentSymbols fileSettings = new CodeCommentSymbols(Environment.CurrentDirectory + "\\..\\..\\Ressources\\FolderToWaterMark\\CommentSymbols.txt");
            FolderToWatermark folderToMark = new FolderToWatermark(Environment.CurrentDirectory + "\\..\\..\\Ressources\\FolderToWaterMark\\FolderIsEmpty", fileSettings);

            Assert.AreEqual(0, folderToMark.FilesToProcessCount);
            Assert.AreEqual(0, folderToMark.ThreadsUsedToProcessFiles);
            Assert.AreEqual(0, folderToMark.FilesProcessedCount);
        }
    }
}
