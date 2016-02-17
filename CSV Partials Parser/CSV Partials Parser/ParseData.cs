using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericParsing;

namespace CSV_Partials_Parser
{
    class ParseData
    {
        private DataTable formatTable;

        private static DataTable GetTable()
        {
            //formatTable = new DataTable();
            //formatTable.Columns.Add();
            throw new NotImplementedException();
        }






        //private List<string> ParseCSVHeaders(string userFile, Encoding enc, Char delimiter, int startRowNumber)
        //{
        //    List<string> headerArray = new List<string>();
        //    GenericParser parser = new GenericParser(userFile, enc) { ColumnDelimiter = delimiter, FirstRowHasHeader = true, MaxBufferSize = 10000, CommentCharacter = null };
        //    bool locker = false;
        //    while (parser.Read())
        //    {
        //        if (locker) continue;
        //        for (int i = 0; i < parser.ColumnCount; i++)
        //        {
        //            if (!String.IsNullOrWhiteSpace(parser.GetColumnName(i)))
        //                headerArray.Add(parser.GetColumnName(i));
        //        }
        //        locker = true;
        //    }
        //    _fileLength = parser.DataRowNumber - startRowNumber;
        //    parser.Dispose();
        //    return headerArray;
        //}
    }
}
