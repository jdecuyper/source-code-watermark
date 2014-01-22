using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceCodeWaterMark
{
    public class FileInfoFullNameComparer : IEqualityComparer<FileInfo>
    {
        public bool Equals(FileInfo x, FileInfo y)
        {
            return x.FullName == y.FullName;
        }

        public int GetHashCode(FileInfo obj)
        {
            return obj.FullName.GetHashCode();
        }
    }
}
