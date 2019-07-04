using ManyConsole;
using Microsoft.EntityFrameworkCore;
using NewsDump.Commands;
using NewsDump.Lib.Data;
using NewsDump.Lib.Model;
using NewsDump.Lib.Operations;
using NewsDump.Lib.Util;
using System;

namespace NewsDump
{
    class Program
    {
        static int Main(string[] args)
        {
            Repository.PerformMigration();

            //Register for event
            EventBus.OnMessageFired += EventBus_OnMessageFired;

            var commands = ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
            var result = ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);

#if DEBUG
            WitForExit();
#endif
            return result;
        }

        private static void EventBus_OnMessageFired(MessageArgs message) => Console.WriteLine(message.ToString());

        private static void WitForExit()
        {
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
