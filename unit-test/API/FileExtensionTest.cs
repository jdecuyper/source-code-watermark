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
            Assert.AreEqual(0, fileSettings.ExtensionsAndCodeSymbols.Count());
        }

        [Test]
        public void SettingsFileInvalidFormat()
        {
            FileExtensionAndCodeSymbol fileSettings = new FileExtensionAndCodeSymbol(Environment.CurrentDirectory + "\\..\\..\\Ressources\\ExtCodeSymbols-InvalidFormat.txt");
            Assert.AreEqual(0, fileSettings.ExtensionsAndCodeSymbols.Count());
        }

        [Test]
        public void SettingsFileOne()
        {
            FileExtensionAndCodeSymbol fileSettings = new FileExtensionAndCodeSymbol(Environment.CurrentDirectory + "\\..\\..\\Ressources\\ExtCodeSymbols-1.txt");
            Assert.AreEqual(1, fileSettings.ExtensionsAndCodeSymbols.Count());
            Assert.IsNotNull(fileSettings.ExtensionsAndCodeSymbols["css"]);
            Assert.AreEqual("/*", fileSettings.ExtensionsAndCodeSymbols["css"].Item1);
            Assert.AreEqual("*/", fileSettings.ExtensionsAndCodeSymbols["css"].Item2);
        }

        [Test]
        public void SettingsFileOneWithExtraSpaces()
        {
            FileExtensionAndCodeSymbol fileSettings = new FileExtensionAndCodeSymbol(Environment.CurrentDirectory + "\\..\\..\\Ressources\\ExtCodeSymbols-1-ExtraSpaces.txt");
            Assert.AreEqual(1, fileSettings.ExtensionsAndCodeSymbols.Count());
            Assert.IsNotNull(fileSettings.ExtensionsAndCodeSymbols["css"]);
            Assert.AreEqual("/*", fileSettings.ExtensionsAndCodeSymbols["css"].Item1);
            Assert.AreEqual("*/", fileSettings.ExtensionsAndCodeSymbols["css"].Item2);
        }

        [Test]
        public void SettingsFileTwo()
        {
            FileExtensionAndCodeSymbol fileSettings = new FileExtensionAndCodeSymbol(Environment.CurrentDirectory + "\\..\\..\\Ressources\\ExtCodeSymbols-2.txt");
            Assert.AreEqual(2, fileSettings.ExtensionsAndCodeSymbols.Count());
            Assert.IsNotNull(fileSettings.ExtensionsAndCodeSymbols["cs"]);
            Assert.AreEqual("//", fileSettings.ExtensionsAndCodeSymbols["cs"].Item1);
            Assert.AreEqual(String.Empty, fileSettings.ExtensionsAndCodeSymbols["cs"].Item2);

            Assert.IsNotNull(fileSettings.ExtensionsAndCodeSymbols["js"]);
            Assert.AreEqual("/*", fileSettings.ExtensionsAndCodeSymbols["js"].Item1);
            Assert.AreEqual("*/", fileSettings.ExtensionsAndCodeSymbols["js"].Item2);
        }

        [Test]
        public void SettingsFileExtraEmptyLines()
        {
            FileExtensionAndCodeSymbol fileSettings = new FileExtensionAndCodeSymbol(Environment.CurrentDirectory + "\\..\\..\\Ressources\\ExtCodeSymbols-ExtraEmptyLines.txt");
            Assert.AreEqual(6, fileSettings.ExtensionsAndCodeSymbols.Count());
            Assert.IsNotNull(fileSettings.ExtensionsAndCodeSymbols["master"]);
            Assert.AreEqual(fileSettings.ExtensionsAndCodeSymbols["master"].Item1, "<%--");
            Assert.AreEqual(fileSettings.ExtensionsAndCodeSymbols["master"].Item2, "--%>");
        }

        [Test]
        public void SettingsFileTabsBetweenItems()
        {
            FileExtensionAndCodeSymbol fileSettings = new FileExtensionAndCodeSymbol(Environment.CurrentDirectory + "\\..\\..\\Ressources\\ExtCodeSymbols-Tabs.txt");
            Assert.AreEqual(2, fileSettings.ExtensionsAndCodeSymbols.Count());
            
            Assert.IsNotNull(fileSettings.ExtensionsAndCodeSymbols["css"]);
            Assert.AreEqual(fileSettings.ExtensionsAndCodeSymbols["css"].Item1, "/*");
            Assert.AreEqual(fileSettings.ExtensionsAndCodeSymbols["css"].Item2, "*/");

            Assert.IsNotNull(fileSettings.ExtensionsAndCodeSymbols["cs"]);
            Assert.AreEqual(fileSettings.ExtensionsAndCodeSymbols["cs"].Item1, "//");
            Assert.AreEqual(fileSettings.ExtensionsAndCodeSymbols["cs"].Item2, String.Empty);
        }
        
    }
}
