using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeResolutionInfrastructure.Entity
{
    public class DisputeResquestLog
    {
        public long Id { get; set; }
        public string Stan { get; set; }
        public string MaskCardPan { get; set; }
        public string TerminalId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionLogRefernce { get; set; }
        public string RetrivalNumber { get; set; }
        public bool IsDisputeCreated { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public DisputeResquestLog()
        {
            IsDisputeCreated = false;
        }

    }
}

