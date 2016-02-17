using System.Collections.Generic;

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

        //private List<PayLine> Paylines;

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
            PartialsActualAmnt = null;
            PartialsActualDate = null;
            PartialsCheckNo = null;
            PartialsCheckAmount = null;
            PartialsCheckDate = null;
        }

        private void ProcessPaylines()
        {
            
        }

    }

    
    


}