using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Marmot.Core
{
    public class Package
    {
        public static void ExtractZipDecoded(string zipFilePath, string outFolder)
        {
            var zf = new ZipFile(zipFilePath);

            var buffer = new byte[64 * 1024];

            try
            {
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile) continue;

                    var entryFileName = Uri.UnescapeDataString(zipEntry.Name);

                    var fullZipToPath = Path.Combine(outFolder, entryFileName);
                    var directoryName = Path.GetDirectoryName(fullZipToPath);

                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    var zipStream = zf.GetInputStream(zipEntry);

                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                zf.Close();
            }
        }

        public async Task ExtractZipForInstall(string zipFilePath, string outFolder)
        {
            var zf = new ZipFile(zipFilePath);
            var entries = zf.OfType<ZipEntry>().ToArray();
            var re = new Regex(@"lib[\\\/][^\\\/]*[\\\/]", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

            try
            {
                //await Utility.ForEachAsync(entries, (zipEntry) => 
                Parallel.ForEach(entries, (zipEntry) =>
                {
                    if (!zipEntry.IsFile) return;

                    var entryFileName = Uri.UnescapeDataString(zipEntry.Name);
                    if (!re.Match(entryFileName).Success) return;

                    var fullZipToPath = Path.Combine(outFolder, re.Replace(entryFileName, "", 1));
                    var directoryName = Path.GetDirectoryName(fullZipToPath);

                    var buffer = new byte[64 * 1024];

                    if (directoryName.Length > 0)
                    {
                        //Utility.Retry(() => 
                        Directory.CreateDirectory(directoryName);
                            //, 2);
                    }

                   // Utility.Retry(() => {
                        using (var zipStream = zf.GetInputStream(zipEntry))
                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                   // }, 5);
                });
            }
            finally
            {
                zf.Close();
            }
        }


    }
}
