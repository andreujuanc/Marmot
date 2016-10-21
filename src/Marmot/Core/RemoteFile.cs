using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Marmot.Core
{
    public class RemoteFile
    {
        public Uri FileUri { get; set; }
        public string Hash { get; set; }
        public int Size { get; set; }
        public string FileName { get { return FileUri.GetLastSegment(); } }
        public bool IsDownloaded { get; set; }
        public string DownloadedFullPath { get; set; }
        public RemoteFile(Uri uri)
        {
            FileUri = uri;
        }
        public RemoteFile(Uri baseUrl, string entry)
        {
            //TODO: Validate null;
            var columns = entry.Split('\t');
            //TODO: Validate columns;
            Hash = columns[0];
            var filename = columns[1];
            FileUri = new Uri(baseUrl, filename);
            Size = int.Parse(columns[2]);
        }

        public void DownloadTo(string tempFolder) { DownloadTo(new Uri(tempFolder)); }
        public void DownloadTo(Uri tempFolder)
        {
            using (var client = new WebClient())
            {
                try
                {
                    var tmp = new Uri(tempFolder, FileName).AbsolutePath;
                    client.DownloadFile(FileUri, tmp);
                    IsDownloaded = true;
                    DownloadedFullPath = tmp;
                }
                catch(Exception ex)
                {

                }
            }
        }


        public async Task<string> DownloadString()
        {
            using (var client = new WebClient())
            {
                try
                {
                    return await client.DownloadStringTaskAsync(FileUri);
                }
                catch(Exception ex)
                {
                    return null;
                }
            }
        }

        internal async void Unpack()
        {
            var path = new Uri(DownloadedFullPath).GetDirectory().Combine("extracted").AbsolutePath;
            var pack = new Package();
            await pack.ExtractZipForInstall(DownloadedFullPath, path);
        }

        internal string PathFrom(string basePath)
        {
            return Path.Combine(basePath, FileName);
        }
    }
}
