
namespace DisputeResolutionCore.Dto
{
    public class DisputeRequestLogDto
    {
        public string Stan { get; set; }
        public string MaskCardPan { get; set; }
        public string TerminalId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionLogRefernce { get; set; }
        public string RetrivalNumber { get; set; }
        public decimal Amount { get; set; }

    }

    public class DisputeResponseLogDto
    {
        public string? logCode { get; set; }
        public string? issuerCode { get; set; }
        public string? issuer { get; set; }
        public string? acquirerCode { get; set; }
        public string? acquirer { get; set; }
        public string? merchantCode { get; set; }
        public string? merchant { get; set; }
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
        public string? disputeAmountType { get; set; }
        public string? disputeAmount { get; set; }
        public string? reasonCode { get; set; }
        public string? reason { get; set; }
        public string? category { get; set; }
        public string? region { get; set; }
        public bool merchantDisputant { get; set; }
        public string? domainCode { get; set; }
        public string? status { get; set; }
        public DateTime statusStartDate { get; set; }
        public List<EvidenceDto?> evidence { get; set; }
        public List<JournalDto?> journal { get; set; }
        public string? createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public string? accountNumber { get; set; }
        public string? pan { get; set; }
        public string? transactionLogReference { get; set; }
    }

    public class EvidenceDto
    {
        public long disputeId { get; set; }
        public string? uuId { get; set; }
        public string? mimeType { get; set; }
        public string? tags { get; set; }
        public string? base64EncodedBinary { get; set; }
    }

    public class JournalDto
    {
        public int disputeId { get; set; }
        public string? detail { get; set; }
        public string? addedBy { get; set; }
        public DateTime addedOn { get; set; }
    }

    public class GenericResponse<T>
    {
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }

    public class MyResponse
    {
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }
        public DisputeResponseLogDto? Data { get; set; }
    }
}