using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Partials_Parser
{
    class ParsePaylines
    {

        private List<int> FindPartials(List<PayLine> PayLineList, int index)
        {

            //PayLine temp = new PayLine(PayLineList[index]);

            List<int> PartialsIndexCount = new List<int>();
            PartialsIndexCount.Add(index);
            for (int j = index + 1; j < PayLineList.Count; j++)
            {
                if (PayLineList[index].PayHeaderNumb.Equals(PayLineList[j].PayHeaderNumb) && (PayLineList[index].PlannedDate.Equals(PayLineList[j].PlannedDate)))
                {
                    PartialsIndexCount.Add(j);
                }
            }
            return PartialsIndexCount;
        }
    }
}
