using ManyConsole;
using NewsDump.Lib.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsDump.Commands
{
    public class ResetCommand : ConsoleCommand
    {
        public ResetCommand()
        {
            IsCommand("ResetDatabase", "Deletes Database and reset to clean slate");
        }
        public override int Run(string[] remainingArguments)
        {
            Repository.ResetDb();
            return 1;
        }
    }
}
