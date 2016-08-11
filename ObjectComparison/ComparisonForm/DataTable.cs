using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace ComparisonForm
{
    public class DataTableExt
    {
        public string TableName { get; set; }
        public string StageTableName { get; set; }
        public string[] ColumnNames { get; set; }
        public DataType[] DataTypes { get; set; }
        public int NumRows { get; set; }
        public int NumColumns { get; set; }

        public class DataType
        {
            public DataType()
            {
                Size = 0;
                Type = "";
                Count = 0;
            }
            public string Type { get; set; }
            public int Size { get; set; }
            public int Count { get; set; }
        }

        //create array of objects
        static T[] InitializeArray<T>(int length) where T : new()
        {
            T[] array = new T[length];
            for (int i = 0; i < length; ++i)
            {
                array[i] = new T();
            }

            return array;
        }

        //analyze table to determine data types
        public static DataType[] Analyze(Dictionary<int, List<string>> table, DataType[] types)
        {
            //if types param is null, create type array
            DataType[] dataTypeRow = types ?? InitializeArray<DataType>(table.Count);

            //DETERMINE DBTYPE
            for (int i = 0; i < table.Count; i++)
            {
                //IGNORE COLUMNS ALREADY DEFAULTED TO VARCHAR2
                foreach (string s in table[i].TakeWhile(s => dataTypeRow[i].Type != "VARCHAR2"))
                {
                    DateTime testDate;
                    Double testNum;
                    if (dataTypeRow[i].Type != "NUMBER" &&
                             DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out testDate))
                        dataTypeRow[i].Type = "DATE";
                    //NUMBER IS ONLY CALCULATED IF SIZE IS BEING ANALYZED
                    else if (s.Length <= 38 &&
                             Double.TryParse(s, out testNum))
                        dataTypeRow[i].Type = "NUMBER";
                    else if (String.IsNullOrEmpty(s))
                    {
                        if (dataTypeRow[i].Type != "NUMBER" &&
                            dataTypeRow[i].Type != "DATE")
                            dataTypeRow[i].Type = "NULL";
                    }
                    else
                    {
                        dataTypeRow[i].Type = "VARCHAR2";
                        break;
                    }
                }
            }
            return dataTypeRow;
        }

        public static DataType[] Analyze(DataTable dataTable, DataType[] types)
        {
            Dictionary<int, List<string>> table = new Dictionary<int, List<string>>();
            for (int i = 0; i < dataTable.Columns.Count; i ++)
            {
                table.Add(i, dataTable.AsEnumerable().Select(r => r[i].ToString()).ToList());
            }
            return Analyze(table, types);
        }

        //used to abbriviate tableName and tableIndex when too long
        public static string Abbreviate(string input)
        {
            string[] splitName = input.Split('_');
            string result = "";
            foreach (string s in splitName)
            {
                if (s.Length < 3)
                    result += s + "_";
                else
                    result += s.Substring(0, 3) + "_";
            }

            return result.Substring(0, result.Length - 1);
        }
    }
}
