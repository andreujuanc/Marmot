using ManyConsole;
using Marmot.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marmot.Commands
{
    public class InstallCommand : ConsoleCommand
    {
        public string AppUrl { get; set; }

        public InstallCommand()
        {
            IsCommand("install", "installs app");

            HasLongDescription("Install an app from a source url");

            // Required options/flags, append '=' to obtain the required value.
            HasRequiredOption("u|url=", "The url of the app's location.", p => AppUrl = p);
        }

        public override int Run(string[] remainingArguments)
        {
            var install = new InstallProcess(this);
            var task = Task.Run(async () => await install.Start());
            task.Wait();
            return task.Result;
        }
    }
}
