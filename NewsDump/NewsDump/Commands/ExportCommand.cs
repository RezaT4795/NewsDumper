using ManyConsole;
using NewsDump.Lib.Operations;
using Olive;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsDump.Commands
{
    public class ExportCommand : ConsoleCommand
    {
        public ExportCommand()
        {
            IsCommand("Export", "Exports to excel file");
            HasRequiredOption("f|folder=", "The folder path of exported file", p => FileLocation = p);

            HasOption("s|skip=", "Pagination, skips the numeration",
            t => Skip = t);

            HasOption("t|take=", "Pagination, takes the numeration",
            t => Take = t);

        }

        string FileLocation;
        string Skip;
        string Take;

        public override int Run(string[] remainingArguments)
        {
            if (Skip.HasValue() && Take.HasValue())
            {
                ExportHandler.Export(FileLocation, Skip.To<int>(), Take.To<int>());
            }
            else
            {
                ExportHandler.Export(FileLocation);
            }
            return 1;
        }
    }
}
