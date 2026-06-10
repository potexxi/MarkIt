using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkIt
{
    public class FileHistoryItem
    {
        public string Path {  get; set; }
        public FileManager.FileType Type {  get; set; }
        public FileHistoryItem() { }

        public FileHistoryItem(string path, FileManager.FileType type)
        {
            Path = path;
            Type = type;
        }
    }
}
