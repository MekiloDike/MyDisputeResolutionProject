using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeResolutionCore.Dto
{
    public class DisputeDto
    {

    }


    public class CreateDisputeRequest
    {
        public string transactionType { get; set; }
        public string transactionReference { get; set; }
        public string disputeAmountType { get; set; }
        public string? comment { get; set; }
        public string reasonCode { get; set; }
        public string category { get; set; }
        public decimal disputeAmount { get; set; }
    }

    public class CreateDisputeResponse
    {



    }
        public class GetDisputeResponse
        {
            public int id { get; set; }
            public string logCode { get; set; }
            public string issuerCode { get; set; }
            public string issuer { get; set; }
            public string acquirerCode { get; set; }
            public string acquirer { get; set; }
            public string merchantCode { get; set; }
            public string merchant { get; set; }
            public string customerReference { get; set; }
            public string transactionStore { get; set; }
            public string transactionReference { get; set; }
            public DateTime transactionDate { get; set; }
            public string transactionType { get; set; }
            public string transactionAmount { get; set; }
            public string surchargeAmount { get; set; }
            public string transactionCurrencyCode { get; set; }
            public string settlementAmount { get; set; }
            public string settlementCurrencyCode { get; set; }
            public string terminalType { get; set; }
            public string disputeAmountType { get; set; }
            public string disputeAmount { get; set; }
            public Additionalinfo additionalInfo { get; set; }
            public string reasonCode { get; set; }
            public string reason { get; set; }
            public string category { get; set; }
            public string region { get; set; }
            public bool merchantDisputant { get; set; }
            public string domainCode { get; set; }
            public string status { get; set; }
            public DateTime statusStartDate { get; set; }
            public object[] evidence { get; set; }
            public Journal[] journal { get; set; }
            public string createdBy { get; set; }
            public DateTime createdOn { get; set; }
            public string accountNumber { get; set; }
            public string pan { get; set; }
            public Statusaction[] statusActions { get; set; }
        }

        public class Additionalinfo
        {
            public string CardAcceptorCode { get; set; }
            public string CardAcceptorLocation { get; set; }
            public string CardScheme { get; set; }
            public bool HasMerchant { get; set; }
            public string MerchantType { get; set; }
            public string ResponseCode { get; set; }
            public string ResponseDescriqption { get; set; }
            public string RetrievalReferenceNumber { get; set; }
            public bool Settled { get; set; }
            public string SinkNodeName { get; set; }
            public string SourceNodeName { get; set; }
            public string Stan { get; set; }
            public string TerminalId { get; set; }
        }

        public class Journal
        {
            public int disputeId { get; set; }
            public string detail { get; set; }
            public string addedBy { get; set; }
            public DateTime addedOn { get; set; }
        }

        public class Statusaction
        {
            public string action { get; set; }
            public string[] required { get; set; }
        }


    
          

    
}
