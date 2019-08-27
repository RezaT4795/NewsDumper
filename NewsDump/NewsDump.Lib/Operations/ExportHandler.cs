using NewsDump.Lib.Data;
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
            var repo = Repository.Of<News>();
            var news = NewsOperation.GetAllNews(repo);
            WriteToExcel(news, path);
        }
        public static void Export(string path, int skip, int take)
        {
            var repo = Repository.Of<News>();
            var news = repo.Take(take, skip);
            WriteToExcel(news, path);
        }
        public static void Export(string path, DateTime? min, DateTime? max)
        {
            var repo = Repository.Of<News>();
            var news = NewsOperation.Where(repo, x => x.PublishDate < max && x.PublishDate > min);
            WriteToExcel(news, path);
        }
        static void WriteToExcel(IEnumerable<News> customerObjects, string path)
        {
            customerObjects = customerObjects.OrderByDescending(x => x.PublishDate);
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
                EventBus.Notify("ذخیره شد!", "DoneOperation");
            }
            else
            {
                EventBus.Notify("There is nothing to save, skipping", "info");
                EventBus.Notify("چیزی برای ذخیره وجود ندارد", "DoneOperation");
            }

        }
    }
}
