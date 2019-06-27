using ManyConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsDump.Commands
{
    public class OtherCommand : ConsoleCommand
    {
        public OtherCommand()
        {
            IsCommand("Nothing", "Does absolutely nothing.");
        }
        public override int Run(string[] remainingArguments)
        {
            Console.WriteLine("Absolutely nothing.");
            return 1;
        }
    }
}
