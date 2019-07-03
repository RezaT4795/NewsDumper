using NewsDump.Lib.Model;
using NewsDump.Lib.Util;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Olive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NewsDump.Lib.Operations
{
    public static class ExportHandler
    {

        public static void Export(string path)
        {
            var news = NewsOperation.GetAllNews();
            WriteToExcel(news, path);
        }
        public static void Export(string path, int skip, int take)
        {
            var news = NewsOperation.GetAllNews().Skip(skip).Take(take);
            WriteToExcel(news, path);
        }
        static void WriteToExcel(IEnumerable<News> customerObjects, string path)
        {
            if (customerObjects.Any())
            {
                var expath = Path.Combine(path.AsDirectory().FullName, $"{DateTime.UtcNow.Ticks}.xlsx");

                var pck = new ExcelPackage();
                var wsEnum = pck.Workbook.Worksheets.Add("News sheet");
                wsEnum.DefaultColWidth = 18;
                wsEnum.Cells["A1"].LoadFromCollection(customerObjects, true, TableStyles.Light1);
                wsEnum.Cells[2, 5, customerObjects.Count() + 1, 5].Style.Numberformat.Format = "dd-MM-yy";

                pck.SaveAs(expath.AsFile());
                EventBus.Notify($"{customerObjects.Count()} rows saved as {expath}", "info");
            }
            else
            {
                EventBus.Notify("There is nothing to save, skipping", "info");
            }

        }
    }
}
