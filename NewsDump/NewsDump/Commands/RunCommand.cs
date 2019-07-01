using ManyConsole;
using NewsDump.Lib.Model;
using NewsDump.Lib.Operations;
using Olive;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsDump.Commands
{
    class RunCommand : ConsoleCommand
    {
        public string Additional { get; set; }
        public RunCommand()
        {
            IsCommand("Run", "Runs the crawler command");
        }
        public override int Run(string[] remainingArguments)
        {

            NewsHandler.Run();

            

            return 1;
        }
    }
}
