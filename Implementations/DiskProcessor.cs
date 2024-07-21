using ConcurrentFileReading.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentFileReading.Implementations
{
    internal class DiskProcessor : IDiskProcessor
    {
        public FileInfo[] GetDirecotryFiles(string dirPath)
        {
            var dirInfo = new DirectoryInfo(dirPath);
            return dirInfo.GetFiles();
        }

        //public int GetFileCharCount(string fileName, char chr)
        //{
        //    var text = File.ReadAllText(fileName, Encoding.UTF8);

        //    int count = 0;
        //    foreach (var c in text.AsSpan())
        //    {
        //        if (c == chr)
        //        {
        //            count++;
        //        }
        //    }
        //    return count;
        //}

        public int GetFileCharCount(string fileName, char chr)
        {
            var text = File.ReadAllText(fileName, Encoding.UTF8);
            Thread.Sleep(1000);
            return text.Count(c => c == chr);
        }
    }
}
