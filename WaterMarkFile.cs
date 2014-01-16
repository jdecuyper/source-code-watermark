using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceCodeWaterMark
{
    public class WaterMarkFile
    {
        private string _filePath = String.Empty;
        private bool _filePathIsValid = true;

        public WaterMarkFile(string filePath) {
            
            if (String.IsNullOrEmpty(filePath)) {
                _filePathIsValid = false;
                return;
            }

            if (!File.Exists(filePath))
            {
                _filePathIsValid = false;
                return;
            }

            _filePath = filePath;
        }

        public virtual bool TryToAddWaterMark() {
            return false;
        }

        public virtual string GetSupportedExtension()
        {
            return String.Empty;
        }
    }
}
