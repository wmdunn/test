using System;
using System.Collections.Generic;
using System.Text;

namespace CSV_Partials_Parser
{
    public class PayLine
    {
        private int LineCount = 0;
        public string Status { get; set; }
        public string PayHeaderNumb { get; set; }
        public string StatusDate { get; set; }
        public string PlannedDate { get; set; }
        public string PlannedAmount { get; set; }
        public string PlannedCurrency { get; set; }
        public string ActualAmount { get; set; }
        public string ActualCurrency { get; set; }
        public string ActualDate { get; set; }
        public string OffsetNumber { get; set; }
        public string OffsetAmount { get; set; }
        public string OffsetDate { get; set; }
        public string AllocationShare { get; set; }
        public string VendorNumber { get; set; }
        public string VendorLocation { get; set; }
        public string AllocationDescription { get; set; }
        public string AllocationType { get; set; }
        public string PctOrAmnt { get; set; }
        public string ReferenceNum { get; set; }
        public string Remarks { get; set; }
        public string CheckNumber { get; set; }
        public string CheckAmount { get; set; }
        public string CheckDate { get; set; }
        public string PartialsActualAmnt { get; set; }
        public string PartialsActualDate { get; set; }
        public string PartialsCheckNo { get; set; }
        public string PartialsCheckAmount { get; set; }
        public string PartialsCheckDate { get; set; }
        public string PartialsCheckReference { get; set; }
        public string PartialsCheckRemarks { get; set; }

        public PayLine(string[] fields)
        {
            LineCount = ++LineCount;
            Status = fields[0];
            PayHeaderNumb = fields[1];
            StatusDate = fields[2];
            PlannedDate = fields[3];
            PlannedAmount = fields[4];
            PlannedCurrency = fields[5];
            ActualAmount = fields[6];
            ActualCurrency = fields[7];
            ActualDate = fields[8];
            OffsetNumber = fields[9];
            OffsetAmount = fields[10];
            OffsetDate = fields[11];
            AllocationShare = fields[12];
            VendorNumber = fields[13];
            VendorLocation = fields[14];
            AllocationDescription = fields[15];
            AllocationType = fields[16];
            PctOrAmnt = fields[17];
            ReferenceNum = fields[18];
            Remarks = fields[19];
            CheckNumber = fields[20];
            CheckAmount = fields[21];
            CheckDate = fields[22];
            PartialsActualAmnt = "";
            PartialsActualDate = "";
            PartialsCheckNo = "";
            PartialsCheckAmount = "";
            PartialsCheckDate = "";
            PartialsCheckReference = "";
            PartialsCheckRemarks = "";
        }

       
        //Copy Constructor
        public PayLine(PayLine payLine)
        {
            LineCount = ++LineCount;
            Status = payLine.Status;
            PayHeaderNumb = payLine.PayHeaderNumb;
            StatusDate = payLine.StatusDate;
            PlannedDate = payLine.PlannedAmount;
            PlannedAmount = payLine.PlannedAmount;
            PlannedCurrency = payLine.PlannedCurrency;
            ActualAmount = payLine.ActualAmount;
            ActualCurrency = payLine.ActualCurrency;
            ActualDate = payLine.ActualDate;
            OffsetNumber = payLine.OffsetNumber;
            OffsetAmount = payLine.OffsetAmount;
            OffsetDate = payLine.OffsetDate;
            AllocationShare = payLine.AllocationShare;
            VendorNumber = payLine.VendorNumber;
            VendorLocation = payLine.VendorLocation;
            AllocationDescription = payLine.AllocationDescription;
            AllocationType = payLine.AllocationType;
            PctOrAmnt = payLine.PctOrAmnt;
            ReferenceNum = payLine.ReferenceNum;
            Remarks = payLine.Remarks;
            CheckNumber = payLine.CheckNumber;
            CheckAmount = payLine.CheckAmount;
            CheckDate = payLine.CheckDate;
            PartialsActualAmnt = payLine.PartialsActualAmnt;
            PartialsActualDate = payLine.PartialsActualDate;
            PartialsCheckNo = payLine.PartialsCheckNo;
            PartialsCheckAmount = payLine.PartialsCheckAmount;
            PartialsCheckDate = payLine.PartialsCheckDate;
            PartialsCheckReference = payLine.PartialsCheckReference;
            PartialsCheckRemarks = payLine.PartialsCheckRemarks;
        }

        public void checkForDelimiter(PayLine payLine)
        {
            string delimiter1 = ",";
            string delimiter2 = "\r\n";
            if(payLine.Status.Contains(delimiter1))
                payLine.Status = "\"" + payLine.Status + "\"";
            if(payLine.PayHeaderNumb.Contains(delimiter1))
                payLine.PayHeaderNumb = "\"" + payLine.PayHeaderNumb + "\"";
            //StatusDate = payLine.StatusDate;
            //PlannedDate = payLine.PlannedAmount;
            //PlannedAmount = payLine.PlannedAmount;
            //PlannedCurrency = payLine.PlannedCurrency;
            //ActualAmount = payLine.ActualAmount;
            //ActualCurrency = payLine.ActualCurrency;
            //ActualDate = payLine.ActualDate;
            //OffsetNumber = payLine.OffsetNumber;
            //OffsetAmount = payLine.OffsetAmount;
            //OffsetDate = payLine.OffsetDate;
            //AllocationShare = payLine.AllocationShare;
            //VendorNumber = payLine.VendorNumber;
            //VendorLocation = payLine.VendorLocation;
            //AllocationDescription = payLine.AllocationDescription;
            //AllocationType = payLine.AllocationType;
            //PctOrAmnt = payLine.PctOrAmnt;
            //ReferenceNum = payLine.ReferenceNum;
            //Remarks = payLine.Remarks;
            if (payLine.Remarks.Contains(delimiter1) || payLine.Remarks.Contains(delimiter2))
                payLine.Remarks = "\"" + payLine.Remarks + "\"";
            //CheckNumber = payLine.CheckNumber;
            //CheckAmount = payLine.CheckAmount;
            //CheckDate = payLine.CheckDate;
            //PartialsActualAmnt = payLine.PartialsActualAmnt;
            //PartialsActualDate = payLine.PartialsActualDate;
            //PartialsCheckNo = payLine.PartialsCheckNo;
            //PartialsCheckAmount = payLine.PartialsCheckAmount;
            //PartialsCheckDate = payLine.PartialsCheckDate;
            //PartialsCheckReference = payLine.PartialsCheckReference;
            //PartialsCheckRemarks = payLine.PartialsCheckRemarks;
            if (payLine.PartialsCheckRemarks.Contains(delimiter1) || payLine.PartialsCheckRemarks.Contains(delimiter2))
                payLine.PartialsCheckRemarks = "\"" + payLine.PartialsCheckRemarks + "\"";
        }
    }

    
    


}