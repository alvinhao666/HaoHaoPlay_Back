using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hao.Excel
{
    public partial class H_Excel
    {
        /// <summary>
        /// 导出excel (EPPlus)
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="exportData"></param>
        /// <param name="tableTitle"></param>
        /// <returns></returns>
        public static async Task ExportByEPPlus(string filePath, IEnumerable<Dictionary<string, string>> exportData, string tableTitle = null)
        {
            await Task.Factory.StartNew(() =>
            {
                using (Stream stream = new FileStream(filePath, FileMode.CreateNew)) //FileMode.CreateNew 当文件不存在时，创建新文件；如果文件存在，则引发异常。
                {
                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet ws = package.Workbook.Worksheets.Add(string.IsNullOrWhiteSpace(tableTitle) ? "Sheet0" : tableTitle);

                        var keys = exportData.FirstOrDefault().Keys;   
                        
                        int colIndex = 1;
                        foreach (var colName in keys)
                        {
                            var headerCol = ws.Cells[1, colIndex];
                            headerCol.Value = colName;
                            headerCol.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            headerCol.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            headerCol.Style.Font.Bold = true;
                            headerCol.Style.Font.Name = "微软雅黑";
                            headerCol.Style.Font.Size = 12;
                            colIndex++;
                        }
                        int row = 2;
                        foreach (var item in exportData)
                        {
                            int cellNum = 1;
                            foreach (var data in item)
                            {
                                var col = ws.Cells[row, cellNum++];
                                col.Value = data.Value;
                                col.Style.Font.Name = "微软雅黑";
                            }
                            row++;
                        }
                        package.Save();
                    }
                }
            });
        }

        /// <summary>
        /// 导出excel（NPOI）
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="tableTitle"></param>
        /// <param name="exportData"></param>
        /// <returns></returns>
        public static async Task ExportByNPOI(string filePath, IEnumerable<Dictionary<string, string>> exportData, string tableTitle = null)
        {
            await Task.Factory.StartNew(() =>
            {
                using (Stream stream = new FileStream(filePath, FileMode.CreateNew)) //FileMode.CreateNew 当文件不存在时，创建新文件；如果文件存在，则引发异常。
                {

                    IWorkbook wk = new XSSFWorkbook();
                    ISheet sheet = string.IsNullOrWhiteSpace(tableTitle) ? wk.CreateSheet() : wk.CreateSheet(tableTitle);

                    ICellStyle headerStyle = wk.CreateCellStyle();
                    headerStyle.VerticalAlignment = VerticalAlignment.Center;//垂直对齐
                    headerStyle.Alignment = HorizontalAlignment.Center;//水平对齐
                    IFont fontStyle = wk.CreateFont();
                    fontStyle.FontName = "微软雅黑";//字体
                    fontStyle.FontHeightInPoints = 12;//字号
                    fontStyle.IsBold = true;//粗体
                    headerStyle.SetFont(fontStyle);


                    var keys = exportData.FirstOrDefault().Keys;

                    IRow headerRow = sheet.CreateRow(0);
                    headerRow.ZeroHeight = false;
                    headerRow.HeightInPoints = 24;
                    int colIndex = 0;
                    foreach (var colName in keys)
                    {
                        ICell cell = headerRow.CreateCell(colIndex++);
                        cell.SetCellValue(colName);
                        cell.CellStyle = headerStyle;
                    }

                    ICellStyle cellStyle = wk.CreateCellStyle();
                    IFont cellFontStyle = wk.CreateFont();
                    cellFontStyle.FontName = "微软雅黑";//字体
                    cellFontStyle.FontHeightInPoints = 11;//字号
                    cellStyle.SetFont(cellFontStyle);

                    int count = 1;
                    foreach(var item in exportData)
                    {
                        IRow childRow = sheet.CreateRow(count++);
                        int cellNum = 0;
                        foreach (var col in item)
                        {
                            ICell childCell = childRow.CreateCell(cellNum++);
                            childCell.SetCellValue(col.Value);
                            childCell.CellStyle = cellStyle;
                        }
                    }
                    wk.Write(stream);
                }
            });
        }
    }
}
