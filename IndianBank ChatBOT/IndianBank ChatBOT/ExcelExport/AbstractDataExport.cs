using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.ExcelExport
{
    public interface IAbstractDataExport
    {
        byte[] Export<T>(List<T> exportData, string sheetName);
    }

    public abstract class AbstractDataExport : IAbstractDataExport
    {
        protected string _sheetName;
        protected string _fileName;
        protected List<string> _headers;
        protected List<string> _type;
        protected IWorkbook _workbook;
        protected ISheet _sheet;
        private const string DefaultSheetName = "Sheet1";

        public byte[] Export<T>(List<T> exportData, string sheetName = DefaultSheetName)
        {
            _sheetName = sheetName;

            _workbook = new XSSFWorkbook();
            _sheet = _workbook.CreateSheet(_sheetName);

            //var headerStyle = _workbook.CreateCellStyle();
            //var headerFont = _workbook.CreateFont();
            //headerFont.FontHeightInPoints = 12;
            //headerFont.IsBold = true;
            //headerStyle.SetFont(headerFont);

            WriteData(exportData);

            //Header
            //var header = _sheet.CreateRow(0);
            //for (var i = 0; i < _headers.Count; i++)
            //{
            //    var cell = header.CreateCell(i);
            //    cell.SetCellValue(_headers[i]);
            //    cell.CellStyle = headerStyle;
            //    // It's heavy, it slows down your Excel if you have large data                
            //    _sheet.AutoSizeColumn(i);
            //}
            using (var memoryStream = new MemoryStream())
            {
                _workbook.Write(memoryStream);

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Generic Definition to handle all types of List
        /// </summary>
        /// <param name="exportData"></param>
        public abstract void WriteData<T>(List<T> exportData);
    }
}
