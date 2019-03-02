using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Mosaic.Utils.Utils
{
    public class ExcelHelper
    {
        public static DataTable ImportExcel(Stream ms)
        {
            DataTable dt = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            byte[] buf = new byte[ms.Length];
            ms.Read(buf, 0, buf.Length);
            ms.Close();
            try
            {
                try
                {
                    workbook = new HSSFWorkbook(new MemoryStream(buf));
                }
                catch
                {
                    try
                    {
                        workbook = new XSSFWorkbook(new MemoryStream(buf));
                    }
                    catch { }
                }

                if (workbook == null)
                    return dt;

                sheet = workbook.GetSheetAt(0);
                dt = new DataTable();
                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue.ToLower());
                    dt.Columns.Add(column);
                }
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = dt.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        ICell cell = row.GetCell(j);
                        if (cell == null)
                        {
                            dataRow[j] = "";
                        }
                        else
                        {
                            dataRow[j] = cell.ToString();
                        }
                    }
                    dt.Rows.Add(dataRow);
                }
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (dt != null)
                { dt.Dispose(); }
                workbook = null;
                sheet = null;
            }
        }

        public static byte[] ExportToExcel(DataTable dt, string sheetName)
        {
            IWorkbook workbook = null;//全局workbook
            ISheet sheet;//sheet
            workbook = new HSSFWorkbook();
            sheet = workbook.CreateSheet(sheetName);
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.Alignment = HorizontalAlignment.Center;

            ICellStyle headStyle = workbook.CreateCellStyle();
            headStyle.BorderBottom = BorderStyle.Thin;
            headStyle.BorderLeft = BorderStyle.Thin;
            headStyle.BorderRight = BorderStyle.Thin;
            headStyle.BorderTop = BorderStyle.Thin;
            headStyle.VerticalAlignment = VerticalAlignment.Center;
            headStyle.Alignment = HorizontalAlignment.Center;
            IFont font = workbook.CreateFont();
            font.Boldweight = 700;
            headStyle.SetFont(font);

            IRow header = sheet.CreateRow(0);
            header.HeightInPoints = 18.75f;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = header.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);
                cell.CellStyle = headStyle;
            }
            int index = 1;
            foreach (DataRow row in dt.Rows)
            {
                IRow workRow = sheet.CreateRow(index++);
                workRow.HeightInPoints = 18.75f;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell = workRow.CreateCell(i);
                    cell.SetCellValue(row[i].ToString());
                    cell.CellStyle = cellStyle;
                }

            }
            for (int columnNum = 0; columnNum < dt.Columns.Count; columnNum++)
            {
                int columnWidth = sheet.GetColumnWidth(columnNum) / 256;//获取当前列宽度  
                for (int rowNum = 0; rowNum <= sheet.LastRowNum; rowNum++)//在这一列上循环行  
                {
                    IRow currentRow = sheet.GetRow(rowNum);
                    ICell currentCell = currentRow.GetCell(columnNum);
                    int length = Encoding.UTF8.GetBytes(currentCell.ToString()).Length;//获取当前单元格的内容宽度  
                    if (columnWidth < length + 1)
                    {
                        columnWidth = length + 1;
                    }//若当前单元格内容宽度大于列宽，则调整列宽为当前单元格宽度，后面的+1是我人为的将宽度增加一个字符  
                }
                sheet.SetColumnWidth(columnNum, columnWidth * 256);
            }
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            byte[] buf = ms.ToArray();
            ms.Flush();
            return buf;
        }
    }
}
