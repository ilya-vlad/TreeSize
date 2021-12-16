using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Models
{
    public class NodeInfo
    {
        public FileSystemInfo Info;

        public Node ParentNode;        

        public NodeInfo(FileInfo fileInfo, Node parentNode)
        {
            Info = fileInfo;
            ParentNode = parentNode;            
        }

        public NodeInfo(DirectoryInfo dirInfo, Node parentNode)
        {
            Info = dirInfo;
            ParentNode = parentNode;            
        }
    }
}
