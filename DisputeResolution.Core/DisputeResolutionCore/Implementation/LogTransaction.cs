using DisputeResolutionCore.Dto;
using DisputeResolutionCore.Enum;
using DisputeResolutionCore.Interface;
using DisputeResolutionInfrastructure.Context;
using DisputeResolutionInfrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisputeResolutionCore.Implementation
{
    public class LogTransaction : ILogTransaction
    {
        private readonly DisputeContext _dbContext;
        public LogTransaction(DisputeContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<GenericResponse<bool>> CreateTransactionLog(DisputeRequestLogDto Request)
        {
            
            var transactionType = GetTransactionType(Request.TransactionLogRefernce);
            if (transactionType == TransactionType.Invalid)
            {
                return new GenericResponse<bool>
                {
                    IsSuccessful = false,
                    Message = "invalid Transaction Type",
                };
            }
            

            // map DisputeRequestLogDto to DisputeRequestLog Entity
            var disputeRequestLog = new DisputeResquestLog
            {
                Stan = Request.Stan,
                MaskCardPan = Request.MaskCardPan,
                TerminalId = Request.TerminalId,
                TransactionDate = Request.TransactionDate,
                TransactionLogRefernce = Request.TransactionLogRefernce,
                RetrivalNumber = Request.RetrivalNumber,
                Amount = Request.Amount,
                TransactionType = transactionType.ToString(),

            };
            await _dbContext.DisputeRequestLogs.AddAsync(disputeRequestLog);                                  
            var saved = _dbContext.SaveChanges() > 0;
            return new GenericResponse<bool>
            {
                IsSuccessful = saved,
                Message = saved == true ? "Successfully Logged request" : "Failed to Log request",
                
            };
            
                
            
                               
        }

        public async Task<GenericResponse<DisputeResponseLogDto>> GetLoggedTransaction(string transactionReference)
        {
            var getResponse = await _dbContext.DisputeResponseLogs
                .Include(x => x.evidence)
                .Include(x => x.journal)
                .FirstOrDefaultAsync(x => x.transactionLogReference == transactionReference);

            if (getResponse == null)
            {
                return new GenericResponse<DisputeResponseLogDto> 
                { 
                    IsSuccessful = true,
                    Message = "No record found",
                    Data = null
                };                
            }

            var journalList = new List<JournalDto>();
            foreach(var item in getResponse.journal)
            {
                var journalDto = new JournalDto
                {
                    disputeId = item.disputeId,
                    addedBy = item.addedBy,
                    addedOn = item.addedOn,
                    detail = item.detail,
                };
                journalList.Add(journalDto);
            }
            var evidencelist = new List<EvidenceDto>();
            foreach(var item in getResponse.evidence)
            {
                var evidenceDto = new EvidenceDto
                {
                    base64EncodedBinary = item.base64EncodedBinary,
                    disputeId = item.disputeId,
                    uuId = item.uuId,
                    mimeType = item.mimeType,
                    tags = item.tags,
                };
                evidencelist.Add(evidenceDto);
            }

            // map DisputeRequestLog Entity to DisputeRequestLogDto
            var disputeResponseLogDto = new DisputeResponseLogDto
            {
                evidence = null,
                journal = journalList,
                logCode = getResponse.logCode,
                transactionAmount = getResponse.transactionType,
                issuerCode = getResponse.issuerCode,
                issuer = getResponse.issuer,
                acquirerCode = getResponse.acquirerCode,
                acquirer = getResponse.acquirer,
                merchantCode = getResponse.merchantCode,
                merchant = getResponse.merchant,
                customerReference = getResponse.customerReference,
                transactionType = getResponse.transactionType,
                transactionCurrencyCode = getResponse.transactionCurrencyCode,
                transactionDate = getResponse.transactionDate,
                transactionLogReference = getResponse.transactionLogReference,
                transactionStore = getResponse.transactionStore,
                surchargeAmount = getResponse.surchargeAmount,
                settlementCurrencyCode = getResponse.settlementCurrencyCode,
                settlementAmount = getResponse.settlementAmount,
                terminalType = getResponse.terminalType,
                transactionReference = getResponse.transactionReference,
                disputeAmountType = getResponse.disputeAmountType,
                accountNumber = getResponse.accountNumber,
                disputeAmount = getResponse.disputeAmount,
                domainCode = getResponse.domainCode,
                merchantDisputant = getResponse.merchantDisputant,
                statusStartDate = getResponse.statusStartDate,
                status = getResponse.status,
                category = getResponse.category,
                createdBy = getResponse.createdBy,
                createdOn = getResponse.createdOn,
                reason = getResponse.reason,
                reasonCode = getResponse.reasonCode,
                region = getResponse.region,
                pan = getResponse.pan,          
                
            };
            
            return new GenericResponse<DisputeResponseLogDto>
            {
                Data = disputeResponseLogDto,
                IsSuccessful = true,
                Message =""
            };
        }

        private TransactionType GetTransactionType(string transactionInpute)
        {
            if (transactionInpute == "3IPG")
            {
                return TransactionType.IpgTransaction;
            }
            else if(transactionInpute == "AFTR")
            {
                return TransactionType.AgencyBanking;
            }
            else if (transactionInpute == "WFTR")
            {
                return TransactionType.TransferTransaction;
            }
            
            return TransactionType.Invalid;
        }
        
    }
}
