﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marmot.Core
{
    public class AppManager
    {
        public Uri AppUri { get; set; }

        public AppManager(Uri uri)
        {
            AppUri = uri;
        }

        internal ReleaseInfo GetLatestRelease()
        {

            return null;
        }
    }
}
