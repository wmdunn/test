using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using GenericParsing;
using  LumenWorks.Framework.IO.Csv;

namespace CSV_Partials_Parser
{
    class Handler
    {
 
        private List<PayLine> Paylines = new List<PayLine>();
        //private List<int> PartialsIndexCount; 
        private DataTable csvDataTable;
        private string path1 = @"C:\Users\wdunn\Desktop\testRepository\testRepository\CSV Partials Parser\TableToFileTest.csv";
        private string path2 = @"C:\Users\wdunn\Desktop\testRepository\testRepository\CSV Partials Parser\ListToFileTestTest.csv";
        private string[] columnNames = new string[]
        {
            "\"Status [Canceled, In Design, On Hold, Paid, Paid(Override), Partially Paid, To be Paid, Void]\"",
            "*Payment Header Number (Text - 30)",
            "Status Date (Date)", 
            "Planned Date (Date)", 
            "Planned Amount (Decimal)",
            "Planned Currency", 
            "Actual Amount (Decimal)",
            "Actual Date (Date)", 
            "Offset Number (Text - 30)", 
            "Offset Amount (Decimal)",
            "Offset Date (Date)",
            "Allocation Share",
            "Allocation Share - Start with and separate by ^ (e.g. '^35^15^50') (Text)",
            "Vendor Number - Start with and separate by ^ (e.g. '^VND1^VND0492^VND9348') (Text)",
            "Vendor Location Number - Start with and separate by ^ (e.g. '^VND1^VND0492^VND9348') (Text)",
            "Allocation Description - Start with and separate by ^ (e.g. '^Ck A/C 6557607504^Monticello RSA-4^Summerville') (Text)",
            "\"Allocation Type - Start with and separate by ^ (e.g. '^Payee Account^Payee Account^Payee Account') [Budget Account, Business Area, Cost Center, GL Account, Journal Account, Payee Account, Profit Center]\"",
            "\"By Percent or Amount? [PCT, AMT]\"",
            "Reference No (Text - 30)",
            "Remarks (Text - 4000)",
            "Check Number (Text - 30)",
            "Check Amount (Decimal)",
            "Check Date (Date)",
            "Partials Actual Amount",
            "Partials Actual Date",
            "Partials Check No",
            "Partials Check Amount",
            "Partials Check Date"

        };

        


         public void Initialize(string filename)
        {
             LoadTableFromFile(filename);
             csvDataTable = SortDataTable(csvDataTable);
             WriteTabletoFile(csvDataTable);
             PopulatePaylineList(csvDataTable);
             Paylines = ParsePayLineList(Paylines);
             WriteListToFile(Paylines);
        }

        public void LoadTableFromFile(string filename)
        {
            GenericParserAdapter adapter = new GenericParserAdapter(filename);
            csvDataTable = adapter.GetDataTable();
        }

        public void WriteListToFile(List<PayLine> PayLines)
        {
            StringBuilder sb = new StringBuilder();

            //create Header row with Proper Field Names
            sb.AppendLine(String.Join(",", columnNames)); 
            
            sb = buildStringFromPaylines(PayLines, sb);
            
            File.WriteAllText(path2, sb.ToString());
        }

        public void WriteTabletoFile(DataTable dt)
        {
            StringBuilder sb = new StringBuilder(); 

            //create column row with Proper Field Names
            sb.AppendLine(String.Join(",", columnNames));
            
            //append all rows to stringbuilder
           foreach (DataRow row in dt.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                sb.AppendLine(String.Join(",", fields));
            }

            File.WriteAllText(path1, sb.ToString());
            
        }

        public DataTable SortDataTable(DataTable dt)
        {
            DataView dv = new DataView();
            dv = dt.DefaultView;
            dv.Sort="column2 Asc, column4 Asc";
            dt = dv.ToTable();
            return dt;
        }

        public void PopulatePaylineList(DataTable dt)
        {
            StringBuilder sb = new StringBuilder(); 
            foreach (DataRow row in dt.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();

                PayLine p = new PayLine(fields);
                    Paylines.Add(p);
            }
            
        }


        public static StringBuilder buildStringFromPaylines(List<PayLine> Paylines, StringBuilder sb)
        {
            foreach (PayLine payline in Paylines)
            {
                sb.Append(payline.Status + ",");
                sb.Append(payline.PayHeaderNumb + ",");
                sb.Append(payline.StatusDate + ",");
                sb.Append(payline.PlannedDate + ",");
                sb.Append(payline.PlannedAmount + ",");
                sb.Append(payline.PlannedCurrency + ",");
                sb.Append(payline.ActualAmount + ",");
                sb.Append(payline.ActualCurrency + ",");
                sb.Append(payline.ActualDate + ",");
                sb.Append(payline.OffsetNumber + ",");
                sb.Append(payline.OffsetAmount + ",");
                sb.Append(payline.OffsetDate + ",");
                sb.Append(payline.AllocationShare + ",");
                sb.Append(payline.VendorNumber + ",");
                sb.Append(payline.VendorLocation + ",");
                sb.Append(payline.AllocationDescription + ",");
                sb.Append(payline.AllocationType + ",");
                sb.Append(payline.PctOrAmnt + ",");
                sb.Append(payline.ReferenceNum + ",");
                sb.Append(payline.Remarks + ",");
                sb.Append(payline.CheckNumber + ",");
                sb.Append(payline.CheckAmount + ",");
                sb.Append(payline.CheckDate + ",");
                sb.Append(payline.PartialsActualAmnt + ",");
                sb.Append(payline.PartialsActualDate + ",");
                sb.Append(payline.PartialsCheckNo + ",");
                sb.Append(payline.PartialsCheckAmount + ",");
                sb.Append(payline.PartialsCheckDate + ",");
                sb.AppendLine();
            }

            return sb;
        }

        public static List<PayLine> ParsePayLineList(List<PayLine> PayLineList)
        {
            int index = 0;
            List<int> PartialsIndexCount = new List<int>();
            for (int i = index; i < PayLineList.Count; i++)
            {
                PartialsIndexCount = FindPartials(PayLineList, index);
                PayLineList = UpdatePayLineList(PayLineList, PartialsIndexCount, index);
                index++;
            }

            return PayLineList;
        }


        public static List<int> FindPartials(List<PayLine> PayLineList, int index)
        {

            List<int> PartialsIndexCount = new List<int>();

            for (int j = index + 1; j < PayLineList.Count; j++)
            {
                if (PayLineList[index].PayHeaderNumb.Equals(PayLineList[j].PayHeaderNumb) && (PayLineList[index].PlannedDate.Equals(PayLineList[j].PlannedDate)))
                {
                    PartialsIndexCount.Add(j);
                }
            }
            return PartialsIndexCount;
        }

        public static List<PayLine> UpdatePayLineList(List<PayLine> PayLineList, List<int> indexCount, int index)
        {
            decimal PlannedAmount = Convert.ToInt32(PayLineList[index].PlannedAmount);
            decimal ActualAmount = Convert.ToInt32(PayLineList[index].ActualAmount);
            for (int i = 0; i < indexCount.Count; i++)
            {
                PlannedAmount += Convert.ToDecimal(PayLineList[indexCount[i]].PlannedAmount);
                ActualAmount += Convert.ToDecimal(PayLineList[indexCount[i]].ActualAmount);
                PayLineList.RemoveAt(indexCount[i]);
            }

            PayLineList[index].PlannedAmount = Convert.ToString(PlannedAmount);
            PayLineList[index].ActualAmount = Convert.ToString(ActualAmount);

            return PayLineList;
        }
    }
      
}
