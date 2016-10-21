using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marmot.Core
{
    public class ReleaseInfo
    {
        public List<RemoteFile> PackedFiles { get; } = new List<RemoteFile>();
        public List<RemoteFile> UnpackedFiles { get; } = new List<RemoteFile>();
        public Uri BaseDirectory { get; internal set; }
        public string VersionString { get; internal set; }

        public List<RemoteFile> ParseContentToFiles(string fileContent)
        {
            if (fileContent == null) throw new ArgumentNullException(nameof(fileContent));

            var files = new List<RemoteFile>();
            var fileEntries = fileContent.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
          
            fileEntries.AsParallel().ForAll((entry) => files.Add(new RemoteFile(BaseDirectory, entry)));
            
            return files;
        }
    }
}
