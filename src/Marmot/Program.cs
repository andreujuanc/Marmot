using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using ManyConsole;

namespace Marmot
{  
    public class Program
    {
        public static int Main(string[] args)
        {
            var commands = GetCommands();
            var result = ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
            return result;
        }

        public static IEnumerable<ConsoleCommand> GetCommands()
        {
            return ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
        }
    }
}
