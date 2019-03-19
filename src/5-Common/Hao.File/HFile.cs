using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hao.FileHelper
{
    public class HFile
    {
        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="fileName"></param>
        /// <param name="url"></param>
        /// <param name="tableTitle"></param>
        /// <param name="tableHeaders"></param>
        /// <param name="exportData"></param>
        /// <param name="mergedCells"></param>
        /// <param name="endMenus"></param>
        /// <returns></returns>
        public static async Task ExportToExcel(string fileName,string filePath,string tableTitle, List<Dictionary<string, string>> exportData, IEnumerable<int> mergedCells = null, List<string> endMenus = null)
        {

            await Task.Factory.StartNew(() => {
                using (Stream stream = new FileStream(filePath, FileMode.CreateNew)) //FileMode.CreateNew 当文件不存在时，创建新文件；如果文件存在，则引发异常。
                {

                    IWorkbook wk = new XSSFWorkbook();
                    ISheet sheet = wk.CreateSheet();

                    //表头单元格格式
                    ICellStyle headStyle = wk.CreateCellStyle();
                    headStyle.VerticalAlignment = VerticalAlignment.Center;//垂直对齐
                    headStyle.Alignment = HorizontalAlignment.Center;//水平对齐
                    headStyle.IsHidden = false;
                    IFont headFontStyle = wk.CreateFont();
                    headFontStyle.FontName = "微软雅黑";//字体
                    headFontStyle.FontHeightInPoints = 12;//字号
                    headFontStyle.Boldweight = 800;//粗体
                    headStyle.SetFont(headFontStyle);
                    headStyle.WrapText = true;


                    ICellStyle cellStyle = wk.CreateCellStyle();
                    cellStyle.VerticalAlignment = VerticalAlignment.Center;//垂直对齐
                    cellStyle.Alignment = HorizontalAlignment.Center;//水平对齐
                    cellStyle.IsHidden = false;
                    IFont fontStyle = wk.CreateFont();
                    fontStyle.FontName = "微软雅黑";//字体
                    fontStyle.FontHeightInPoints = 11;//字号
                    fontStyle.Boldweight = 700;//粗体
                    cellStyle.SetFont(fontStyle);

                    var keys = exportData.FirstOrDefault().Keys;
                    int maxColumn = keys.Count();

                    //创建表头
                    if (!string.IsNullOrWhiteSpace(tableTitle))
                    {
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, maxColumn - 1));
                        IRow nameRow = sheet.CreateRow(0);
                        nameRow.ZeroHeight = false;
                        ICell nameCell = nameRow.CreateCell(0);
                        nameCell.SetCellValue(tableTitle);
                        nameCell.CellStyle = headStyle;
                    }
                    else
                    {
                        IRow index = sheet.CreateRow(0);
                        index.ZeroHeight = false;
                        for (int i = 0; i < maxColumn; i++)
                        {
                            ICell cell = index.CreateCell(i);
                            cell.SetCellValue(i + 1);
                            cell.CellStyle = cellStyle;
                        }
                    }

                    IRow headerRow = sheet.CreateRow(1);
                    headerRow.ZeroHeight = false;
                    headerRow.HeightInPoints = 24;
                    int colIndex = 0;
                    foreach (var colName in keys)
                    {
                        ICell cell = headerRow.CreateCell(colIndex);
                        cell.SetCellValue(colName);
                        cell.CellStyle = cellStyle;
                        colIndex++;
                    }


                    int count = 2;
                    IRow childRow = null;
                    int lastCount = 0;

                    lastCount = count;
                    for (int childIndex = 0; childIndex < exportData.Count(); childIndex++)
                    {
                        childRow = sheet.CreateRow(count++);
                        childRow.ZeroHeight = false;

                        var data = exportData[childIndex];
                        int cellNum = 0;
                        foreach (var col in data)
                        {
                            ICell childCell = childRow.CreateCell(cellNum);
                            childCell.SetCellValue(col.Value);
                            cellNum++;
                        }
                    }

                    if (mergedCells != null && mergedCells.Count() > 0)
                    {
                        foreach (int cellNum in mergedCells)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(lastCount, count - 1, cellNum, cellNum));
                        }
                    }

                    if (endMenus != null)
                    {
                        foreach (var item in endMenus)
                        {
                            IRow endRow = sheet.CreateRow(count++);
                            endRow.ZeroHeight = false;
                            ICell oneCell = endRow.CreateCell(0);
                            oneCell.SetCellValue(item);
                        }
                    }
                    wk.Write(stream);
                }
            });
         
        }


        #region 文件相关操作
        public static void CreateFile(string filePath, string text, Encoding encoding)
        {
            try
            {
                if (IsExistFile(filePath))
                {
                    DeleteFile(filePath);
                }
                if (!IsExistFile(filePath))
                {
                    string directoryPath = GetDirectoryFromFilePath(filePath);
                    CreateDirectory(directoryPath);

                    FileInfo file = new FileInfo(filePath);
                    using (FileStream stream = file.Create())
                    {
                        using (StreamWriter writer = new StreamWriter(stream, encoding))
                        {
                            writer.Write(text);
                            writer.Flush();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
        public static void CreateDirectory(string directoryPath)
        {
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        public static void DeleteFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                File.Delete(filePath);
            }
        }
        public static string GetDirectoryFromFilePath(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            DirectoryInfo directory = file.Directory;
            return directory.FullName;
        }
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        #endregion
    }
}
