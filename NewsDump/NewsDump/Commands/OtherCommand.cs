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
            IsCommand("Export", "Exports to excel file");
        }
        public override int Run(string[] remainingArguments)
        {
            Console.WriteLine("Absolutely nothing.");
            return 1;
        }
    }
}
