using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Service
{
    public class FolderAnalyzer
    {
        public string[] GetFilesInFolder(string path)
        {
            string[] files = Directory.GetFiles(path);
            return files;
        }
    }
}
