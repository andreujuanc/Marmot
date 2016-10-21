using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marmot.Core
{
    public class AppManager
    {
        public string ApplicationName { get; private set; }
        public Uri AppUri { get; set; }
        public Uri MarmotReleaseFile { get { return new Uri(AppUri, "RELEASES"); } }

        public string InstallPath
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                
                return Path.Combine(path, ApplicationName);
            }
        }

        public string TempPath
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                return Path.Combine(path, ApplicationName, "temp");
            }
        }

        public AppManager(Uri uri, string applicationName = null)
        {
            uri = uri.EnsureDirectory();
            ApplicationName = applicationName;
            if (ApplicationName == null) throw new NotImplementedException("TODO: Get appname from foldername");
            AppUri = uri;
        }

        public async Task<ReleaseInfo> GetLatestRelease()
        {
            var releaseFile = new RemoteFile(MarmotReleaseFile);
            var fileContent = await releaseFile.DownloadString();

            var result = new ReleaseInfo();
            result.BaseDirectory = MarmotReleaseFile.GetDirectory();
            var allfiles = result.ParseContentToFiles(fileContent);
            var package = allfiles.OrderByDescending(x => x.FileName).Take(1);
            result.PackedFiles.AddRange(package);
            result.VersionString = GetVersionFromFile(package.First());
            return result;
        }

        private string GetVersionFromFile(RemoteFile remoteFile)
        {

            return remoteFile.FileName.Substring(0, remoteFile.FileName.LastIndexOf('.'));
        }
    }
}
