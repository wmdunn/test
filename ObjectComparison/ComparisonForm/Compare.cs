using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;
using GenericParsing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ComparisonForm
{
    class Compare
    {
        public Dictionary<string, string> _defaults;
        public Dictionary<string, string> _ignores;


        public void DoStuff(string baseFilePath, string compareFilePath, string configFilePath, bool? sampleSet)
        {
            string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            ComparisonWindow.WorkerParser.ReportProgress(0, "Reading Base Table");
            GenericParserAdapter parserAdapter = new GenericParserAdapter(baseFilePath, Encoding.UTF8)
            {
                ColumnDelimiter = ',',
                FirstRowHasHeader = true,
                MaxBufferSize = 1000000,
                CommentCharacter = null
            };
            DataTable table1 = parserAdapter.GetDataTable();
            ComparisonWindow.WorkerParser.ReportProgress(0, "Analyzing Base Table");
            DataTableExt.DataType[] table1Types = DataTableExt.Analyze(table1, null);
            GenericParserAdapter parserAdapter2 = new GenericParserAdapter(compareFilePath, Encoding.UTF8)
            {
                ColumnDelimiter = ',',
                FirstRowHasHeader = true,
                MaxBufferSize = 1000000,
                CommentCharacter = null
            };
            ComparisonWindow.WorkerParser.ReportProgress(0, "Reading Comparison Table");
            DataTable table2 = parserAdapter2.GetDataTable();
            ComparisonWindow.WorkerParser.ReportProgress(0, "Analyzing Comparison Table");
            DataTableExt.DataType[] table2Types = DataTableExt.Analyze(table2, null);

            ComparisonWindow.WorkerParser.ReportProgress(0, "Merging Data Types");
            DataTableExt.DataType[] masterTypes = new DataTableExt.DataType[table1Types.Length];
            for (int i = 0; i < table1Types.Length; i++)
            {
                if (string.Equals(table1Types[i].Type, table2Types[i].Type))
                    masterTypes[i] = table1Types[i];
                else if (table1Types[i].Type == "NULL")
                    masterTypes[i] = table2Types[i];
                else if (table2Types[i].Type == "NULL")
                    masterTypes[i] = table1Types[i];
                else
                    masterTypes[i] = new DataTableExt.DataType {Type = "VARCHAR2"};
            }

            ComparisonWindow.WorkerParser.ReportProgress(0, "Reading Config");
            XmlDocument configFile = Config.LoadConfig(configFilePath);
            XmlNode xn = configFile.SelectSingleNode("configuration");
            if (xn == null)
                throw new Exception("Settings could not be found. Please check your configuration file.");

            string siterraObject = xn["object"].InnerText;
            List<string> keys = new List<string>();
            foreach (XmlNode xn1 in xn.SelectSingleNode("primaryKeys").ChildNodes)
            {
                keys.Add(xn1.InnerText);
            }
            Dictionary<string, string> defaults = new Dictionary<string, string>();
            Dictionary<string, string> ignores = new Dictionary<string, string>();
            foreach (XmlNode xn1 in xn.SelectSingleNode("defaults").ChildNodes)
            {
                defaults.Add(xn1.Attributes["name"]?.InnerText.ToUpper(), xn1.InnerText);
            }
            foreach (XmlNode xn1 in xn.SelectSingleNode("ignore").ChildNodes)
            {
                ignores.Add(xn1.Attributes["name"]?.InnerText.ToUpper(), xn1.InnerText);
            }
            _defaults = defaults;
            _ignores = ignores;
            
            //Rename 'Base' table columns to be the same as the comparison table for ease of use.
            for (int i = 0; i < table1.Columns.Count; i++)
            {
                table1.Columns[i].ColumnName = table2.Columns[i].ColumnName;
            }
            
            DataColumn[] table2Keys = new DataColumn[keys.Count];
            for (int i = 0; i < keys.Count; i++)
            {
                if (table2.Columns[keys[i]] == null)
                    throw new Exception($@"Could not find default column {keys[i]} in base file.");
                table2Keys[i] = table2.Columns[keys[i]];
            }
            table2.PrimaryKey = table2Keys;


            ComparisonWindow.WorkerParser.ReportProgress(0, "Comparifying Files");

            string filePath = basePath + $@"\{siterraObject}ComparisonFile.xlsx";
            if (File.Exists(filePath))
                filePath = filePath.Replace(".xlsx", "_" + DateTime.Now.ToString("MMddyyHHmmss") + ".xlsx");

            FileInfo excelFile = new FileInfo(filePath);
            ExcelPackage ep = new ExcelPackage(excelFile);
            ExcelWorksheet excelSheet = ep.Workbook.Worksheets.Add("Comparison");
            for (int i = 1; i <= table1.Columns.Count; i++)
            {
                excelSheet.Cells[2, i].Value = table1.Columns[i-1].ColumnName;
                excelSheet.Cells[2, i].Style.Font.Bold = true;
                excelSheet.Column(i).Width = 24;
            }

            //Formatting
            using (var range = excelSheet.Cells[1, 1, 1, table1.Columns.Count])
            {
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            }
            using (var range = excelSheet.Cells[2, 1, 2, table1.Columns.Count])
            {
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            }

            int row = 3;
            int[] numErrors = new int[table1.Columns.Count];

            foreach (DataRow dr in table1.Rows)
            {
                List<object> keyList = new List<object>();
                foreach (string s in keys)
                {
                    keyList.Add(dr[s]);
                }
                object[] keyvals = keyList.ToArray();
                DataRow dr2 = table2.Rows.Find(keyvals);
                for (int i = 0; i < dr.ItemArray.Count(); i++)
                {
                    excelSheet.Cells[row, i + 1].Value = dr[i];
                    if (!CompareValues(dr[i].ToString(), dr2[i].ToString(), masterTypes[i].Type, table1.Columns[i].ColumnName))
                    {
                        numErrors[i] ++;
                        excelSheet.Cells[row, i + 1].AddComment(dr2[i].ToString(), "SkyNet");
                        excelSheet.Cells[row, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        excelSheet.Cells[row, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 238, 189, 184));
                    }
                }
                row++;
                if (row%1000 == 0)
                {
                    decimal d = 100*row/table1.Rows.Count;
                    ComparisonWindow.WorkerParser.ReportProgress((int)Math.Round(d), "Comparifying Files");
                    excelSheet = ep.Workbook.Worksheets["Comparison"];
                    if (sampleSet != null && (bool)sampleSet)
                        break;
                }
            }

            ComparisonWindow.WorkerParser.ReportProgress(100, "Counting Errors");
            for (int i = 1; i <= numErrors.Length; i++)
            {
                if (numErrors[i - 1] == 1)
                    excelSheet.Cells[1, i].Value = numErrors[i - 1] + " error";
                else
                    excelSheet.Cells[1, i].Value = numErrors[i - 1] + " errors";
                excelSheet.Cells[1, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                if (numErrors[i - 1] == 0)
                    excelSheet.Cells[1, i].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 184, 255, 184));
                else
                    excelSheet.Cells[1, i].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 238, 189, 184));
            }

            ComparisonWindow.WorkerParser.ReportProgress(100, "Saving Excel, clear up some memory and take a nap...");

            ep.Save();

            //everybody do your share
            ep.Dispose();
            parserAdapter.Dispose();
            parserAdapter2.Dispose();
            table1.Dispose();
            table2.Dispose();

            ComparisonWindow.WorkerParser.ReportProgress(100, "DONE");
            System.Diagnostics.Process.Start(filePath);
        }
       


        public bool CompareValues(string s1, string s2, string dataType, string columnName)
        {
            if (_defaults.ContainsKey(columnName))
            {
                if (string.IsNullOrWhiteSpace(s1))
                    s1 = _defaults[columnName];
            }

            if (_ignores.ContainsKey(columnName))
            {
                s1 = "";
                s2 = "";
            }

            switch (dataType)
            {
                case "NULL":
                    return true;
                case "VARCHAR2":
                    return string.Equals(s1.Replace("\r\n", "\n").Replace("\r", "\n"), s2.Replace("\r\n", "\n").Replace("\r", "\n"), StringComparison.Ordinal);
                case "NUMBER":
                    if (string.IsNullOrWhiteSpace(s1) && !string.IsNullOrWhiteSpace(s2))
                        return false;
                    if (!string.IsNullOrWhiteSpace(s1) && string.IsNullOrWhiteSpace(s2))
                        return false;
                    if (string.IsNullOrWhiteSpace(s1) && string.IsNullOrWhiteSpace(s2))
                        return true;
                    return Convert.ToDecimal(s1) == Convert.ToDecimal(s2);
                case "DATE":
                    if (string.IsNullOrWhiteSpace(s1) && !string.IsNullOrWhiteSpace(s2))
                        return false;
                    if (!string.IsNullOrWhiteSpace(s1) && string.IsNullOrWhiteSpace(s2))
                        return false;
                    if (string.IsNullOrWhiteSpace(s1) && string.IsNullOrWhiteSpace(s2))
                        return true;
                    return DateTime.Parse(s1) == DateTime.Parse(s2);
            }
            return true;
        }
    }
}
