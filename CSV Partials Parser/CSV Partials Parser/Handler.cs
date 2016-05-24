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
        private string _filename;
        private string path1 = @"C:\Users\wdunn\Desktop\testRepository\testRepository\CSV Partials Parser\TableToFileTest.csv";
        private string path2 = @"C:\Users\wdunn\Desktop\testRepository\testRepository\CSV Partials Parser\";
        private string[] columnNames = new string[]
        {
            "\"Status [Canceled, In Design, On Hold, Paid, Paid(Override), Partially Paid, To be Paid, Void]\"",
            "*Payment Header Number (Text - 30)",
            "Status Date (Date)", 
            "Planned Date (Date)", 
            "Planned Amount (Decimal)",
            "Planned Currency", 
            "Actual Amount (Decimal)",
            "Actual Currency",
            "Actual Date (Date)",      
            "Offset Number (Text - 30)", 
            "Offset Amount (Decimal)",
            "Offset Date (Date)",
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
            "Partials Check Date",
            "Partials Check Reference No",
            "Partials Remarks"

        };

        


         public void Initialize(string filename)
         {
             _filename = Path.GetFileName(filename);
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
            
            File.WriteAllText(path2+_filename, sb.ToString());
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
            //StringBuilder sb = new StringBuilder(); 
            string[] fields = new string[23];
            for (int i = 0; i < fields.Length; i++)
            {
             
                    fields[i] = "";
                
            }
            foreach (DataRow row in dt.Rows)
            {
                fields = row.ItemArray.Select(field => field.ToString()).ToArray();

                PayLine p = new PayLine(fields);
                    Paylines.Add(p);
            }
            
        }


        public static StringBuilder buildStringFromPaylines(List<PayLine> Paylines, StringBuilder sb)
        {
            foreach (PayLine payline in Paylines)
            {
                payline.checkForDelimiter(payline);
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
                sb.Append(payline.PartialsCheckReference + ",");
                sb.Append(payline.PartialsCheckRemarks + ",");
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
                if(PartialsIndexCount.Count > 0)
                    PayLineList = UpdatePayLineList(PayLineList, PartialsIndexCount, index);
                index++;
            }

            return PayLineList;
        }

        //For every payments that shares the same header and planned date, the index of that payline will be stored within the list ParitalsIndexCount
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

        //Given the indexes of all paylines with the same payheader and planned date, this method will combine them into one payline. It will then remove the partials 
        public static List<PayLine> UpdatePayLineList(List<PayLine> PayLineList, List<int> indexCount, int index)
        {
            decimal PlannedAmount = 0;
            decimal offsetAmount = 0;
            string offsetNumber = null;
            //decimal PlannedAmount = Convert.ToDecimal(PayLineList[index].PlannedAmount);
            decimal ActualAmount = Convert.ToDecimal(PayLineList[index].ActualAmount);
            string PartialsActualAmount = "^" + PayLineList[index].ActualAmount;
            string PartialsActualDate = "^" + PayLineList[index].ActualDate;
            string PartialsCheckNo = "^" + PayLineList[index].CheckNumber;
            string PartialsCheckAmount = "^" + PayLineList[index].CheckAmount;
            string PartialsCheckDate = "^" + PayLineList[index].CheckDate;
            string PartialsReferenceNum = "^" + PayLineList[index].ReferenceNum;
            string PartialsRemark = "^" + PayLineList[index].Remarks;
            for (int i = 0; i < indexCount.Count; i++)
            {
                if (PayLineList[indexCount[i]].OffsetAmount != "")
                    offsetAmount = Convert.ToDecimal(PayLineList[indexCount[i]].OffsetAmount);
                if (PayLineList[indexCount[i]].OffsetNumber != "")
                    offsetNumber = PayLineList[indexCount[i]].OffsetNumber;
                PlannedAmount = Convert.ToDecimal(PayLineList[indexCount[i]].PlannedAmount);
                ActualAmount += Convert.ToDecimal(PayLineList[indexCount[i]].ActualAmount);
                PartialsActualAmount += "^" + PayLineList[indexCount[i]].ActualAmount;
                //if (DateTime.Parse(PartialsActualDate) > DateTime.Parse(PayLineList[indexCount[i]].CheckDate))
                //    PartialsActualDate = PayLineList[indexCount[i]].CheckDate;
                PartialsActualDate += "^"+ PayLineList[indexCount[i]].ActualDate;
                PartialsCheckNo += "^" + PayLineList[indexCount[i]].CheckNumber;
                PartialsCheckAmount += "^" + PayLineList[indexCount[i]].CheckAmount;
                PartialsCheckDate += "^" + PayLineList[indexCount[i]].CheckDate;
                PartialsReferenceNum += "^" + PayLineList[indexCount[i]].ReferenceNum;
                PartialsRemark += "^" + PayLineList[indexCount[i]].Remarks;
                //PayLineList.RemoveAt(indexCount[i]);
            }
            for (int i = 0; i < indexCount.Count; i++)
                PayLineList.RemoveAt(indexCount[i-i]);
            if (offsetAmount != 0)
                PayLineList[index].OffsetAmount = Convert.ToString(offsetAmount);
            if (offsetNumber != null)
                PayLineList[index].OffsetNumber = Convert.ToString(offsetNumber);
            PayLineList[index].PlannedAmount = Convert.ToString(PlannedAmount);
            PayLineList[index].ActualAmount = Convert.ToString(ActualAmount);
            PayLineList[index].PartialsActualAmnt = PartialsActualAmount;
            PayLineList[index].PartialsActualDate = PartialsActualDate;
            PayLineList[index].PartialsCheckNo = PartialsCheckNo;
            PayLineList[index].PartialsCheckAmount = PartialsCheckAmount;
            PayLineList[index].PartialsCheckDate = PartialsCheckDate;
            PayLineList[index].PartialsCheckReference = PartialsReferenceNum;
            PayLineList[index].PartialsCheckRemarks = PartialsRemark;
            PayLineList[index].Status = "Paid(Override)";
            //if (Convert.ToDecimal(PayLineList[index].PlannedAmount) <= Convert.ToDecimal(PayLineList[index].ActualAmount))
            //{
            //    PayLineList[index].Status = "Paid";
            //}
            //else if (Convert.ToDecimal(PayLineList[index].PlannedAmount) > Convert.ToDecimal(PayLineList[index].ActualAmount))
            //{
            //    PayLineList[index].Status = "Partially Paid";
            //}
            

            return PayLineList;
        }
    }
      
}
