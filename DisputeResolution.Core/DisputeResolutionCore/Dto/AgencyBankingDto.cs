using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeResolutionCore.Dto
{
    public class AgencyBankingRequest
    {
        public int terminal { get; set; }
        public int pan { get; set; }
        public DateTime date { get; set; }
        public int stan { get; set; }
    }

    public class AgencyBankingResponse
    {
        public string?  issuerCode { get; set; }
        public string? issuer { get; set; }
        public string? acquirerCode { get; set; }
        public string? acquirer { get; set; }
        public string? merchantCode { get; set; }
        public string? merchant { get; set; }
        public string? merchantType { get; set; }
        public string? customerReference { get; set; }
        public string? transactionStore { get; set; }
        public string? transactionReference { get; set; }
        public DateTime transactionDate { get; set; }
        public string? transactionType { get; set; }
        public string? transactionAmount { get; set; }
        public string? surchargeAmount { get; set; }
        public string? transactionCurrencyCode { get; set; }
        public string? settlementAmount { get; set; }
        public string? settlementCurrencyCode { get; set; }
        public string? terminalType { get; set; }
        public string? terminalId { get; set; }
        public string? stan { get; set; }
        public string? pan { get; set; }
        public string? retrievalReferenceNumber { get; set; }
        public string? cardAcceptorCode { get; set; }
        public string? cardAcceptorLocation { get; set; }
        public string? sourceNodeName { get; set; }
        public string? sinkNodeName { get; set; }
        public string? responseCode { get; set; }
        public string? responseDescription { get; set; }
        public bool settled { get; set; }
        public bool hasMerchant { get; set; }
        public object? additionalInfo { get; set; }
        public string accountNumber { get; set; }
    }

}
