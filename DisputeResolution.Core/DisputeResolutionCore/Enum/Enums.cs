using DisputeResolutionCore.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeResolutionCore.Enum
{
    public enum TransactionType
    {
        IpgTransaction, //3IPG
        AgencyBanking, //AFTR
        TransferTransaction, //WFTR
        Invalid,
    }
}
