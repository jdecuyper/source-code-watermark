using NUnit.Framework;
using SourceCodeWaterMark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCodeWatermarkUnitTests.API
{
    public class FileExtensionTest
    {
        [Test]
        public void SettingsFileDoesNotExist()
        {
            FileExtensionAndCodeSymbol fileSettings = new FileExtensionAndCodeSymbol("file does not exist");
            Assert.IsFalse(fileSettings.SettingFileWasLoaded);
        }

        [Test]
        public void SettingsFileExists()
        {
            FileExtensionAndCodeSymbol fileSettings = new FileExtensionAndCodeSymbol(Environment.CurrentDirectory + "\\..\\..\\Ressources\\ExtCodeSymbols-Empty.txt");
            Assert.IsTrue(fileSettings.SettingFileWasLoaded);
        }

        [Test]
        public void SettingsFileEmpty()
        {
            FileExtensionAndCodeSymbol fileSettings = new FileExtensionAndCodeSymbol(Environment.CurrentDirectory + "\\..\\..\\Ressources\\ExtCodeSymbols-Empty.txt");
            Assert.AreEqual(0, fileSettings.Extensions.Count());
        }

        [Test]
        public void SettingsFileInvalidFormat()
        {
            FileExtensionAndCodeSymbol fileSettings = new FileExtensionAndCodeSymbol(Environment.CurrentDirectory + "\\..\\..\\Ressources\\ExtCodeSymbols-InvalidFormat.txt");
            Assert.AreEqual(0, fileSettings.Extensions.Count());
        }

        [Test]
        public void SettingsFileOne()
        {
            FileExtensionAndCodeSymbol fileSettings = new FileExtensionAndCodeSymbol(Environment.CurrentDirectory + "\\..\\..\\Ressources\\ExtCodeSymbols-1.txt");
            Assert.AreEqual(1, fileSettings.Extensions.Count());
            Assert.IsNotNull(fileSettings.Extensions["css"]);
            Assert.AreEqual("/*", fileSettings.Extensions["css"].Item1);
            Assert.AreEqual("*/", fileSettings.Extensions["css"].Item2);
        }

        [Test]
        public void SettingsFileOneWithExtraSpaces()
        {
            FileExtensionAndCodeSymbol fileSettings = new FileExtensionAndCodeSymbol(Environment.CurrentDirectory + "\\..\\..\\Ressources\\ExtCodeSymbols-1-ExtraSpaces.txt");
            Assert.AreEqual(1, fileSettings.Extensions.Count());
            Assert.IsNotNull(fileSettings.Extensions["css"]);
            Assert.AreEqual("/*", fileSettings.Extensions["css"].Item1);
            Assert.AreEqual("*/", fileSettings.Extensions["css"].Item2);
        }

        [Test]
        public void SettingsFileTwo()
        {
            FileExtensionAndCodeSymbol fileSettings = new FileExtensionAndCodeSymbol(Environment.CurrentDirectory + "\\..\\..\\Ressources\\ExtCodeSymbols-2.txt");
            Assert.AreEqual(2, fileSettings.Extensions.Count());
            Assert.IsNotNull(fileSettings.Extensions["cs"]);
            Assert.AreEqual("//", fileSettings.Extensions["cs"].Item1);
            Assert.AreEqual(String.Empty, fileSettings.Extensions["cs"].Item2);

            Assert.IsNotNull(fileSettings.Extensions["js"]);
            Assert.AreEqual("/*", fileSettings.Extensions["js"].Item1);
            Assert.AreEqual("*/", fileSettings.Extensions["js"].Item2);
        }

        [Test]
        public void SettingsFileExtraEmptyLines()
        {
            FileExtensionAndCodeSymbol fileSettings = new FileExtensionAndCodeSymbol(Environment.CurrentDirectory + "\\..\\..\\Ressources\\ExtCodeSymbols-ExtraEmptyLines.txt");
            Assert.AreEqual(6, fileSettings.Extensions.Count());
            Assert.IsNotNull(fileSettings.Extensions["master"]);
            Assert.AreEqual(fileSettings.Extensions["master"].Item1, "<%--");
            Assert.AreEqual(fileSettings.Extensions["master"].Item2, "--%>");
        }
    }
}
