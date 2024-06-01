

namespace DisputeResolutionCore.Dto
{
    public class IpgTransactionRequest
    {
        public DateTime date { get; set; }
        public int stan { get; set; }
        public int maskedCardPan { get; set; }
        public int retrievalReferenceNumber { get; set; }
        public int merchantCode { get; set; }
        
    }

    public class IpgTransactionResponse
    {
        public string? issuerCode { get; set; }
        public string? issuer { get; set; }
        public string? issuerCountry { get; set; }
        public string? acquirerCode { get; set; }
        public string? acquirer { get; set; }
        public string? acquirerCountry { get; set; }
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
        public string? settlementCurrencyCode { get; set; }
        public string? terminalType { get; set; }
        public string? stan { get; set; }
        public string? pan { get; set; }
        public string? retrievalReferenceNumber { get; set; }
        public string? responseCode { get; set; }
        public string responseDescription { get; set; }
        public bool settled { get; set; }
        public bool hasMerchant { get; set; }
    }

}
