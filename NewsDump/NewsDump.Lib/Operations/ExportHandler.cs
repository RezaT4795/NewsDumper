using NewsDump.Lib.Model;
using Syncfusion.Drawing;
using Syncfusion.XlsIO;
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
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                var application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2016;




                //Create a new workbook
                var workbook = application.Workbooks.Create(1);
                var sheet = workbook.Worksheets[0];

                //Import data from customerObjects collection
                sheet.ImportData(customerObjects, 5, 1, false);

                #region Define Styles
                var pageHeader = workbook.Styles.Add("PageHeaderStyle");
                var tableHeader = workbook.Styles.Add("TableHeaderStyle");

                pageHeader.Font.RGBColor = Color.FromArgb(0, 83, 141, 213);
                pageHeader.Font.FontName = "Calibri";
                pageHeader.Font.Size = 18;
                pageHeader.Font.Bold = true;
                pageHeader.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                pageHeader.VerticalAlignment = ExcelVAlign.VAlignCenter;

                tableHeader.Font.Color = ExcelKnownColors.White;
                tableHeader.Font.Bold = true;
                tableHeader.Font.Size = 11;
                tableHeader.Font.FontName = "Calibri";
                tableHeader.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                tableHeader.VerticalAlignment = ExcelVAlign.VAlignCenter;
                tableHeader.Color = Color.FromArgb(0, 118, 147, 60);
                tableHeader.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                tableHeader.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                tableHeader.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                tableHeader.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                #endregion

                #region Apply Styles
                //Apply style to the header
                //sheet["A1"].Text = "Yearly Sales Report";
                //sheet["A1"].CellStyle = pageHeader;

                //sheet["A2"].Text = "Namewise Sales Comparison Report";
                //sheet["A2"].CellStyle = pageHeader;
                //sheet["A2"].CellStyle.Font.Bold = false;
                //sheet["A2"].CellStyle.Font.Size = 16;

                //sheet["A1:D1"].Merge();
                //sheet["A2:D2"].Merge();
                //sheet["A3:A4"].Merge();
                //sheet["D3:D4"].Merge();
                //sheet["B3:C3"].Merge();

                //sheet["B3"].Text = "Sales";
                //sheet["A3"].Text = "Sales Person";
                //sheet["B4"].Text = "January - June";
                //sheet["C4"].Text = "July - December";
                //sheet["D3"].Text = "Change(%)";
                //sheet["A3:D4"].CellStyle = tableHeader;
                //sheet.UsedRange.AutofitColumns();
                //sheet.Columns[0].ColumnWidth = 24;
                //sheet.Columns[1].ColumnWidth = 21;
                //sheet.Columns[2].ColumnWidth = 21;
                //sheet.Columns[3].ColumnWidth = 16;
                #endregion

                sheet.UsedRange.AutofitColumns();

                //Save the file in the given path
                Stream excelStream = File.Create(Path.Combine(Path.GetFullPath(path), $"{DateTime.UtcNow.Ticks}.xlsx"));
                workbook.SaveAs(excelStream);
                excelStream.Dispose();
            }
        }
    }
}
