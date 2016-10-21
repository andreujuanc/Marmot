using Marmot.Commands;
using Marmot.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marmot.Processes
{
    public class InstallProcess
    {
        private InstallCommand Command { get; set; }

        private AppManager Manager { get; set; }
        public Uri InstallPathWithVersion { get; set; }
        public Uri TempPathWithVersion { get; set; }

        public InstallProcess(InstallCommand command)
        {
            this.Command = command;
            this.Manager = new AppManager(new Uri(Command.AppUrl), "MarmotTest");
        }

        internal async Task<int> Start()
        {
            var release =await Manager.GetLatestRelease();
            if (IsInstalled(release)) return 0;

            InstallPathWithVersion = new Uri(PathWithVersion(Manager.InstallPath, release));
            InstallPathWithVersion = InstallPathWithVersion.EnsureDirectory();

            TempPathWithVersion = new Uri(PathWithVersion(Manager.TempPath, release));
            TempPathWithVersion = TempPathWithVersion.EnsureDirectory();

            CleanTempFolder();
            await Download(release);
            await Unpack(release);
            await Check(release);
            await Apply(release);            

            return 0;
        }

        private bool IsInstalled(ReleaseInfo release)
        {
            //TODO: Implement
            return false;
        }

        private async Task Unpack(ReleaseInfo release)
        {
#if DEBUG
            Console.WriteLine("Decompress");
#endif
            release.PackedFiles.AsParallel().ForAll((file) => {
                file.Unpack();
            });
            return;
        }

        public void CleanTempFolder()
        {
            if(Directory.Exists(Manager.TempPath))
                Directory.Delete(Manager.TempPath, true);
        }

        internal async Task Check(ReleaseInfo release)
        {
#if DEBUG
            Console.WriteLine("Check");
#endif
            return;
        }

        internal async Task Apply(ReleaseInfo release)
        {
#if DEBUG
            Console.WriteLine("Apply");
#endif
            foreach (var file in release.UnpackedFiles)
            {
                var tempFile = file.PathFrom(PathWithVersion(Manager.TempPath, release));
                var installFile = file.PathFrom(InstallPathWithVersion.AbsolutePath);
                CreateFolder(InstallPathWithVersion.AbsolutePath);

                File.Copy(tempFile, installFile, true);
            }
            return;
        }

        private string PathWithVersion(string basePath, ReleaseInfo release)
        {
            return Path.Combine(basePath, release.VersionString);
        }

        internal async Task Download(ReleaseInfo release)
        {
#if DEBUG
            Console.WriteLine("Download");
#endif
            CreateFolder(TempPathWithVersion.AbsolutePath);
            var result = Parallel.ForEach(release.PackedFiles, (file) => 
                            file.DownloadTo(TempPathWithVersion)
                            );
            var a = result.IsCompleted;

            return;
        }

        private void CreateFolder(string tempFolder)
        {
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);
        }
    }
}
