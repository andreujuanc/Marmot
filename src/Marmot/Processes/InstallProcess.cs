using Marmot.Commands;
using Marmot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marmot.Processes
{
    public class InstallProcess
    {
        private InstallCommand Command { get; set; }

        private AppManager Manager { get; set; }

        public InstallProcess(InstallCommand command)
        {
            this.Command = command;
            this.Manager = new AppManager(new Uri(Command.AppUrl));
        }

        internal int Start()
        {
            var release = Manager.GetLatestRelease();
            return 0;
        }
    }
}
