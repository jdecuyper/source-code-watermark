using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceCodeWaterMark
{
    /// <summary>
    /// Load all supported file extensions and their associated code symbols.
    /// For instance, *.cs file will use the following code symbol right before 
    /// the watermark: //
    /// These settings can be edited inside the following file: ExtensionsAndCodeSymbols.txt
    /// </summary>
    public class FileExtensionAndCodeSymbol
    {
        private Dictionary<string, Tuple<string, string>> extensions = new Dictionary<string, Tuple<string, string>>();
        private string fileWithExtensionsName = "Extension.txt";

        public FileExtensionAndCodeSymbol()
        {
            ReadSettingFile();
        }

        private void ReadSettingFile(){
            // load hashtable with extensions and code symbols   
        }

        public Dictionary<string, Tuple<string, string>> Extensions{
            get {
                return extensions;    
            }  
        }
    }
}
