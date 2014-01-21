using NUnit.Framework;
using SourceCodeWaterMark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCodeWatermarkUnitTests.API
{
    public class CommentSymbolsTest
    {
        [Test]
        public void SettingsFileDoesNotExist()
        {
            CodeCommentSymbols fileSettings = new CodeCommentSymbols("file does not exist");
            Assert.IsFalse(fileSettings.SettingFileWasLoaded);
        }

        [Test]
        public void SettingsFileExists()
        {
            CodeCommentSymbols fileSettings = new CodeCommentSymbols(Environment.CurrentDirectory + "\\..\\..\\Ressources\\CommentSymbols-Empty.txt");
            Assert.IsTrue(fileSettings.SettingFileWasLoaded);
        }

        [Test]
        public void SettingsFileEmpty()
        {
            CodeCommentSymbols fileSettings = new CodeCommentSymbols(Environment.CurrentDirectory + "\\..\\..\\Ressources\\CommentSymbols-Empty.txt");
            Assert.AreEqual(0, fileSettings.CommentSymbols.Count());
        }

        [Test]
        public void SettingsFileInvalidFormat()
        {
            CodeCommentSymbols fileSettings = new CodeCommentSymbols(Environment.CurrentDirectory + "\\..\\..\\Ressources\\CommentSymbols-InvalidFormat.txt");
            Assert.AreEqual(0, fileSettings.CommentSymbols.Count());
        }

        [Test]
        public void SettingsFileOne()
        {
            CodeCommentSymbols fileSettings = new CodeCommentSymbols(Environment.CurrentDirectory + "\\..\\..\\Ressources\\CommentSymbols-1.txt");
            Assert.AreEqual(1, fileSettings.CommentSymbols.Count());
            Assert.IsNotNull(fileSettings.CommentSymbols["css"]);
            Assert.AreEqual("/*", fileSettings.CommentSymbols["css"].Item1);
            Assert.AreEqual("*/", fileSettings.CommentSymbols["css"].Item2);
        }

        [Test]
        public void SettingsFileOneWithExtraSpaces()
        {
            CodeCommentSymbols fileSettings = new CodeCommentSymbols(Environment.CurrentDirectory + "\\..\\..\\Ressources\\CommentSymbols-1-ExtraSpaces.txt");
            Assert.AreEqual(1, fileSettings.CommentSymbols.Count());
            Assert.IsNotNull(fileSettings.CommentSymbols["css"]);
            Assert.AreEqual("/*", fileSettings.CommentSymbols["css"].Item1);
            Assert.AreEqual("*/", fileSettings.CommentSymbols["css"].Item2);
        }

        [Test]
        public void SettingsFileTwo()
        {
            CodeCommentSymbols fileSettings = new CodeCommentSymbols(Environment.CurrentDirectory + "\\..\\..\\Ressources\\CommentSymbols-2.txt");
            Assert.AreEqual(2, fileSettings.CommentSymbols.Count());
            Assert.IsNotNull(fileSettings.CommentSymbols["cs"]);
            Assert.AreEqual("//", fileSettings.CommentSymbols["cs"].Item1);
            Assert.AreEqual(String.Empty, fileSettings.CommentSymbols["cs"].Item2);

            Assert.IsNotNull(fileSettings.CommentSymbols["js"]);
            Assert.AreEqual("/*", fileSettings.CommentSymbols["js"].Item1);
            Assert.AreEqual("*/", fileSettings.CommentSymbols["js"].Item2);
        }

        [Test]
        public void SettingsFileExtraEmptyLines()
        {
            CodeCommentSymbols fileSettings = new CodeCommentSymbols(Environment.CurrentDirectory + "\\..\\..\\Ressources\\CommentSymbols-ExtraEmptyLines.txt");
            Assert.AreEqual(6, fileSettings.CommentSymbols.Count());
            Assert.IsNotNull(fileSettings.CommentSymbols["master"]);
            Assert.AreEqual(fileSettings.CommentSymbols["master"].Item1, "<%--");
            Assert.AreEqual(fileSettings.CommentSymbols["master"].Item2, "--%>");
        }

        [Test]
        public void SettingsFileTabsBetweenItems()
        {
            CodeCommentSymbols fileSettings = new CodeCommentSymbols(Environment.CurrentDirectory + "\\..\\..\\Ressources\\CommentSymbols-Tabs.txt");
            Assert.AreEqual(2, fileSettings.CommentSymbols.Count());
            
            Assert.IsNotNull(fileSettings.CommentSymbols["css"]);
            Assert.AreEqual(fileSettings.CommentSymbols["css"].Item1, "/*");
            Assert.AreEqual(fileSettings.CommentSymbols["css"].Item2, "*/");

            Assert.IsNotNull(fileSettings.CommentSymbols["cs"]);
            Assert.AreEqual(fileSettings.CommentSymbols["cs"].Item1, "//");
            Assert.AreEqual(fileSettings.CommentSymbols["cs"].Item2, String.Empty);
        }
        
    }
}
