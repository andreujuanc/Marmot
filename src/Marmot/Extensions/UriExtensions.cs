using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class UriExtensions
    {
        public static Uri GetDirectory(this Uri uri)
        {
            if (uri.AbsoluteUri.EndsWith("\\") || uri.AbsoluteUri.EndsWith("/"))
                return uri;
            return new Uri(uri, ".");
        }

        public static string GetLastSegment(this Uri uri)
        {
            return uri.Segments.Last();
        }
        public static Uri EnsureDirectory(this Uri uri)
        {
            if (uri.AbsoluteUri.EndsWith("\\") || uri.AbsoluteUri.EndsWith("/"))
                return uri;
            return new Uri(uri.AbsoluteUri + "/");
        }
        public static Uri Combine(this Uri uri, string fragment)
        {
            return new Uri(Path.Combine(uri.EnsureDirectory().AbsoluteUri, fragment));
        }
    }
}

