using ManyConsole;
using NewsDump.Lib.Operations;
using Olive;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            s => Skip = s);

            HasOption("t|take=", "Pagination, takes the numeration",
            t => Take = t);

            HasOption("m|min=", "Minimum time for output selection, mm/dd/yyyy example: 06/15/2008",
            t => Min = t);
            HasOption("x|max=", "Maximum time for output selection, mm/dd/yyyy example: 06/15/2008",
            t => Max = t);

        }

        string FileLocation;
        string Skip;
        string Take;
        string Min;
        string Max;

        public override int Run(string[] remainingArguments)
        {
            if ((Skip.HasValue() && Take.HasValue()) || (Min.HasValue() && Max.HasValue()))
            {
                if (Min.HasValue() && Max.HasValue())
                {
                    var min = DateTime.ParseExact(Min, "d", CultureInfo.InvariantCulture);
                    var max = DateTime.ParseExact(Max, "d", CultureInfo.InvariantCulture);
                    ExportHandler.Export(FileLocation, min, max);
                }
                if (Skip.HasValue() && Take.HasValue())
                {

                    ExportHandler.Export(FileLocation, Skip.To<int>(), Take.To<int>());
                }

            }
            else
            {
                ExportHandler.Export(FileLocation);
            }
            return 1;
        }
    }
}
