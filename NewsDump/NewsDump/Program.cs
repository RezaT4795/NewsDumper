using ManyConsole;
using NewsDump.Commands;
using NewsDump.Lib.Model;
using NewsDump.Lib.Operations;
using System;

namespace NewsDump
{
    class Program
    {
        static int Main(string[] args)
        {
            var commands = ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
            var result =  ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);

#if DEBUG
            WitForExit();
#endif
            return result;
        }

        private static void WitForExit()
        {
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
