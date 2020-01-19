using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Cashflowio.Core.Entities;
using OfficeOpenXml;

namespace Cashflowio.Infrastructure.Data
{
    public static class RawTransactionFactory
    {
        private const int FirstDataRow = 2;
        private const string WorkSheetName = "CoinKeeper";

        public static List<RawTransaction> ReadFromFile(string filePath)
        {
            var file = new FileInfo(filePath);

            using var package = new ExcelPackage(file);
            var workSheet = package.Workbook.Worksheets[WorkSheetName];
            var totalRows = workSheet.Dimension.Rows;

            var transactions = new List<RawTransaction>();

            for (var i = FirstDataRow; i <= totalRows; i++)
                transactions.Add(new RawTransaction
                {
                    Date = workSheet.GetDate(i, CoinkeeperColumn.Date),
                    Type = workSheet.GetString(i, CoinkeeperColumn.Type),
                    Source = workSheet.GetString(i, CoinkeeperColumn.From),
                    Destination = workSheet.GetString(i, CoinkeeperColumn.To),
                    Tag = workSheet.GetString(i, CoinkeeperColumn.Tags),
                    Amount = workSheet.GetDouble(i, CoinkeeperColumn.Amount),
                    Currency = workSheet.GetString(i, CoinkeeperColumn.Currency),
                    Note = workSheet.GetString(i, CoinkeeperColumn.Note)
                }.Normalized());

            return transactions;
        }
    }

    public enum CoinkeeperColumn
    {
        Date = 1,
        Type,
        From,
        To,
        Tags,
        Amount,
        Currency,
        Note = 11
    }

    public static class ExcelWorksheetExtensions
    {
        private const DateTimeStyles DateTimeStyle = DateTimeStyles.None;
        private static readonly CultureInfo Provider = new CultureInfo("en-US");

        public static string GetString(this ExcelWorksheet worksheet, int row, CoinkeeperColumn column)
        {
            return worksheet.Cells[row, (int) column].Value?.ToString() ?? string.Empty;
        }

        public static double GetDouble(this ExcelWorksheet worksheet, int row, CoinkeeperColumn column)
        {
            double.TryParse(worksheet.GetString(row, column), out var result);
            return result;
        }

        public static DateTime GetDate(this ExcelWorksheet worksheet, int row, CoinkeeperColumn column)
        {
            var str = worksheet.GetString(row, column).Trim().Split(" ")[0];

            if (str.StartsWith("0"))
                str = str.Substring(1);

            DateTime.TryParse(str, Provider, DateTimeStyle, out var result);

            return result;
        }
    }
}