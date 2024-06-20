using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeResolutionInfrastructure.Entity
{
    public class DisputeResponseLog
    {
        [Key]
        public long Id { get; set; }
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
        public List<Evidence> evidence { get; set; } = new List<Evidence>();
        public List<Journal> journal { get; set; } = new List<Journal>();
        public string? createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public string? accountNumber { get; set; }
        public string? pan { get; set; }
        public string? statusActions { get; set; }
        public string? transactionLogReference { get; set; }
        public Additionalinfo? Additionalinfo { get; set; }
    }

    public class Additionalinfo
    {
        [Key]
        public long Id { get; set; }
        public string? CardAcceptorCode { get; set; }
        public string? CardAcceptorLocation { get; set; }
        public string? CardScheme { get; set; }
        public bool HasMerchant { get; set; }
        public string? MerchantType { get; set; }
        public string? ResponseCode { get; set; }
        public string? ResponseDescription { get; set; }
        public string? RetrievalReferenceNumber { get; set; }
        public bool Settled { get; set; }
        public string? SinkNodeName { get; set; }
        public string? SourceNodeName { get; set; }
        public string? Stan { get; set; }
        public string? TerminalId { get; set; }

        [ForeignKey("DisputeResponseLog")]
        public long DisputeResponseLogId { get; set; }
        public DisputeResponseLog? DisputeResponseLog { get; set; }
    }

    public class Journal
    {
        [Key]
        public long id { get; set; }
        public int disputeId { get; set; }
        public string? detail { get; set; }
        public string? addedBy { get; set; }
        public DateTime addedOn { get; set; }

        [ForeignKey("DisputeResponseLog")]
        public long DisputeResponseLogId { get; set; }
        public DisputeResponseLog? DisputeResponseLog { get; set; }
    }

    public class Evidence
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public long disputeId { get; set; }
        public string? uuId { get; set; }
        public string? mimeType { get; set; }
        public string? tags { get; set; }
        public string?  base64EncodedBinary { get; set; }

        [ForeignKey("DisputeResponseLog")]
        public long DisputeResponseLogId { get; set; }
        public DisputeResponseLog? DisputeResponseLog { get; set; }

    }

}

