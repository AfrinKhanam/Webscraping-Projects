using IndianBank_ChatBOT.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.ExcelExport
{
    public class LeadGenerationExportBridge : AbstractDataExport
    {
        public LeadGenerationExportBridge()
        {
            _headers = new List<string>();
            _type = new List<string>();
        }

        Tuple<string, string, Type>[] reportColumns;
        public override void WriteData<T>(List<T> exportData, string from, string to)
        {
            DataTable table = new DataTable();
            var columnsList = new List<string>();
            var headerColumnList = new List<string>();

            reportColumns = new Tuple<string, string, Type>[] {
                                                    Tuple.Create("S No.","", typeof(int)),
                                                    Tuple.Create("Visitor","Visitor", typeof(string)),
                                                    Tuple.Create("Phone Number","PhoneNumber", typeof(string)),
                                                    Tuple.Create("Queried On", "QueriedOn", typeof(DateTime)),
                                                    Tuple.Create("Action","LeadGenerationAction", typeof(string))
                                                 };

            foreach (var item in reportColumns)
            {
                table.Columns.Add(item.Item1, item.Item3);
                _type.Add(item.Item3.Name);
                _headers.Add(item.Item1);
            }

            // Generating the Data Rows
            int slNo = 1;

            foreach (var item in exportData as List<LeadGenerationInfo>)
            {
                DataRow row = table.NewRow();
                row["S No."] = slNo;
                row["Visitor"] = item.Visitor;
                row["Phone Number"] = item.PhoneNumber;
                row["Queried On"] = item.QueriedOn;
                row["Action"] = item.LeadGenerationAction;

                slNo += 1;
                table.Rows.Add(row);
            }

            IRow sheetRow = null;

            var data = exportData as List<LeadGenerationInfo>;

            var groupedLeadGenerationInfo = data
                                            .GroupBy(u => u.DomainName)
                                            .Select(grp => grp.ToList())
                                            .ToList();

            var totalLeadGenerationQueries = data.Count();
            var totalUnattendedQueries = data.Where(d => !d.LeadGenerationAction.HasValue).Count();

            sheetRow = _sheet.CreateRow(0);
            ICell reportHeaderCell = sheetRow.CreateCell(0);
            reportHeaderCell.SetCellValue("Indian Bank - ChatBOT Insights");

            var reportHeaderStyle = _workbook.CreateCellStyle();

            var reportHeaderFont = _workbook.CreateFont();
            reportHeaderFont.IsBold = true;
            reportHeaderFont.FontName = "Calibri";
            reportHeaderFont.Boldweight = (short)FontBoldWeight.Bold;
            reportHeaderFont.FontHeightInPoints = 14;
            reportHeaderFont.Color = IndexedColors.BlueGrey.Index;

            reportHeaderStyle.SetFont(reportHeaderFont);
            reportHeaderStyle.Alignment = HorizontalAlignment.Left;
            reportHeaderStyle.WrapText = true;

            reportHeaderCell.CellStyle = reportHeaderStyle;

            var cra = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 2);
            _sheet.AddMergedRegion(cra);

            //From 
            ICell reportFromCell = sheetRow.CreateCell(3);
            reportFromCell.SetCellValue("From");

            var reportFromStyle = _workbook.CreateCellStyle();
            reportFromStyle.Alignment = HorizontalAlignment.Right;
            reportFromCell.CellStyle = reportFromStyle;

            var reportFromFont = _workbook.CreateFont();
            reportFromFont.IsItalic = true;

            reportFromStyle.SetFont(reportFromFont);

            reportFromCell.CellStyle = reportFromStyle;

            //To
            ICell reportToCell = sheetRow.CreateCell(4);
            reportToCell.SetCellValue("To");

            var reportToStyle = _workbook.CreateCellStyle();
            reportToStyle.Alignment = HorizontalAlignment.Right;
            reportToCell.CellStyle = reportToStyle;

            var reportToFont = _workbook.CreateFont();
            reportToFont.IsItalic = true;

            reportToStyle.SetFont(reportToFont);

            reportToCell.CellStyle = reportToStyle;

            //Report Sub heading
            sheetRow = _sheet.CreateRow(1);
            var reportSubHeadingCell = sheetRow.CreateCell(0);
            reportSubHeadingCell.SetCellValue("Leads Generated");

            var reportSubHeadingStyle = _workbook.CreateCellStyle();

            var reportSubHeadingFont = _workbook.CreateFont();
            reportSubHeadingFont.IsBold = true;
            reportSubHeadingFont.FontName = "Calibri";
            reportSubHeadingFont.Boldweight = (short)FontBoldWeight.Bold;
            reportSubHeadingFont.FontHeightInPoints = 12;
            reportSubHeadingFont.IsItalic = true;

            reportSubHeadingStyle.SetFont(reportSubHeadingFont);
            reportSubHeadingStyle.Alignment = HorizontalAlignment.Left;
            reportSubHeadingStyle.WrapText = true;

            reportSubHeadingCell.CellStyle = reportSubHeadingStyle;

            _sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 2));


            //From Data
            var reportFromDataCell = sheetRow.CreateCell(3);
            reportFromDataCell.SetCellValue(from);

            var reportFromDataStyle = _workbook.CreateCellStyle();
            reportFromDataStyle.BorderLeft = BorderStyle.Thin;
            reportFromDataStyle.BorderTop = BorderStyle.Thin;
            reportFromDataStyle.BorderRight = BorderStyle.Thin;
            reportFromDataStyle.BorderBottom = BorderStyle.Thin;

            var reportFromDataFont = _workbook.CreateFont();
            reportFromDataFont.FontName = "Calibri";
            reportFromDataFont.FontHeightInPoints = 11;
            reportFromDataFont.IsItalic = true;

            reportFromDataStyle.SetFont(reportFromDataFont);
            reportFromDataStyle.Alignment = HorizontalAlignment.Right;

            reportFromDataCell.CellStyle = reportFromDataStyle;

            //To Data
            var reportToDataCell = sheetRow.CreateCell(4);
            reportToDataCell.SetCellValue(to);

            var reportToDataStyle = _workbook.CreateCellStyle();
            reportToDataStyle.BorderLeft = BorderStyle.Thin;
            reportToDataStyle.BorderTop = BorderStyle.Thin;
            reportToDataStyle.BorderRight = BorderStyle.Thin;
            reportToDataStyle.BorderBottom = BorderStyle.Thin;

            var reportToDataFont = _workbook.CreateFont();
            reportToDataFont.FontName = "Calibri";
            reportToDataFont.FontHeightInPoints = 11;
            reportToDataFont.IsItalic = true;

            reportToDataStyle.SetFont(reportToDataFont);
            reportToDataStyle.Alignment = HorizontalAlignment.Right;

            reportToDataCell.CellStyle = reportToDataStyle;

            //Total Lead Header
            sheetRow = _sheet.CreateRow(2);

            var reportTotalLeadHeaderCell = sheetRow.CreateCell(3);
            reportTotalLeadHeaderCell.SetCellValue("Total lead generation queries");

            var reportTotalLeadHeaderStyle = _workbook.CreateCellStyle();

            var reportTotalLeadHeaderFont = _workbook.CreateFont();
            reportTotalLeadHeaderFont.FontName = "Calibri";
            reportTotalLeadHeaderFont.Boldweight = (short)FontBoldWeight.Bold;
            reportTotalLeadHeaderFont.FontHeightInPoints = 11;
            reportTotalLeadHeaderFont.IsItalic = true;

            reportTotalLeadHeaderStyle.SetFont(reportTotalLeadHeaderFont);
            reportTotalLeadHeaderStyle.Alignment = HorizontalAlignment.Right;

            reportTotalLeadHeaderCell.CellStyle = reportTotalLeadHeaderStyle;

            //Total Lead Data
            var reportTotalLeadDataCell = sheetRow.CreateCell(4);
            reportTotalLeadDataCell.SetCellValue(totalLeadGenerationQueries);

            var reportTotalLeadDataStyle = _workbook.CreateCellStyle();


            reportTotalLeadDataStyle.BorderLeft = BorderStyle.Thin;
            reportTotalLeadDataStyle.BorderTop = BorderStyle.Thin;
            reportTotalLeadDataStyle.BorderRight = BorderStyle.Thin;
            reportTotalLeadDataStyle.BorderBottom = BorderStyle.Thin;


            var reportTotalLeadDataFont = _workbook.CreateFont();
            reportTotalLeadDataFont.FontName = "Calibri";
            reportTotalLeadDataFont.Boldweight = (short)FontBoldWeight.Bold;
            reportTotalLeadDataFont.FontHeightInPoints = 11;
            reportTotalLeadDataFont.IsItalic = true;

            reportTotalLeadDataStyle.SetFont(reportTotalLeadDataFont);
            reportTotalLeadDataStyle.Alignment = HorizontalAlignment.Right;

            reportTotalLeadDataCell.CellStyle = reportTotalLeadDataStyle;

            //Total Unattended Lead Header
            sheetRow = _sheet.CreateRow(3);

            var reportTotalUnattendedLeadHeaderCell = sheetRow.CreateCell(3);
            reportTotalUnattendedLeadHeaderCell.SetCellValue("Unattended queries");

            var reportTotalUnattendedLeadHeaderStyle = _workbook.CreateCellStyle();

            var reportTotalUnattendedLeadHeaderFont = _workbook.CreateFont();
            reportTotalUnattendedLeadHeaderFont.FontName = "Calibri";
            reportTotalUnattendedLeadHeaderFont.Boldweight = (short)FontBoldWeight.Bold;
            reportTotalUnattendedLeadHeaderFont.FontHeightInPoints = 11;
            reportTotalUnattendedLeadHeaderFont.IsItalic = true;

            reportTotalUnattendedLeadHeaderStyle.SetFont(reportTotalUnattendedLeadHeaderFont);
            reportTotalUnattendedLeadHeaderStyle.Alignment = HorizontalAlignment.Right;

            reportTotalUnattendedLeadHeaderCell.CellStyle = reportTotalUnattendedLeadHeaderStyle;

            //Total Unattended Lead Data

            var reportTotalUnattendedLeadDataCell = sheetRow.CreateCell(4);
            reportTotalUnattendedLeadDataCell.SetCellValue(totalUnattendedQueries);

            var reportTotalUnattendedLeadDataStyle = _workbook.CreateCellStyle();

            reportTotalUnattendedLeadDataStyle.BorderLeft = BorderStyle.Thin;
            reportTotalUnattendedLeadDataStyle.BorderTop = BorderStyle.Thin;
            reportTotalUnattendedLeadDataStyle.BorderRight = BorderStyle.Thin;
            reportTotalUnattendedLeadDataStyle.BorderBottom = BorderStyle.Thin;

            var reportTotalUnattendedLeadDataFont = _workbook.CreateFont();
            reportTotalUnattendedLeadDataFont.FontName = "Calibri";
            reportTotalUnattendedLeadDataFont.Boldweight = (short)FontBoldWeight.Bold;
            reportTotalUnattendedLeadDataFont.FontHeightInPoints = 11;
            reportTotalUnattendedLeadDataFont.IsItalic = true;

            reportTotalUnattendedLeadDataStyle.SetFont(reportTotalUnattendedLeadDataFont);
            reportTotalUnattendedLeadDataStyle.Alignment = HorizontalAlignment.Right;

            reportTotalUnattendedLeadDataCell.CellStyle = reportTotalUnattendedLeadDataStyle;

            var domainRowIndex = 5;
            var headerRowIndex = 7;

            foreach (var item in groupedLeadGenerationInfo)
            {
                var domianName = item.FirstOrDefault().DomainName;
                var domainLevelTotalQueries = item.Count();
                var domainLevelUnattendedQueries = item.Where(l => !l.LeadGenerationAction.HasValue).Count();

                var dataSheetRow = _sheet.CreateRow(domainRowIndex);

                //Domain Name
                var reportDomainNameDataCell = dataSheetRow.CreateCell(0);
                reportDomainNameDataCell.SetCellValue(domianName);

                var reportDomainNameDataStyle = _workbook.CreateCellStyle();

                var reportDomainNameDataFont = _workbook.CreateFont();
                reportDomainNameDataFont.FontName = "Calibri";
                reportDomainNameDataFont.Boldweight = (short)FontBoldWeight.Bold;
                reportDomainNameDataFont.FontHeightInPoints = 11;

                reportDomainNameDataStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                reportDomainNameDataStyle.FillPattern = FillPattern.SolidForeground;

                reportDomainNameDataStyle.SetFont(reportDomainNameDataFont);
                reportDomainNameDataStyle.Alignment = HorizontalAlignment.Left;

                reportDomainNameDataCell.CellStyle = reportDomainNameDataStyle;

                //Total Queries
                var reportTotalQueriesHeaderCell = dataSheetRow.CreateCell(1);
                reportTotalQueriesHeaderCell.SetCellValue("Total queries");

                var reportTotalQueriesHeaderStyle = _workbook.CreateCellStyle();

                var reportTotalQueriesHeaderFont = _workbook.CreateFont();
                reportTotalQueriesHeaderFont.FontName = "Calibri";
                reportTotalQueriesHeaderFont.Boldweight = (short)FontBoldWeight.Bold;
                reportTotalQueriesHeaderFont.FontHeightInPoints = 11;
                reportTotalQueriesHeaderFont.IsItalic = true;

                reportTotalQueriesHeaderStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                reportTotalQueriesHeaderStyle.FillPattern = FillPattern.SolidForeground;

                reportTotalQueriesHeaderStyle.SetFont(reportTotalQueriesHeaderFont);
                reportTotalQueriesHeaderStyle.Alignment = HorizontalAlignment.Left;

                reportTotalQueriesHeaderCell.CellStyle = reportTotalQueriesHeaderStyle;

                //Total Queries Data
                var reportTotalQueriesDataCell = dataSheetRow.CreateCell(2);
                reportTotalQueriesDataCell.SetCellValue(domainLevelTotalQueries);

                var reportTotalQueriesDataStyle = _workbook.CreateCellStyle();

                var reportTotalQueriesDataFont = _workbook.CreateFont();
                reportTotalQueriesDataFont.FontName = "Calibri";
                reportTotalQueriesDataFont.Boldweight = (short)FontBoldWeight.Bold;
                reportTotalQueriesDataFont.FontHeightInPoints = 11;

                reportTotalQueriesDataStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                reportTotalQueriesDataStyle.FillPattern = FillPattern.SolidForeground;

                reportTotalQueriesDataStyle.SetFont(reportTotalQueriesDataFont);
                reportTotalQueriesDataStyle.Alignment = HorizontalAlignment.Right;

                reportTotalQueriesDataCell.CellStyle = reportTotalQueriesDataStyle;

                //UnAttended Queries
                var reportTotalUnattendedQueriesHeaderCell = dataSheetRow.CreateCell(3);
                reportTotalUnattendedQueriesHeaderCell.SetCellValue("Unattended queries");

                var reportTotalUnattendedQueriesHeaderStyle = _workbook.CreateCellStyle();

                var reportTotalUnattendedQueriesHeaderFont = _workbook.CreateFont();
                reportTotalUnattendedQueriesHeaderFont.FontName = "Calibri";
                reportTotalUnattendedQueriesHeaderFont.Boldweight = (short)FontBoldWeight.Bold;
                reportTotalUnattendedQueriesHeaderFont.FontHeightInPoints = 11;
                reportTotalUnattendedQueriesHeaderFont.IsItalic = true;

                reportTotalUnattendedQueriesHeaderStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                reportTotalUnattendedQueriesHeaderStyle.FillPattern = FillPattern.SolidForeground;

                reportTotalUnattendedQueriesHeaderStyle.SetFont(reportTotalUnattendedQueriesHeaderFont);
                reportTotalUnattendedQueriesHeaderStyle.Alignment = HorizontalAlignment.Left;

                reportTotalUnattendedQueriesHeaderCell.CellStyle = reportTotalUnattendedQueriesHeaderStyle;

                //UnAttended Queries Data
                var reportTotalUnattendedQueriesDataCell = dataSheetRow.CreateCell(4);
                reportTotalUnattendedQueriesDataCell.SetCellValue(domainLevelUnattendedQueries);

                var reportTotalUnattendedQueriesDataStyle = _workbook.CreateCellStyle();
  
                var reportTotalUnattendedQueriesDataFont = _workbook.CreateFont();
                reportTotalUnattendedQueriesDataFont.FontName = "Calibri";
                reportTotalUnattendedQueriesDataFont.Boldweight = (short)FontBoldWeight.Bold;
                reportTotalUnattendedQueriesDataFont.FontHeightInPoints = 11;

                reportTotalUnattendedQueriesDataStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                reportTotalUnattendedQueriesDataStyle.FillPattern = FillPattern.SolidForeground;

                reportTotalUnattendedQueriesDataStyle.SetFont(reportTotalUnattendedQueriesDataFont);
                reportTotalUnattendedQueriesDataStyle.Alignment = HorizontalAlignment.Right;

                reportTotalUnattendedQueriesDataCell.CellStyle = reportTotalUnattendedQueriesDataStyle;

                //Headers Part
                var headerStyle = _workbook.CreateCellStyle();
                var headerFont = _workbook.CreateFont();
                headerFont.FontHeightInPoints = 11;
                headerFont.FontName = "Calibri";
                headerFont.Boldweight = (short)FontBoldWeight.Bold;
                headerFont.IsBold = true;
                headerFont.IsItalic = true;
                headerStyle.SetFont(headerFont);

                var headerSheetRow = _sheet.CreateRow(headerRowIndex);
                for (var i = 0; i < _headers.Count; i++)
                {
                    var cell = headerSheetRow.CreateCell(i);
                    cell.SetCellValue(_headers[i]);
                    if (i == 0)
                    {
                        var firstHeaderStyle = _workbook.CreateCellStyle();
                        firstHeaderStyle.Alignment = HorizontalAlignment.Right;
                        var firstHeaderFont = _workbook.CreateFont();
                        firstHeaderFont.FontHeightInPoints = 11;
                        firstHeaderFont.FontName = "Calibri";
                        firstHeaderFont.Boldweight = (short)FontBoldWeight.Bold;
                        firstHeaderFont.IsBold = true;
                        firstHeaderFont.IsItalic = true;
                        firstHeaderStyle.SetFont(firstHeaderFont);

                        cell.CellStyle = firstHeaderStyle;
                    }
                    else
                    {
                        cell.CellStyle = headerStyle;
                    }
                    // It's heavy, it slows down your Excel if you have large data                
                    _sheet.AutoSizeColumn(i);
                }

                var itemIndex = 0;
                for (int rowIndex = headerRowIndex + 1; rowIndex <= headerRowIndex + item.Count; rowIndex++)
                {
                    var itemSheetRow = _sheet.CreateRow(rowIndex);

                    for (int columnIndex = 0; columnIndex < reportColumns.Length; columnIndex++)
                    {
                        ICell cell = itemSheetRow.CreateCell(columnIndex);
                        string cellName = reportColumns[columnIndex].Item2;
                        if (string.IsNullOrEmpty(cellName))
                        {
                            string cellvalue = (itemIndex + 1).ToString();
                            cell.SetCellValue(cellvalue);

                            var firstHeaderDataStyle = _workbook.CreateCellStyle();
                            firstHeaderDataStyle.Alignment = HorizontalAlignment.Right;
                            var firstHeaderDataFont = _workbook.CreateFont();
                            firstHeaderDataFont.FontHeightInPoints = 11;
                            firstHeaderDataFont.FontName = "Calibri";
                            firstHeaderDataStyle.SetFont(firstHeaderDataFont);

                            cell.CellStyle = firstHeaderDataStyle;
                        }
                        else
                        {
                            var itemObject = item[itemIndex].GetType().GetProperty(cellName);
                            if (itemObject != null)
                            {
                                var cellvalue = Convert.ToString(itemObject.GetValue(item[itemIndex], null));

                                if (string.IsNullOrWhiteSpace(cellvalue))
                                {
                                    cell.SetCellValue(string.Empty);
                                }
                                else if (_type[columnIndex].ToLower() == "string")
                                {
                                    cell.SetCellValue(cellvalue);
                                }
                                else if (_type[columnIndex].ToLower() == "leadgenerationaction")
                                {
                                    cell.SetCellValue(Enum.GetName(typeof(LeadGenerationAction), Convert.ToInt32(cellvalue)));
                                }
                                else if (_type[columnIndex].ToLower() == "int32")
                                {
                                    cell.SetCellValue(Convert.ToInt32(cellvalue));
                                }
                                else if (_type[columnIndex].ToLower() == "double")
                                {
                                    cell.SetCellValue(Convert.ToDouble(cellvalue));
                                }
                                else if (_type[columnIndex].ToLower() == "datetime")
                                {
                                    cell.SetCellValue(Convert.ToDateTime(cellvalue).ToString("dd-MMM-yy HH:mm"));
                                }
                                else
                                {
                                    cell.SetCellValue(string.Empty);
                                }
                            }
                        }
                    }
                    itemIndex += 1;
                }

                domainRowIndex = domainRowIndex + 4 + item.Count();
                headerRowIndex = headerRowIndex + 4 + item.Count();
            }
        }
    }
}
